using UnityEngine;
using System.Collections;

public class menuManager : MonoBehaviour {
	
	RaycastHit hit;
	public GameObject gameMode;
	public GameObject[] RaceCars;
	public GameObject plank;
	public Camera cam;
	private mode m;
	private bool isCarSelect;
	private int carIndex;
	// Use this for initialization
	void Start () {
		carIndex = 0;
		isCarSelect = false;
		m = gameMode.GetComponent<mode>();
		DontDestroyOnLoad(gameMode);
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetMouseButtonDown(0))
		{
			if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit))
			{
				if(hit.transform.name == "Race_Collider")
				{
					m.raceType = 1;
					cam.animation.Play("temp_menu_cam_anim");
					isCarSelect = true;
				}
				
				else if(hit.transform.name == "Time_Collider")
				{
					m.raceType = 0;
					cam.animation.Play("temp_menu_cam_anim");
					isCarSelect = true;
				}
				
				else if(hit.transform.name == "AICar")
				{
					m.carType = 0;
					Application.LoadLevel(1);
				}
				else if(hit.transform.name == "buggy car")
				{
					m.carType = 1;
					Application.LoadLevel(1);
				}	
				else if(hit.transform.name == "car buggy")
				{
					m.carType = 2;
					Application.LoadLevel(1);
				}
				else if(hit.transform.name == "PoliceCar")
				{
					m.carType = 3;
					Application.LoadLevel(1);
				}
				else if(hit.transform.name == "volswagan")
				{
					m.carType = 4;
					Application.LoadLevel(1);
				}
				
				
			}
			
			
		}
		
	
		
		
	}
	
	
	
}


