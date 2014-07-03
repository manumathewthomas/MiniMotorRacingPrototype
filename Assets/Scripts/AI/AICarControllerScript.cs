using UnityEngine;
using System.Collections;

public class AICarControllerScript : MonoBehaviour {
	
	
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
	private WheelFrictionCurve slipWFC;
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
	public float driftFactor = 250f;
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
	public bool isTimer;
	public bool isAvoidForward;
	public bool isAvoidRight;
	public bool isAvoidLeft;
	private float wheelSpeed;
	private Vector3 _currentRotation;
	
	
	//AI Variables
	
	public float AISpeed;
	public float AIMass;
	public float AIRadius;
	
	private float AICurSpeed;
	private float AICurPathIndex;
	private Vector3 AITargetPoint;
	
	public GameObject waypointContainer;
	private Vector3[] waypoints;
	private Transform[] potentialWaypoints;
	private int pathindex;
	public int curWaypoint = 0;
	public int waypointLength;
	private Vector3 AIDirection;
	public float AIThrottleSpeed;
	public int rank;
	
	private float respawnTimer;
	
	
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
		AIMass = myRigidBody.mass;
		AIRadius = 2.0f;
		AIThrottleSpeed = -1.0f;
		AIDirection = transform.forward;
		isAvoidForward = false;
		isAvoidRight = false;
		isAvoidLeft = false;
		rank = 0;
		
		GetWaypoints();
		
		SetupWheelColliders();
		
