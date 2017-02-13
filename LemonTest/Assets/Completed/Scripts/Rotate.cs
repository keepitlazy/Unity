using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {
    public GameObject centerObj;
    public float speed = 10f;
    public float yspeed = 3f;
    public bool isJumping = false;
    private float yPosition;

    Quaternion qua;

    // Use this for initialization  
    void Start()
    {

    }

    // Update is called once per frame  
    void Update()
    {

        if (centerObj != null)
        {
            centerObj = GameObject.Find("Cube");
            //roateObj围绕centerObj旋转，x,y不旋转  
            transform.RotateAround(centerObj.transform.position, new Vector3(0, 1, 0), speed * Time.deltaTime);
            //这里处理不然roateObj图片的显示位置发生变化  
            qua = transform.rotation;
            qua.z = 0;
            transform.rotation = qua;
        }

        if (isJumping)
        {
            if (yPosition > transform.position.y)
            {
				transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);
				yspeed = 10f;
                isJumping = false;
            }
            else
            {
                transform.position += transform.up * yspeed * Time.deltaTime;
                yspeed -= 19.8f * Time.deltaTime;
            }
        }

    }

    public void OnJump()
    {
		if (!isJumping) {
			isJumping = true;

			yPosition = transform.position.y;
		}
    }

}
