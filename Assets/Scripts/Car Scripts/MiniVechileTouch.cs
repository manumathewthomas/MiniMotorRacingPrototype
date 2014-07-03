using UnityEngine;
using System.Collections;

public class MiniVechileTouch: MonoBehaviour {
	
	
	//Suspension/Wheel Setting
	private float wheelRadius = 0.4f;
	public float suspensionRange = 0.1f;
	public float suspensionDamper = 50.0f;
	public float suspensionSpringFront = 18500.0f;
	public float suspensionSpringRear = 9250.0f; 
	public float SlipFactor;
	public Transform[] frontWheels;
	public Transform[] rearWheels;
	public int maximumTurn = 30;
	public int minimumTurn = 12; 
	
	//Wheel Friction Curve
	private WheelFrictionCurve forwardWFC;
	private WheelFrictionCurve sidewayWFC;
	private WheelFrictionCurve brakeWFC;
	private Wheel[] wheels;
	private float WheelCount;
	private float sidewayStiffnessOverride = -1.0f;
	
	//Car Performence
	public AnimationCurve EngineTorqueCurve;
	public float transmissionEfficiency = 0.7f;
	public float[] gearRatios = new float[] {2.3f,1.78f,1.3f,1.0f,0.74f,0.5f};
	public float[] gearThrottleLimiting = new float[] {0.2f,0.2f,0.2f,1.0f,1.0f,1.0f};
	public float[] autoRPMChangeUp = new float[] {3000.0f,3000.0f,3000.0f,3000.0f,4500.0f};
	public float diffRatio = 3.42f;
	public float frontalArea = 2.0f;
	public float sideAreaMultiplier = 3.0f;
	public float coefDrag = 0.25f;
	public float coefRollingResistance = 0.01f;
	public float coefEngineBraking = 0.54f;
	public float brakeTorque = 1000.0f; 
	
	//Controller
	public float throttle = 0.0f;
	public float steer = 0.0f;
	private float gearChange = 0.0f;

	
	//Center of mass
	public Transform centerOfMass;
	
	//Speed,RPM and Current Gear
	public float topSpeed = 180.0f;
	private float rpm = 1000;
	private int currentGear = 0;
	
	//Cached Components
	private Rigidbody myRigidBody = null;
	private Transform myTransform = null;
	
	private bool canSteer;
	private bool canDrive;
	private bool ThrottleDisabled;
	private bool Slipping;
	public bool isBraked;
	
	private Vector3 _currentRotation;
	
	class Wheel
	{
		public WheelCollider collider;
		public Transform wheelGraphic;
		public Transform tireGraphic;
		public bool frontWheel = false;
		public bool rearWheel = false;
		public Vector3 wheelVelo = Vector3.zero;
		public Vector3 groundSpeed = Vector3.zero;
	}
	
	void Awake()
	{
		myRigidBody = rigidbody;
		myTransform = transform; 
		_currentRotation = new Vector3(transform.localEulerAngles.x,transform.localEulerAngles.y,transform.localEulerAngles.z);
	}

	// Use this for initialization
	void Start () {
		wheels = new Wheel[frontWheels.Length + rearWheels.Length];
		
		SetupWheelColliders();
		
		SetupCenterOfMass();
		
	}
	
	void SetupWheelColliders()
	{
		SetupWheelFrictionCurve();
		int wheelCount = 0;
		
		foreach(Transform t in frontWheels)
		{
			wheels[wheelCount] = SetupWheel(t,true);
			wheelCount++;
		}
		
		foreach(Transform t in rearWheels)
		{
			wheels[wheelCount] = SetupWheel(t,false);
			wheelCount++;
		}
	}
	