		SetupCenterOfMass();
		
	}
	
	
	void GetWaypoints()
	{
		waypointLength = waypointContainer.GetComponentsInChildren<Transform>().Length - 1;
		potentialWaypoints  = waypointContainer.GetComponentsInChildren<Transform>() as Transform[];
		
		waypoints = new Vector3[waypointLength];
		foreach(Transform potentialWaypoint in potentialWaypoints)
		{
			if(potentialWaypoint != waypointContainer.transform)
			{
				waypoints[pathindex] = potentialWaypoint.position;
				pathindex++;
			}
			
		}
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
	
	void SetDriftValues()
	{
		slipWFC = new WheelFrictionCurve();
		slipWFC.extremumSlip = 0.5f;
		slipWFC.extremumValue = driftFactor;
		slipWFC.asymptoteSlip = 2.0f;
		slipWFC.asymptoteValue = driftFactor/2;
		slipWFC.stiffness = 1.0f;
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
		if(isTimer)
		{
			
			GetInput();
		
			Vector3 relativeVelocity = myTransform.InverseTransformDirection(myRigidBody.velocity);
		
			gearChange -=Time.deltaTime;
		
			UpdateFriction(relativeVelocity);
			
			ApplyResistanceForces(relativeVelocity);
		
			ApplyMotorTorque();
		
			//ApplyBrakeTorque(relativeVelocity); 
		
			SetDriftValues();
		
			CalculateRPM(relativeVelocity);
		
			UpdateGear(relativeVelocity);
		
			CalculateState();
		
			ApplySteering(canSteer,relativeVelocity);
		
			AIController();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 relativeVelocity = myTransform.InverseTransformDirection(myRigidBody.velocity);
		AISpeed = -relativeVelocity.magnitude;
		
		SetupCenterOfMass();
		
		UpdateWheelGraphics(relativeVelocity);
		
		
	
	}
	
		void LateUpdate()
	{
		if(steer == 0)
		{
			float y = _currentRotation.y;
			 
			transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,y,transform.localEulerAngles.z);
		}
		else
		{
			_currentRotation.y = transform.localEulerAngles.y;
		}
		
	}
	
	
	void GetInput()
	{
		throttle = Mathf.Lerp(-1,1,AIThrottleSpeed);
		
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
					tempPosition.y = wheel.radius+0.15f + wheel.transform.InverseTransformPoint(wh.point).y;
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
	

	
	float GetCurrentSpeed()
	{
		
		foreach(Wheel w in wheels)
		{
			if(w.rearWheel)
			{
				return Mathf.Round(2 * Mathf.PI * w.collider.radius * w.collider.rpm / 60);
			}
		}
		
		return 0;
	}
	void UpdateFriction(Vector3 relativeVelocity)
	{
		float trackSlipModifier = 2.0f;
		if(GetCurrentSpeed() < 1.0)
		{
			sidewayWFC.stiffness = 10.0f;
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
			
			if(Slipping)
			{
				//w.collider.sidewaysFriction = slipWFC;
				
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
		
		float CrG = coefRollingResistance * 2.0f;
		
		Vector3 rollingResistanceForce = -Mathf.Sign(relativeVelocity.z) * myTransform.forward * (CrG * myRigidBody.mass);
		
		if(Mathf.Abs(relativeVelocity.z) < CrG) 
		{
			rollingResistanceForce *= 0;
		}
		
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
		
		
		if(GetCurrentSpeed() <= topSpeed)
		
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
		
		
		foreach(Wheel w in wheels)
		{
			if(w.rearWheel)
			{
				if(Mathf.Sign(throttle) == Mathf.Sign(relativeVelocity.z))
					w.collider.brakeTorque = 0.0f;
				else
				{
					//w.collider.brakeTorque = (brakeTorque*Mathf.Abs(throttle)) * (coefEngineBraking * (rpm/60.0f));
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
		
		wheelSpeed = (wheelRPM * 2 * Mathf.PI * wheelRadius)/60.0f;
		
		if(Mathf.Abs(wheelSpeed-relativeVelocity.z) > 1.0f)
			Slipping = true;
		else
			Slipping = false;
		
		
	}
	
	void UpdateGear(Vector3 relativeVelocity)
	{
		
		if( wheelSpeed < 5)
		{
			currentGear = 0;
			return;
		}
		
		if(gearChange > 0 )
			return;
		
		if(currentGear<autoRPMChangeUp.Length && rpm>autoRPMChangeUp[currentGear] && throttle > 0)
		{
			currentGear++;
			gearChange = 0.05f;
		}
		else
		{
			if(currentGear > 0 && ((rpm < 2500 && throttle <=0) || rpm < 1000))
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
	
	
	void AIController()
	{
		AICurSpeed  = AISpeed * Time.deltaTime;
		AITargetPoint = GetPoint(curWaypoint);
	
		Debug.DrawRay(AITargetPoint,transform.position-AITargetPoint);
		
		if(Vector3.Distance(transform.position,AITargetPoint) < AIRadius)
		{
			if(curWaypoint < waypointLength-1)
			{
				curWaypoint++;
				rank++;
				respawnTimer = Time.time;
			}
			else
			{
				return;
			}
			
		}
		
		if(curWaypoint >= waypointLength)
			return;
		
		if(curWaypoint >= waypointLength - 1)
		{
			
			curWaypoint =0;
			//AIDirection += Steer(AITargetPoint,true);
		}
		else
		{
			//AIDirection += Steer(AITargetPoint);
		}
		
		if(isAvoidForward)
		{
			steer = 1.0f;
		}
		else if(isAvoidRight)
		{
			steer = -1.0f;
		}
		else if(isAvoidLeft)
		{
			steer = 1.0f;
			
		}
		else
		{
			steer = Steer(AITargetPoint);
			
		}
		
		
	}
	
	public float Steer(Vector3 targetPoint,bool bFinalPoint = false)
	{

		Vector3 desiredDirection = targetPoint - transform.position;
		desiredDirection.y =0.0f;
		float distance = desiredDirection.magnitude;
		
	
		desiredDirection.Normalize();
		
		Vector3 cp = Vector3.Cross(transform.forward,desiredDirection);
		
		float steerTemp = cp.magnitude;
		
		if(cp.y > 0.0f)
			steerTemp = -steerTemp;
		
	
	
		return steerTemp;
		
		
	}
	
	Vector3 GetPoint(int curPathIndex)
	{
		
		return waypoints[curPathIndex];
	}
	
	
	void OnCollisionStay(Collision col)
	{
		
		Vector3 _direction;
		Quaternion _lookRotation;
		if((col.transform.tag == "Obstacle" || col.transform.tag == "Car") && transform.GetComponent<AICarControllerScript>().enabled)
		{
			float tempRot =0.05f;
			tempRot = Mathf.Sign(steer) * tempRot;
			Debug.Log(Time.time - respawnTimer);
			transform.RotateAroundLocal(Vector3.up,tempRot);
			
			if(Time.time - respawnTimer >= 3.0f)
			{
				transform.position = AITargetPoint;
				_direction = (AITargetPoint - transform.position).normalized;
				_lookRotation = Quaternion.LookRotation(_direction);
				transform.rotation = Quaternion.Slerp(transform.rotation,_lookRotation,Time.deltaTime*5);
			}
		}
	}
	
	void OnCollisionExit(Collision col)
	{
		if((col.transform.tag == "Obstacle" || col.transform.tag == "Car") && transform.GetComponent<AICarControllerScript>().enabled)
		{
			float tempRot =0.05f;
			tempRot = Mathf.Sign(steer) * tempRot;
			
			transform.RotateAroundLocal(Vector3.up,tempRot);
		}
	}
	
}
