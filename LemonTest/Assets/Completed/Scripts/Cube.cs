using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : ReactableObject {

	protected override void Awake()
	{
		base.Awake();

	}

	protected override IEnumerator OnPressing()
	{
		var camera = Camera.main;
		if (camera) {
			Vector3 screenPosition = camera.WorldToScreenPoint (transform.position);

			Vector3 mScreenPosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
			print ("drag starting:" + transform.name);

			while (Input.GetMouseButton (0)) {
				mScreenPosition = new Vector3 (Input.mousePosition.x, screenPosition.y, screenPosition.z);

				transform.position = camera.ScreenToWorldPoint (mScreenPosition);
				yield return new WaitForFixedUpdate ();
			}

			print ("drag compeleted");

		}
	}
}