	void SetupWheelFrictionCurve()
	{
		sidewayWFC = new WheelFrictionCurve();
		sidewayWFC.extremumSlip = 1;
		sidewayWFC.extremumValue = 350;
		sidewayWFC.asymptoteSlip = 2;
		sidewayWFC.asymptoteValue = 150;
		sidewayWFC.stiffness = 1.0f;
		
		forwardWFC = new WheelFrictionCurve();
		forwardWFC.extremumSlip = 0.5f;
		forwardWFC.extremumValue = 6000;
		forwardWFC.asymptoteSlip = 2.0f;
		forwardWFC.asymptoteValue = 400;
		forwardWFC.stiffness = 1.0f;
		
		brakeWFC = new WheelFrictionCurve();
		brakeWFC.extremumSlip = 1;
		brakeWFC.extremumValue = 500;
		brakeWFC.asymptoteSlip = 2;
		brakeWFC.asymptoteValue = 250;
		brakeWFC.stiffness = 1.0f;
		
	}
	
	Wheel  SetupWheel(Transform wheelTransform,bool isFrontWheel)
	{
		GameObject go = new GameObject(wheelTransform.name + " Collider");
		
		go.transform.position = wheelTransform.position;
		go.transform.parent = transform;
		go.transform.rotation = wheelTransform.rotation;
		
		WheelCollider wc = go.AddComponent(typeof(WheelCollider)) as WheelCollider;
		
		wc.suspensionDistance = suspensionRange;
		JointSpring js = wc.suspensionSpring;
		
		if(isFrontWheel)
		{
			js.spring = suspensionSpringFront;
		}
		else
		{
			js.spring = suspensionSpringRear;
		}
		
		js.damper = suspensionDamper;
		
		wc.suspensionSpring =js;
		
		wc.mass = 3.2f; 
		
		wc.sidewaysFriction = sidewayWFC;
		wc.forwardFriction = forwardWFC;
		
		
		Wheel wheel = new Wheel();
		wheel.collider = wc;
		wheel.wheelGraphic = wheelTransform;  
  		wheel.tireGraphic = wheelTransform.GetComponentsInChildren<Transform>()[1];
		wheelRadius = wheel.tireGraphic.renderer.bounds.size.y / 2.0f;
		wheel.collider.radius = wheelRadius;
		
		if(isFrontWheel)
		{
			wheel.frontWheel = true;
			go = new GameObject(wheelTransform.name + "Steer Column");
			go.transform.position = wheelTransform.position;
			go.transform.rotation = wheelTransform.rotation;
			go.transform.parent = transform;
			wheelTransform.parent = go.transform;
		}
		else
		{
			wheel.rearWheel = true;	
		}
		
		
		 return wheel;
		
		
	}
	
	void SetupCenterOfMass()
	{
		if(centerOfMass!=null) 
			rigidbody.centerOfMass = centerOfMass.localPosition;
	}
	
	
	void FixedUpdate()
	{
		GetInput();
		
		Vector3 relativeVelocity = myTransform.InverseTransformDirection(myRigidBody.velocity);
		
		gearChange -=Time.deltaTime;
		
		UpdateFriction(relativeVelocity);
		
		ApplyResistanceForces(relativeVelocity);
		
		ApplyMotorTorque();
		
		ApplyBrakeTorque(relativeVelocity); 
		
		CalculateRPM(relativeVelocity);
		
		UpdateGear(relativeVelocity);
		
		CalculateState();
		
		ApplySteering(canSteer,relativeVelocity);
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 relativeVelocity = myTransform.InverseTransformDirection(myRigidBody.velocity);
		
		GetCurrentSpeed();
		
		UpdateWheelGraphics(relativeVelocity);
	
	}
	
