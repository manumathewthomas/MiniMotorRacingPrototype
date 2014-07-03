using UnityEngine;
using System.Collections;

public class BrakeArea : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.transform.tag =="Car"&&col.GetComponent<AICarControllerScript>().enabled)
		{
			col.GetComponent<AICarControllerScript>().AIThrottleSpeed = 0.3f;
		}
	}
	
	void OnTriggerStay(Collider col)
	{
		if(col.transform.tag == "Car" && col.GetComponent<AICarControllerScript>().enabled)
		{
			col.GetComponent<AICarControllerScript>().AIThrottleSpeed = 0.2f;
		}
	}
	
	void OnTriggerExit(Collider col)
	{
		if(col.transform.tag =="Car"&&col.GetComponent<AICarControllerScript>().enabled)
		{
			col.GetComponent<AICarControllerScript>().AIThrottleSpeed = 0f;
		}
	}
}
