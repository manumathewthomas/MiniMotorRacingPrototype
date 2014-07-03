using UnityEngine;
using System.Collections;

public class CameraSwitchScript : MonoBehaviour {
	
	public Camera cam1;
	public Camera cam2;
	public Camera cam3;
	public Camera cam4;
	public Camera cam5;
	public Camera cam6;
	

	// Use this for initialization
	public Camera GUICamera;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		
			
			if(Input.GetMouseButtonDown(0))
			{
				RaycastHit hit = new RaycastHit();
				Ray ray = GUICamera.ScreenPointToRay(Input.mousePosition);
				
				if(Physics.Raycast(ray,out hit))
				{
					if(hit.transform.name == "Cam1")
					{
						cam1.gameObject.SetActive(true);
						cam2.gameObject.SetActive(false);
						cam3.gameObject.SetActive(false);
						cam4.gameObject.SetActive(false);
						cam5.gameObject.SetActive(false);
						cam6.gameObject.SetActive(false);
						
					}
					
					if(hit.transform.name == "Cam2")
					{
						cam1.gameObject.SetActive(false);
						cam2.gameObject.SetActive(true);
						cam3.gameObject.SetActive(false);
						cam4.gameObject.SetActive(false);
						cam5.gameObject.SetActive(false);
						cam6.gameObject.SetActive(false);
						
					}
					
					if(hit.transform.name == "Cam3")
					{
					Debug.Log("dsf");
						cam1.gameObject.SetActive(false);
						cam2.gameObject.SetActive(false);
						cam3.gameObject.SetActive(true);
						cam4.gameObject.SetActive(false);
						cam5.gameObject.SetActive(false);
						cam6.gameObject.SetActive(false);
						
					}
					
					if(hit.transform.name == "Cam4")
					{
						cam1.gameObject.SetActive(false);
						cam2.gameObject.SetActive(false);
						cam3.gameObject.SetActive(false);
						cam4.gameObject.SetActive(true);
						cam5.gameObject.SetActive(false);
						cam6.gameObject.SetActive(false);
						
					}
				
					if(hit.transform.name == "Cam5")
					{
						cam1.gameObject.SetActive(false);
						cam2.gameObject.SetActive(false);
						cam3.gameObject.SetActive(false);
						cam4.gameObject.SetActive(false);
						cam5.gameObject.SetActive(true);
						cam6.gameObject.SetActive(false);
						
					}
				
					if(hit.transform.name == "Cam6")
					{
						cam1.gameObject.SetActive(false);
						cam2.gameObject.SetActive(false);
						cam3.gameObject.SetActive(false);
						cam4.gameObject.SetActive(false);
						cam5.gameObject.SetActive(false);
						cam6.gameObject.SetActive(true);
						
					}
						if(hit.transform.name == "Reset")
					{
					Application.LoadLevel(0);
						
					}
				}
			}
		
	
	}
}