		void LateUpdate()
	{
		if(steer == 0)
		{
			float y = _currentRotation.y;
			Debug.Log(_currentRotation.y);
			transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,y,transform.localEulerAngles.z);
		}
		else
		{
			_currentRotation.y = transform.localEulerAngles.y;
		}
		
	}
	
	void UpdateWheelGraphics(Vector3 relativeVelocity)
	{
		int wheelCount = -1;
		int groundHit = 0;
		
		
		foreach(Wheel w in wheels)
		{
			wheelCount++;
			
			WheelCollider wheel = w.collider;
			
			WheelHit wh= new WheelHit();
			
			if(wheel.GetGroundHit(out wh))
			{
				groundHit++;
				Vector3 tempPosition = w.wheelGraphic.localPosition;
				if(w.rearWheel)
				{	
					tempPosition.y = wheel.radius + wheel.transform.InverseTransformPoint(wh.point).y;
				}
				else
				{
					tempPosition.y = wheel.radius + wheel.transform.InverseTransformPoint(wh.point).y;
				}
				w.wheelGraphic.localPosition = tempPosition;
				w.wheelVelo = myRigidBody.GetPointVelocity(wh.point);
				w.groundSpeed = w.wheelGraphic.InverseTransformDirection(w.wheelVelo);
			}
			else
			{
				w.wheelGraphic.position = wheel.transform.position - (wheel.transform.up * suspensionRange );
			}
			
			if(w.frontWheel)
			{
				Vector3 ea = w.wheelGraphic.parent.localEulerAngles;
				ea.y = steer * maximumTurn;
				w.wheelGraphic.parent.localEulerAngles = ea;
			}
			
			w.tireGraphic.Rotate(Vector3.forward * w.collider.rpm * 2 * Mathf.PI/60.0f * Time.deltaTime * Mathf.Rad2Deg);
			
			
		}
	}
	
	void GetInput()
	{
		Debug.Log(throttle);
		throttle = -Input.GetAxis("Vertical"); 	
		steer =  Input.GetAxis("Horizontal");
		
	}
	
	float GetCurrentSpeed()
	{
		foreach(Wheel w in wheels)
		{
			if(w.rearWheel)
			{
				return Mathf.Round(2 * Mathf.PI * w.collider.radius * w.collider.rpm * 60/1000);
			}
		}
		
		return 0;
	}
	void UpdateFriction(Vector3 relativeVelocity)
	{
		float trackSlipModifier = 3.0f;
		if(GetCurrentSpeed() < 1.0)
		{
			sidewayWFC.stiffness = 20.0f;
		}
		else
		{
			sidewayWFC.stiffness = SlipFactor * trackSlipModifier; 
		}
		
		brakeWFC.stiffness = SlipFactor * trackSlipModifier;
		
		float gearFactor = Mathf.Clamp01((currentGear * 3.0f)/(float)(gearRatios.Length));
		forwardWFC.stiffness = Mathf.Clamp(Mathf.Lerp(0,1,gearFactor),0.7f,1.0f); 
		
		foreach(Wheel w in wheels)
		{
			w.collider.sidewaysFriction = sidewayWFC;
			
			if(Mathf.Sign(throttle) == Mathf.Sign(relativeVelocity.z))
			{
				w.collider.forwardFriction = forwardWFC;
			}
			else
			{
				w.collider.forwardFriction = brakeWFC;
			}
		}
	}
	
	void ApplyResistanceForces(Vector3 relativeVelocity)
	{
		Vector3 relDrag;
		
		
		relDrag = -relativeVelocity;
		float airDensity = 1.2041f;
		
		float CDrag = 0.5f * coefDrag * frontalArea *airDensity;
		
		relDrag.x *= CDrag*sideAreaMultiplier;
		relDrag.z *= CDrag;
		relDrag.y *= CDrag*sideAreaMultiplier;
		
		float CrG = coefRollingResistance * 9.0f;
		
		Vector3 rollingResistanceForce = -Mathf.Sign(relativeVelocity.z) * myTransform.forward * (CrG * myRigidBody.mass);
		
		if(Mathf.Abs(relativeVelocity.z) < CrG) 
			rollingResistanceForce *= 0;
		
		myRigidBody.AddForce(myTransform.TransformDirection(relDrag),ForceMode.Impulse);
		
		if(canDrive)
		{
			myRigidBody.AddForce(rollingResistanceForce,ForceMode.Impulse);
		}
		
	}
	
	void ApplyMotorTorque()
	{
		float motorTorque = 0;
		
		float tRpm = Mathf.Abs(rpm);
		
		if(tRpm < 1000 || gearChange > 0)
			tRpm = 1000;
		
		float MaxEngineTorque = EngineTorqueCurve.Evaluate(tRpm);
		
		float skidControl = gearThrottleLimiting[currentGear];
		
		float EngineTorque = MaxEngineTorque * throttle * skidControl;
		
		motorTorque = EngineTorque * gearRatios[currentGear] * diffRatio * transmissionEfficiency;
		
		foreach(Wheel w in wheels)
		{
			if(w.rearWheel)
			{
				w.collider.motorTorque = motorTorque;
			}
		}
	}
	
	void ApplyBrakeTorque(Vector3 relativeVelocity)
	{
		if(Mathf.Abs(relativeVelocity.z)<0.01)
			return;
		
		foreach(Wheel w in wheels)
		{
			if(w.rearWheel)
			{
				if(Mathf.Sign(throttle) == Mathf.Sign(relativeVelocity.z))
					w.collider.brakeTorque = 0.0f;
				else if(isBraked)
				{
					w.collider.brakeTorque = (brakeTorque*Mathf.Abs(throttle)) * (coefEngineBraking * (rpm/60.0f));
				}
			}
		}
	}
	
	void CalculateRPM(Vector3 relativeVelocity)
	{
		float wheelRPM = 0;
		float wheelRadius = 0;
		
		foreach(Wheel w in wheels)
		{
			if(w.rearWheel)
			{
				wheelRPM = w.collider.rpm;
				rpm = wheelRPM * gearRatios[currentGear] * diffRatio;
				wheelRadius = w.collider.radius;
				break;
				
			}
		}
		
		if(rpm < 1000.0f)
			rpm = 1000.0f;
		
		float wheelSpeed = (wheelRPM * 2 * Mathf.PI * wheelRadius)/60.0f;
		
		if(Mathf.Abs(wheelSpeed-relativeVelocity.z) > 1.5f)
			Slipping = true;
		else
			Slipping = false;
	}
	
	void UpdateGear(Vector3 relativeVelocity)
	{
		if(GetCurrentSpeed()<5)
		{
			currentGear = 0;
			return;
		}
		
		if(gearChange > 0 )
			return;
		
		if(currentGear<autoRPMChangeUp.Length && rpm>autoRPMChangeUp[currentGear] && throttle >0)
		{
			currentGear++;
			gearChange = 0.05f;
		}
		else
		{
			if(currentGear > 0 && (rpm < 2500 && throttle<=0 || rpm<1000))
			{
				currentGear--;
				
				gearChange = 0.05f; 
			}
		}
	}
	
	void CalculateState()
	{
		canDrive = false;
		canSteer = false;
		
		foreach(Wheel w in wheels)
		{
			if(w.collider.isGrounded)
			{
				if(w.rearWheel)
					canDrive = true;
				if(w.frontWheel)
					canSteer = true;
			}
		}
	}
	
	void ApplySteering(bool canSteer,Vector3 relativeVelocity)
	{
		myRigidBody.angularDrag = Mathf.Abs(relativeVelocity.z)/5.0f;
		
		
		if(throttle < 0 )
			myRigidBody.angularDrag = 0;
		
		if(canSteer)
		{
			float minMaxTurn = EvaluateSpeedToTurn(myRigidBody.velocity.magnitude);
			
			if(Mathf.Abs(steer)<0.01)
				steer = 0.0f;
			
			foreach(Wheel w in wheels)
			{
				if (w.frontWheel)
					w.collider.steerAngle = minMaxTurn * steer;
				else
					w.collider.steerAngle = 0;
			}
		}
			
	}
	
	float EvaluateSpeedToTurn(float speed)
	{
		return Mathf.Lerp(maximumTurn,minimumTurn,speed/2);
	}
}
