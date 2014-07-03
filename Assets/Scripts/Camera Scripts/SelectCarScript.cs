using UnityEngine;
using System.Collections;

public class SelectCarScript : MonoBehaviour {
	
	public Transform Car1;
	public Transform Car2;
	public Transform Car3;
	
	public GameObject gui;
	
	public Camera TopCamera;
	
	public GameObject Wordings;


	// Use this for initialization
	

	void Start () {
		gui.SetActive(false);
	
		
	}
	
	// Update is called once per frame
	void Update () {
		
		foreach(Touch touch in Input.touches)
		{
			
			if(Input.touchCount > 0 && touch.phase == TouchPhase.Began)
			{
				RaycastHit hit = new RaycastHit();
				Ray ray = transform.camera.ScreenPointToRay(touch.position);
				
				if(Physics.Raycast(ray,out hit))
				{
					if(hit.transform.name == "Car1")
					{
						Car2.gameObject.SetActive(false);
						Car3.gameObject.SetActive(false);
						gui.SetActive(true);
						transform.camera.gameObject.SetActive(false);
						TopCamera.gameObject.SetActive(true);
						Wordings.SetActive(false);
						
					}
					
					else if(hit.transform.name == "Car2")
					{
						
						Car1.gameObject.SetActive(false);
						Car3.gameObject.SetActive(false);
						gui.SetActive(true);
						transform.camera.gameObject.SetActive(false);
						TopCamera.gameObject.SetActive(true);
						Wordings.SetActive(false);
						
					}
					
					else if(hit.transform.name == "Car3")
					{
						Car2.gameObject.SetActive(false);
						Car1.gameObject.SetActive(false);
						gui.SetActive(true);
						transform.camera.gameObject.SetActive(false);
						TopCamera.gameObject.SetActive(true);
						Wordings.SetActive(false);
						
					}
				}	
	
			}
		}
	
	}
}
