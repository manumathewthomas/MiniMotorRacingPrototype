using UnityEngine;
using System.Collections;

public class OnScreenGUIScript : MonoBehaviour {
	
	public GameObject Car;
	private MiniVechicleControllerScript _vehicleControl;
	private float FrontSuspensionSpring,frontSuspensionSpring;
	private float RearSuspensionSpring,rearSuspensionSpring;
	private float DragCoefficent,dragCoefficent;
	private float RollingResistanceCoeffient,rollingResistanceCoefficent;
	private float EngineBraking,engineBraking;
	private float BrakeTorque,brakeTorque;
	private float DriftFactor,driftFactor;
	
	// Use this for initialization
	void Start () {
		_vehicleControl = Car.GetComponent<MiniVechicleControllerScript>();
		frontSuspensionSpring = _vehicleControl.suspensionSpringFront;
		rearSuspensionSpring = _vehicleControl.suspensionSpringRear;
		dragCoefficent = _vehicleControl.coefDrag;
		rollingResistanceCoefficent = _vehicleControl.coefRollingResistance;
		engineBraking = _vehicleControl.coefEngineBraking;
		brakeTorque = _vehicleControl.brakeTorque;
		driftFactor = _vehicleControl.driftFactor;
		
		DefaultValues();
	}
	
	void DefaultValues()
	{
		FrontSuspensionSpring = frontSuspensionSpring;
		RearSuspensionSpring = rearSuspensionSpring;
		DragCoefficent = dragCoefficent;
		RollingResistanceCoeffient = rollingResistanceCoefficent;
		EngineBraking = engineBraking;
		BrakeTorque = brakeTorque;
		DriftFactor = driftFactor;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	

	
	void OnGUI()
	{
		GUI.Box(new Rect(Screen.width-250,10,200,350),"Settings");
		
		GUI.Label(new Rect(Screen.width-220, 25, 150, 30), "Front Suspension Spring");
		FrontSuspensionSpring = GUI.HorizontalSlider(new Rect(Screen.width-220, 45, 100, 40), FrontSuspensionSpring, 5000.0F, 20000.0F);
		GUI.Label(new Rect(Screen.width-100, 40, 150, 30),FrontSuspensionSpring.ToString());
		_vehicleControl.suspensionSpringFront = FrontSuspensionSpring;
		
		
		GUI.Label(new Rect(Screen.width-220, 55, 150, 60), "Rear Suspension Spring");
		RearSuspensionSpring = GUI.HorizontalSlider(new Rect(Screen.width-220, 75, 100, 40), RearSuspensionSpring,5000.0F, 20000.0F);
		GUI.Label(new Rect(Screen.width-100, 70, 150, 30),RearSuspensionSpring.ToString());
		_vehicleControl.suspensionSpringRear = RearSuspensionSpring;	
		
		GUI.Label(new Rect(Screen.width-220, 85, 150, 60), "Drag Coefficent");
		DragCoefficent = GUI.HorizontalSlider(new Rect(Screen.width-220, 110, 100, 40), DragCoefficent, 0.0F, 1.0F);
		GUI.Label(new Rect(Screen.width-100, 100, 150, 30),DragCoefficent.ToString());
		_vehicleControl.coefDrag = DragCoefficent;	
		
		GUI.Label(new Rect(Screen.width-220, 115, 150, 150), "Rolling Resistance Coefficent");
		RollingResistanceCoeffient = GUI.HorizontalSlider(new Rect(Screen.width-220, 145, 100, 40), RollingResistanceCoeffient, 0.0F, 1.0F);
		GUI.Label(new Rect(Screen.width-100, 130, 150, 30),RollingResistanceCoeffient.ToString());
		_vehicleControl.coefRollingResistance = RollingResistanceCoeffient;
		
		GUI.Label(new Rect(Screen.width-220, 165, 150, 150), "Engine Braking Coefficent");
		EngineBraking = GUI.HorizontalSlider(new Rect(Screen.width-220, 200, 100, 40), EngineBraking, 0.0F, 1.0F);
		GUI.Label(new Rect(Screen.width-100, 180, 150, 30),EngineBraking.ToString());
		_vehicleControl.coefEngineBraking = EngineBraking;
		
		GUI.Label(new Rect(Screen.width-220, 210, 150, 60), "Brake Torque");
		BrakeTorque = GUI.HorizontalSlider(new Rect(Screen.width-220, 235, 100, 40), BrakeTorque, 0.0F, 5000F);
		GUI.Label(new Rect(Screen.width-100, 230, 150, 30),BrakeTorque.ToString());
		_vehicleControl.brakeTorque = BrakeTorque;
		
		GUI.Label(new Rect(Screen.width-220, 240, 150, 60), "Drift Factor");
		DriftFactor = GUI.HorizontalSlider(new Rect(Screen.width-220, 270, 100, 40), DriftFactor, 0.0F, 1000f);
		GUI.Label(new Rect(Screen.width-100, 280, 150, 30), DriftFactor.ToString());
		_vehicleControl.driftFactor = DriftFactor;
		
		
		if (GUI.Button(new Rect(Screen.width-220, 300, 150, 60), "Reset"))
		{
			FrontSuspensionSpring = frontSuspensionSpring;
			RearSuspensionSpring = rearSuspensionSpring;
			DragCoefficent = dragCoefficent;
			RollingResistanceCoeffient = rollingResistanceCoefficent;
			EngineBraking = engineBraking;
			BrakeTorque = brakeTorque;
			DriftFactor = driftFactor;
		}
	
	}
}
