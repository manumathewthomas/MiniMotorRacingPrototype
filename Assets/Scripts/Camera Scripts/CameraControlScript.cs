
using UnityEngine;
using System.Collections;

public class CameraControlScript : MonoBehaviour {
	
	private Transform car;
	public float distance = 120f;
	public float height = 70f;
	public float rotationDamping = 1.0f;
	public float heightDamping = 2.0f;
	public float zoomRatio = 0.5f;
	public float DefaultFOV = 60;
	public GameObject[] cars;
	public GameObject player;
	
	
	void Start()
	{
	
		foreach(GameObject car in cars)
		{
			if(car.GetComponent<MiniVechicleControllerScript>().enabled)
			{
				player = car;
			}
			
		}
	}
	
	void LateUpdate()
	{
		float wantedAngle = car.eulerAngles.y;
		float wantedHeight = car.position.y + height;
		float myAngle = transform.eulerAngles.y;
		float myHeight = transform.position.y;
		
		myAngle = Mathf.LerpAngle(myAngle,wantedAngle,rotationDamping * Time.deltaTime);
		myHeight = Mathf.Lerp(myHeight,wantedHeight,heightDamping * Time.deltaTime);
		
		Quaternion currentRotation = Quaternion.Euler(0,myAngle,0);
		transform.position = car.position;
		transform.position -=currentRotation * Vector3.forward * distance;
		transform.position = new Vector3(transform.position.x,myHeight,transform.position.z);
		transform.LookAt(player.transform);
	}

}
