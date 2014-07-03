using UnityEngine;
using System.Collections;

public class CameraPerspectiveScript : MonoBehaviour {
	

	
	public GameObject[] cars;
	public Transform player;
	// Use this for initialization
	void Start () {
		
		foreach(GameObject car in cars)
		{
			if(car.GetComponent<MiniVechicleControllerScript>().enabled)
			{
				player = car.transform;
			}
			
		}
		
	
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.position = new Vector3(player.position.x-15,transform.position.y,player.position.z);
	
	}
}
