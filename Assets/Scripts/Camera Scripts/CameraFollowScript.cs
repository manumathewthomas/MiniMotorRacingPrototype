using UnityEngine;
using System.Collections;

public class CameraFollowScript : MonoBehaviour {
	public GameObject[] manager;
	public GameObject player;
	mode race;
	// Use this for initialization
	void Start () {
	race = GameObject.Find("mode").GetComponent<mode>();
	player = manager[race.carType];
		
	if(race.raceType == 0)
		{
	
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		transform.position = new Vector3(player.transform.position.x-15,transform.position.y,player.transform.position.z-10);
		transform.LookAt(player.transform);
		
	}
}
