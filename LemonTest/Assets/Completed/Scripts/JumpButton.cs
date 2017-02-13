using UnityEngine;
using System.Collections;


public class JumpButton : ReactableObject
{
	private float a = 5f;
	private float speed;
	private float cd = 0f;

	protected override void Awake()
	{
		base.Awake();

	}

	protected override void Start()
	{
		base.Start ();
		transform.position = new Vector3 (rightBorder - 2, topBorder - 2, transform.position.z);
	}

	protected override void OnMoveOut()
	{
	}



	protected override void OnPressDown()
	{

		Debug.Log("Mouse Down!");

		GameObject player = GameObject.Find("Player");
		player.GetComponent<Rotate>().OnJump();

	}



	protected override void OnSlice()
	{
		speed = releaseVector.magnitude;
		StartCoroutine (OnSlicing());
	}

	protected virtual IEnumerator OnSlicing()
	{
		while (speed > 0)
		{
			if (cd > 0.02) {
				transform.position += releaseVector.normalized * speed * Time.deltaTime;
				speed -= a;
				cd = 0;
			}
			cd += Time.deltaTime;
			yield return new WaitForFixedUpdate ();
		}
		print ("slice done");
	}


}