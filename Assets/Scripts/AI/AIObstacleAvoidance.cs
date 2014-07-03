using UnityEngine;
using System.Collections;

public class AIObstacleAvoidance : MonoBehaviour {
	
	private AICarControllerScript _AICar;
	Vector3 position;
	RaycastHit hit;
	// Use this for initialization
	void Start () {
		
		_AICar = transform.GetComponent<AICarControllerScript>();
	
	}
	
	// Update is called once per frame
	void Update () {
		position = transform.position+new Vector3(0,0.3f,0);
		
		 if(Physics.Raycast(position,(-transform.forward + transform.right*0.5f),out hit,5))
		{
			if(hit.transform.tag == "Obstacle")
			{
				
				transform.RotateAroundLocal(Vector3.up,0.25f);
			
			}
		}
		
		else if(Physics.Raycast(position,(-transform.forward  - transform.right*0.5f),out hit,5))
		{
			
			if(hit.transform.tag == "Obstacle")
			
			{
				transform.RotateAroundLocal(Vector3.up,0.25f);
			
			}
		}
		
		else if(Physics.Raycast(position,-transform.forward,out hit,5))
		{
				if(hit.transform.tag == "Obstacle")
			
			{
				transform.RotateAroundLocal(Vector3.up,0.25f);
			}
		}
		
		
		
	}
	
	
	void OnCollisionEnter(Collision col)
	{
		if(col.transform.tag == "Obstacle")
		{
			transform.RotateAroundLocal(Vector3.up,0.25f);
		}
	}
}
