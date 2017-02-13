using UnityEngine;
using System.Collections;

public class ReactableObject : MonoBehaviour
{

	protected virtual void OnMoveOver()
	{

	}


	protected virtual void OnMoveOut()
	{

	}

	protected virtual IEnumerator OnPressing()
	{
		var camera = Camera.main;
		if (camera) {
			//转换对象到当前屏幕位置
			Vector3 screenPosition = camera.WorldToScreenPoint (transform.position);

			//鼠标屏幕坐标
			Vector3 mScreenPosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
			//获得鼠标和对象之间的偏移量,拖拽时相机应该保持不动
			Vector3 offset = transform.position - camera.ScreenToWorldPoint (mScreenPosition);
			print ("drag starting:" + transform.name);
			lastPosition = transform.position;
			//若鼠标左键一直按着则循环继续
			while (Input.GetMouseButton (0)) {
				//鼠标屏幕上新位置
				mScreenPosition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
				// 对象新坐标 
				Collider c = GetComponent<Collider>();

				transform.position = offset + camera.ScreenToWorldPoint (mScreenPosition);
				if (transform.position.x + c.bounds.extents.x > rightBorder) {
					transform.position = new Vector3(rightBorder - c.bounds.extents.x,transform.position.y,transform.position.z);
				}
				if (transform.position.x - c.bounds.extents.x < leftBorder) {
					transform.position = new Vector3(leftBorder + c.bounds.extents.x,transform.position.y,transform.position.z);
				}
				if (transform.position.y + c.bounds.extents.y > topBorder) {
					transform.position = new Vector3(transform.position.x,topBorder - c.bounds.extents.y,transform.position.z);
				}
				if (transform.position.y - c.bounds.extents.y < downBorder) {
					transform.position = new Vector3(transform.position.x,downBorder + c.bounds.extents.y,transform.position.z);
				}

				nextRecord += Time.deltaTime;
				//协同，等待下一帧继续
				//if (nextRecord > 0.05) {
					releaseVector = mScreenPosition - lastPosition;
					lastPosition = mScreenPosition;
					nextRecord = 0.0;
				//}
				yield return new WaitForFixedUpdate ();
			}
			print ("drag compeleted");

			}
	}

	protected virtual void OnPressDown()
	{

	}

	protected virtual void OnPressUp()
	{
	}

	protected virtual void OnSlice()
	{
	}

	protected bool IsButtonPressed { get { return _isButtonPressed; } }
	protected bool IsButtonHovered { get { return _isButtonHovered; } }


	protected virtual void Awake()
	{
		if (_buttonCollider == null)
		{
			_buttonCollider = GetComponent<Collider>();
			if (_buttonCollider == null)
			{
				_buttonCollider = GetComponentInChildren<Collider>();
				if (_buttonCollider)
				{
					Debug.LogError("Couldn't find a collider for button");
				}
			}
		}
	}

	protected virtual void Start()
	{
		Vector3 cornerPos=Camera.main.ViewportToWorldPoint(new Vector3(1f,1f,
			Mathf.Abs(-Camera.main.transform.position.z)));

		leftBorder=Camera.main.transform.position.x-(cornerPos.x-Camera.main.transform.position.x);
		rightBorder=cornerPos.x;
		topBorder=cornerPos.y;
		downBorder=Camera.main.transform.position.y-(cornerPos.y-Camera.main.transform.position.y);

		width=rightBorder-leftBorder;
		height=topBorder-downBorder;

	}

	protected virtual void Update()
	{
		if (HitButtonArea())
		{
			_isButtonHovered = true;
			if (Application.platform == RuntimePlatform.IPhonePlayer || 
				Application.platform == RuntimePlatform.Android)
			{
				OnMoveOver();
			}

			if (Input.GetMouseButtonDown(0))
			{
				_isButtonPressed = true;
				OnPressDown ();
				StartCoroutine (OnPressing());
			}

			else if (Input.GetMouseButtonUp(0) && _isButtonPressed)
			{
				_isButtonPressed = false;
				OnPressUp();
			}
		}
		else if (_isButtonHovered)
		{
			if (_isButtonPressed) {
				_isButtonPressed = false;
				OnSlice ();
			}
			_isButtonHovered = false;
			OnMoveOut();
		}
	}



	private bool HitButtonArea()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			return hit.collider == _buttonCollider;
		}
		return false;
	}

	private Collider _buttonCollider;
	private bool _isButtonHovered;
	private bool _isButtonPressed;

	[HideInInspector]public float leftBorder;
	[HideInInspector]public float rightBorder;
	[HideInInspector]public float topBorder;
	[HideInInspector]public float downBorder;
	private float width;
	private float height;

	public Vector3 releaseVector;
	private Vector3 lastPosition;
	private double nextRecord = 0.0;
}
