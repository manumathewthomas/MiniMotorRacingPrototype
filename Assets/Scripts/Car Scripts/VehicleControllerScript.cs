using UnityEngine;
using System.Collections;

public class VehicleControllerScript : MonoBehaviour {
	
	
	public WheelCollider WheelFLCollider;
	public WheelCollider WheelFRCollider;
	public WheelCollider WheelBLCollider;
	public WheelCollider WheelBRCollider;
	
	public Transform WheelFL;
	public Transform WheelFR;
	public Transform WheelBL;
	public Transform WheelBR;
	
	
	public float steerMax = 30.0f;
	public float motorMax = 30.0f;
	public float brakeMax = 30.0f;
	
	public float steer = 0;
	public float motor = 0;
	public float brake = 0;
	
	
	public bool isBraked = true;
	public bool isGasOn = false;
	
	public float steerDirection = 0;
	
	public Transform Cog;
	
	public bool isWheelUpdate = false;
	
	public bool autoSteer =false;

	// Use this for initialization
	void Start () {
		rigidbody.centerOfMass = Cog.localPosition;
	}
	
	
	void FixedUpdate()
	{
		if(isGasOn)
		{
			WheelBLCollider.motorTorque = motor * motorMax;
			WheelBRCollider.motorTorque = motor * motorMax;
		}
	
		if(isBraked)
		{
			WheelBLCollider.brakeTorque = brake * brakeMax;
			WheelBRCollider.brakeTorque = brake * brakeMax;
			
			WheelBLCollider.motorTorque = 0;
			WheelBRCollider.motorTorque = 0;
		}
		
		WheelFLCollider.steerAngle = steerDirection * steerMax;
		WheelFRCollider.steerAngle = steerDirection * steerMax;
		
		
		WheelFL.localEulerAngles = new Vector3(WheelFL.localEulerAngles.x,WheelFLCollider.steerAngle + 90,WheelFL.localEulerAngles.z);
		WheelFR.localEulerAngles = new Vector3(WheelFR.localEulerAngles.x,WheelFRCollider.steerAngle + 90,WheelFR.localEulerAngles.z);
		
		WheelBL.Rotate(0,0,WheelBLCollider.rpm * -6 *Time.deltaTime);
		WheelBR.Rotate(0,0,WheelBRCollider.rpm * -6 *Time.deltaTime);
		WheelFL.Rotate(0,0,WheelFLCollider.rpm * -6 *Time.deltaTime);
		WheelFR.Rotate(0,0,WheelFRCollider.rpm * -6 *Time.deltaTime);
	}
	
	// Update is called once per frame
	void Update () {
		
		if(isGasOn)
		{
			motor += 0.05f;
			if(motor>=1)
				motor = 1;
		}
		else
		{
			
			motor -= 0.05f;
			if(motor<=0)
				motor = 0;
		}
		
		if(isBraked)
		{
			brake += 0.5f;
			if(brake>=1)
				brake = 1;
		}
		else
		{
			brake =0;
		}
		
		if(isWheelUpdate)
		{
			UpdateWheelHeight(WheelBL,WheelBLCollider);
			UpdateWheelHeight(WheelBR,WheelBRCollider);
			UpdateWheelHeight(WheelFL,WheelFLCollider);
			UpdateWheelHeight(WheelFR,WheelFRCollider);
		}
	
	
	}
	
	
	void UpdateWheelHeight(Transform wheelTransform,WheelCollider collider)
	{
		RaycastHit hit = new RaycastHit();
		Vector3 wheelPosition = wheelTransform.position;
		
		
	}
}
