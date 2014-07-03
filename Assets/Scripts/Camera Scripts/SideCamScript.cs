using UnityEngine;
using System.Collections;

public class SideCamScript : MonoBehaviour {

	private CameraFollowScript _camera;
	public GameObject player;
	// Use this for initialization
	void Start () {
		
		_camera = transform.GetComponent<CameraFollowScript>();
		player = _camera.player;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		//transform.position = new Vector3(player.transform.position.x-15,transform.position.y,player.transform.position.z-10);
	}
}
