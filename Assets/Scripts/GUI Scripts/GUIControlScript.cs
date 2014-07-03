using UnityEngine;
using System.Collections;

public class GUIControlScript : MonoBehaviour {

	public GUITexture gasGUI;
	public GUITexture brakeGUI;
	public Transform driveGUI;
	public Transform drivePoint;
	public Camera GUICamera;
	
	
	
	private VehicleControllerScript _vehicleControl;
	private float drivePointOffset;
	private float driveOffset;
	private float driveGUILastAngle;
	
	private bool isDrive =false;
	
	
	


	
	void Start()
	{
	
		
	}
	
	void OnEnable()
	{
			foreach(GameObject Car in GameObject.FindGameObjectsWithTag("Car"))
		{
			if(Car.activeSelf)
			{
				_vehicleControl = Car.GetComponent<VehicleControllerScript>();
				break;
			}
			
		}
	}
	
	
	void Update()
	{
		TouchControls();
		
		if(!isDrive && _vehicleControl.steerDirection!=0 && _vehicleControl.autoSteer)
		{
			if(_vehicleControl.steerDirection< 0)
				_vehicleControl.steerDirection += 0.05f;
			else if(_vehicleControl.steerDirection > 0)
				_vehicleControl.steerDirection -= 0.05f;
		}
	}
	
	
	void TouchControls()
	{
		
		foreach(Touch touch in Input.touches)
		{
			
			if(Input.touchCount > 0 && touch.phase == TouchPhase.Began)
			{
				RaycastHit hit = new RaycastHit();
				Ray ray = GUICamera.ScreenPointToRay(touch.position);
				
				if(Physics.Raycast(ray,out hit))
				{
					if(hit.transform.tag == "driveGUI")
					{
						drivePoint.LookAt(hit.point);
					
						drivePointOffset = drivePoint.eulerAngles.y;
						driveOffset = driveGUI.eulerAngles.y;
					}
				}
			}
			
			else if(Input.touchCount > 0 && touch.phase == TouchPhase.Stationary)
			{
				if(gasGUI.HitTest(touch.position,GUICamera))
				{
					_vehicleControl.isGasOn = true;
					_vehicleControl.isBraked = false;
					
				}
			
				if(brakeGUI.HitTest(touch.position,GUICamera))
				{
					
					_vehicleControl.isBraked = true;
					_vehicleControl.isGasOn = false;
				
				}
				
			}
			
			else if(Input.touchCount > 0 && touch.phase == TouchPhase.Moved)
			{
				RaycastHit hit = new RaycastHit();
				Ray ray = GUICamera.ScreenPointToRay(touch.position);
				
				if(Physics.Raycast(ray,out hit))
				{
					if(hit.transform.tag == "driveGUI")
					{
						isDrive = true;
						drivePoint.LookAt(hit.point);
						driveGUI.eulerAngles = new Vector3(driveGUI.eulerAngles.x,driveOffset + (drivePoint.eulerAngles.y - drivePointOffset),driveGUI.eulerAngles.z);
						
						if(driveGUI.eulerAngles.y - driveGUILastAngle < -300)
							driveGUILastAngle = 0;
						else if(driveGUI.eulerAngles.y - driveGUILastAngle > 300)
							driveGUILastAngle = 360;
							
						if(driveGUI.eulerAngles.y>driveGUILastAngle)
						{
							Debug.Log("Clockwise");
							_vehicleControl.steerDirection += 0.05f;
							if(_vehicleControl.steerDirection>=1)
								_vehicleControl.steerDirection = 1;
						}
						else
						{	
							Debug.Log("Anti-Clockwise");
							_vehicleControl.steerDirection -= 0.05f;
							if(_vehicleControl.steerDirection<=-1)
								_vehicleControl.steerDirection = -1;
							
						}
						
						driveGUILastAngle = driveGUI.eulerAngles.y;
						
						
					}
				}
				
			}
			
			else if(Input.touchCount > 0 && touch.phase == TouchPhase.Ended)
			{
				if(gasGUI.HitTest(touch.position,GUICamera))
				{
					_vehicleControl.isGasOn = false;
					_vehicleControl.isBraked = true;
				}
				
				RaycastHit hit = new RaycastHit();
				Ray ray = GUICamera.ScreenPointToRay(touch.position);
				
					if(Physics.Raycast(ray,out hit))
				{
					if(hit.transform.tag == "driveGUI")
					{
						isDrive = false;
						
					}
				}
			}
		}
	}
	
	
}
