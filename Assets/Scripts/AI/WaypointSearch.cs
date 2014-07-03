using UnityEngine;
using System.Collections;

public class WaypointSearch : MonoBehaviour {
	
	AICarControllerScript AICar;
	MiniVechicleControllerScript PlayerCar;
	Vector3 position;
	public int waypointNumber;
	RaycastHit hit;
	
	// Use this for initialization
	void Start () {
	
		
		position = transform.position + new Vector3(0,0.3f,0);
	
	}
	
	// Update is called once per frame
	void Update () {
		
		
	if(Physics.Raycast(position,transform.right,out hit,2))	
		{
			if(hit.transform.tag == "Car")
				
			{
				changeWayPoint(hit);
			}
		}
		
		if(Physics.Raycast(position,-transform.right,out hit,2))
		{
			if(hit.transform.tag == "Car")
				
			{
				changeWayPoint(hit);
			}
		}
		
		if(Physics.Raycast(position,transform.forward,out hit,2))
		{
			
			if(hit.transform.tag == "Car")
				
			{
				
				changeWayPoint(hit);
			}
		}
		
		if(Physics.Raycast(position,-transform.forward,out hit,2))
		{
			
			if(hit.transform.tag == "Car")
				
			{
				
				changeWayPoint(hit);
			}
		}
		Debug.DrawRay(position,transform.right*2.0f,Color.blue);
		Debug.DrawRay(position,-transform.right*2.0f,Color.blue);
		Debug.DrawRay(position,transform.forward*2.0f,Color.red);
		Debug.DrawRay(position,-transform.forward*2.0f,Color.red);
	
	}
	
	void changeWayPoint(RaycastHit hit)
	{

		if(hit.transform.GetComponent<AICarControllerScript>().enabled)
		{
			AICar = hit.transform.GetComponent<AICarControllerScript>();
			if(waypointNumber > AICar.curWaypoint)
			{
				AICar.curWaypoint = waypointNumber;
			}
			else if(waypointNumber >= AICar.waypointLength)
			{
		   			AICar.curWaypoint = 0;
			}
		}
		
		else if(hit.transform.GetComponent<MiniVechicleControllerScript>().enabled)
		{
			PlayerCar = hit.transform.GetComponent<MiniVechicleControllerScript>();
			if(waypointNumber > PlayerCar.curWaypoint)
			{
				PlayerCar.curWaypoint = waypointNumber;
			}
			else if(waypointNumber >= PlayerCar.waypointLength)
			{
		   			PlayerCar.curWaypoint = 0;
			}
		}
	}
}
