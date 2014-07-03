using UnityEngine;
using System.Collections;

public class CarControlScript : MonoBehaviour {

	public WheelCollider wheelFL;
	public WheelCollider wheelFR;
	public WheelCollider wheelBL;
	public WheelCollider wheelBR;
	
	public Transform wheelFLTransform;
	public Transform wheelFRTransform;
	public Transform wheelBLTransform;
	public Transform wheelBRTransform;
	
	public float currentSpeed;
	public float topSpeed = 150;
	public float maxTorque = 150;
	public float speed = 20;
	public float maxBrakeTorque = 100;
	
	public float lowestSteerSpeed = 50;
	public float lowSpeedSteerAngle = 60;
	public float highSpeedSteerAngle = 1;
	public float steerDirection = 0;
	
	public bool isBraked = false;
	public bool isGasOn = false;
	
	public void Start()
	{
		
	}
	
	public void FixedUpdate()
	{
		
	}
	
	
	public void Update()
	{
		wheelFLTransform.Rotate(((wheelFL.rpm / 60) * 360) * Time.deltaTime,0,0);
		wheelFRTransform.Rotate(((wheelFR.rpm / 60) * 360) * Time.deltaTime,0,0);
		wheelBLTransform.Rotate(((wheelBL.rpm / 60) * 360) * Time.deltaTime,0,0);
		wheelBRTransform.Rotate(((wheelBR.rpm / 60) * 360) * Time.deltaTime,0,0);
		
		wheelFLTransform.localEulerAngles = new Vector3(wheelFLTransform.localEulerAngles.x,wheelFL.steerAngle - wheelFLTransform.localEulerAngles.z,wheelFLTransform.localEulerAngles.z);
		wheelFRTransform.localEulerAngles = new Vector3(wheelFRTransform.localEulerAngles.x,wheelFR.steerAngle - wheelFRTransform.localEulerAngles.z,wheelFRTransform.localEulerAngles.z);
		
		Control();
		HandBrake();
	}
	
	
	public void Control()
	{
		if(isGasOn)
		{
			currentSpeed = 2 * Mathf.PI * wheelBL.radius * wheelBL.rpm * 60/1000;
			currentSpeed = Mathf.Round(currentSpeed);
		
			if(currentSpeed <= topSpeed)
			{
				wheelBL.motorTorque = maxTorque;
				wheelBR.motorTorque = maxTorque;
			}
		
			else
			{
				wheelBL.motorTorque = 0;
				wheelBR.motorTorque = 0;
			}
		}
		
		else
		{
			wheelBL.motorTorque = 0;
			wheelBR.motorTorque = 0;
		}
		
		float speedFactor = rigidbody.velocity.magnitude/lowestSteerSpeed;
		float currentSteerAngle = Mathf.Lerp(lowSpeedSteerAngle,highSpeedSteerAngle,speedFactor);
		
		currentSteerAngle *= steerDirection;
		wheelFL.steerAngle = currentSteerAngle * 3;
		wheelFR.steerAngle = currentSteerAngle * 3;
	}
	
	public void HandBrake()
	{
		if(isBraked)
		{
			wheelBL.brakeTorque = maxBrakeTorque;
			wheelBR.brakeTorque = maxBrakeTorque;
			
			wheelBL.motorTorque = 0;
			wheelBR.motorTorque = 0;
		}
		
		else
		{
			wheelBL.brakeTorque = 0;
			wheelBR.brakeTorque = 0;
		}
		
	}
}
