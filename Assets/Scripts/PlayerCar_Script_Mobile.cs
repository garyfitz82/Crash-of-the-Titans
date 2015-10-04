using UnityEngine;
using System.Collections;

public class PlayerCar_Script_Mobile : MonoBehaviour 
{

// These variables allow the script to power the wheels of the car Joystick joystickPos;
public Vector2 joystick;
public Joystick joystickPos;
public Transform fl;
public Transform fr;
public WheelCollider FrontLeftWheel;
public WheelCollider FrontRightWheel;
public WheelCollider RearLeftWheel;
public WheelCollider RearRightWheel; 
//GUITexture reverseButton;
public GUITexture gasButton;
public GUITexture brakeButton;
//GUITexture leftButton;
//GUITexture rightButton;
public GUITexture handBrake;

public GUIText mphDisplay;
//float turnSpeed = 1.0f;
public int motorInputTouch = 0;
public float brakePower = 300; //200
//float centreOfMass = 0;
// These variables are for the gears, the array is the list of ratios. The script
// uses the defined gear ratios to determine how much torque to apply to the wheels.
public float[] GearRatio;
public int CurrentGear = 0;
public float highSpeedSteer = 20.0f;
public float lowSpeedSteer = 45.0f;
public float highSpeedLevel = 50;
public bool  reverse = false;
// These variables are just for applying torque to the wheels and shifting gears.
// using the defined Max and Min Engine RPM, the script can determine what gear the
// car needs to be in.
public float EngineTorque = 200.0f;
public float MaxEngineRPM = 3000.0f;
public float MinEngineRPM = 1000.0f;
public float EngineRPM = 0.0f;
//private bool  reverse = false;
//private int back = 0;

void Awake (){
	//reverseButton = GameObject.Find("reverse").guiTexture;
	gasButton = GameObject.Find("gas").guiTexture;
	brakeButton = GameObject.Find("brake").guiTexture;
	//leftButton = GameObject.Find("left").guiTexture;
	//rightButton = GameObject.Find("right").guiTexture;
	}

void Start (){
	// Set the rigidbody's centre of mass
	//rigidbody.centerOfMass.y = -1.6f;
	//rigidbody.centerOfMass.z = 0.5f;
    rigidbody.centerOfMass = new Vector3(0.0f, 0.0f, 0.6f);
	gameObject.audio.Play();
	
}

void Update (){
	rigidbody.drag = rigidbody.velocity.magnitude / 250;
	float mph = rigidbody.velocity.magnitude * 2.237f;
	mphDisplay.text = "MPH: " + mph.ToString("F0");
	
	float fwdSpeed= rigidbody.velocity.magnitude;
    float highSpeedFraction= fwdSpeed / highSpeedLevel;
    float actualSteerAngle= Mathf.Lerp(lowSpeedSteer, highSpeedSteer, highSpeedFraction);
    
	fl.localEulerAngles = new Vector3(0,FrontLeftWheel.steerAngle - fl.localEulerAngles.z, 0);
	fr.localEulerAngles = new Vector3(0,FrontRightWheel.steerAngle - fr.localEulerAngles.z, 0);
/////////////////////////////************************Joystick Code**************************////////////////////////////////////////	
	 //Debug.Log(joystickPos.position.x);
	 
		 	if(joystickPos.position.x < 0)
		 	{
		 		FrontLeftWheel.steerAngle = -actualSteerAngle;
				FrontRightWheel.steerAngle = -actualSteerAngle;
			}	
	
		 	else if(joystickPos.position.x > 0)
		 	{
		 		FrontLeftWheel.steerAngle = actualSteerAngle;
				FrontRightWheel.steerAngle = actualSteerAngle;
		 	}

	 		else
	 		{
	 			FrontLeftWheel.steerAngle = 0;
				FrontRightWheel.steerAngle = 0;
	 		}
	
	
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	// limit the maimum speed of the car by imposing drag
	rigidbody.drag = rigidbody.velocity.magnitude / 250;
	

	foreach(Touch touch in Input.touches)
	{

		if (gasButton.HitTest (touch.position) && touch.phase == TouchPhase.Stationary){
			motorInputTouch = 1;
			//Debug.Log ("Hitting the Gas Button");
		}
		else if (touch.phase == TouchPhase.Ended)
		{
			//Debug.Log("Lifted off the gas");
			motorInputTouch = 0;
			if(motorInputTouch == 0)
				{
					rigidbody.velocity = rigidbody.velocity * 0.995f;	
				}
		}
		
		if ( brakeButton.HitTest (touch.position) && touch.phase == TouchPhase.Stationary)
		{
			brakePower = 200;
			//if(this.rigidbody.velocity.z >= 0.0f && !reverse)
			if(touch.tapCount==1)
			{
				brakePower = 0;
				motorInputTouch = -1;
			}
		}

		
		else if (touch.phase == TouchPhase.Moved)
		{
			brakePower = 0;
			
		}
		
		if (handBrake.HitTest (touch.position) && touch.phase == TouchPhase.Stationary)
		{
			RearRightWheel.brakeTorque = 200.0f;
            RearLeftWheel.brakeTorque = 200.0f;
			WheelFrictionCurve temp1 = new WheelFrictionCurve();
			temp1 = RearRightWheel.sidewaysFriction;
			temp1.extremumSlip = 100.0f;
			RearRightWheel.sidewaysFriction = temp1;
			
			WheelFrictionCurve temp2 = new WheelFrictionCurve();
			temp2 = RearLeftWheel.sidewaysFriction;
            temp2.extremumSlip = 100.0f;
			RearLeftWheel.sidewaysFriction = temp2;
			//Debug.Log ("Handbrake on");
		
		}
		else if (touch.phase == TouchPhase.Ended)
		{
			RearRightWheel.brakeTorque = 0;
            RearLeftWheel.brakeTorque = 0;
				
			WheelFrictionCurve temp1 = new WheelFrictionCurve();
            temp1 = RearRightWheel.sidewaysFriction;
			temp1.extremumSlip = 1.0f;
			RearRightWheel.sidewaysFriction = temp1;
			
			WheelFrictionCurve temp2 = new WheelFrictionCurve();
            temp2 = RearLeftWheel.sidewaysFriction;
			temp2.extremumSlip = 1.0f;
			RearLeftWheel.sidewaysFriction = temp2;
			//Debug.Log("Handbrake off");
			
		}
			
			
		/*
		if (touch.phase == TouchPhase.Stationary && leftButton.HitTest (touch.position)){ // 
			FrontLeftWheel.steerAngle = -actualSteerAngle;
			FrontRightWheel.steerAngle = -actualSteerAngle;
		}
		else if (touch.phase == TouchPhase.Ended){
			FrontLeftWheel.steerAngle = 0;
			FrontRightWheel.steerAngle = 0;
		}
		
		if (touch.phase == TouchPhase.Stationary && rightButton.HitTest (touch.position)){
			FrontLeftWheel.steerAngle = actualSteerAngle;
			FrontRightWheel.steerAngle = actualSteerAngle;
		}
		else if (touch.phase == TouchPhase.Ended){
			FrontLeftWheel.steerAngle = 0;
			FrontRightWheel.steerAngle = 0;
		}
		*/
		
		//if (touch.phase == TouchPhase.Began && reverseButton.HitTest (touch.position)){
			//Application.Quit();
		//}
		
}
	
	// Compute the engine RPM based on the average RPM of the two wheels, then call the shift gear function
	EngineRPM = (FrontLeftWheel.rpm + FrontRightWheel.rpm)/2 * GearRatio[CurrentGear];
	ShiftGears();

	// set the audio pitch to the percentage of RPM to the maximum RPM plus one, this makes the sound play
	// up to twice it's pitch, where it will suddenly drop when it switches gears.
	audio.pitch = Mathf.Abs(EngineRPM / MaxEngineRPM) + 0.5f ;
	// this line is just to ensure that the pitch does not reach a value higher than is desired.
	if ( audio.pitch > 2.0f ) {
		audio.pitch = 2.0f;
	}

	// finally, apply the values to the wheels.	The torque applied is divided by the current gear, and
	// multiplied by the user input variable.
	FrontLeftWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * motorInputTouch;
	FrontRightWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * motorInputTouch;
	RearRightWheel.brakeTorque = brakePower;
	RearLeftWheel.brakeTorque = brakePower;		
	// the steer angle is an arbitrary value multiplied by the user input.
	//FrontLeftWheel.steerAngle = 30 * Input.GetAxis("Horizontal");
	
	//FrontRightWheel.steerAngle = 30 * Input.GetAxis("Horizontal");
}

void ShiftGears()
	{
		if ( EngineRPM >= MaxEngineRPM ) 
		{
			int AppropriateGear = CurrentGear;
			
			for ( int i = 0; i < GearRatio.Length; i ++ ) 
			{
				if ( FrontLeftWheel.rpm * GearRatio[i] < MaxEngineRPM ) 
				{
					AppropriateGear = i;
					break;
				}
			}
		
			CurrentGear = AppropriateGear;
		}
	
	
		if ( EngineRPM <= MinEngineRPM ) 
		{
			int AppropriateGear = CurrentGear;
			
			for ( int j = GearRatio.Length-1; j >= 0; j-- ) 
			{
				if ( FrontLeftWheel.rpm * GearRatio[j] > MinEngineRPM ) 
				{
					AppropriateGear = j;
					break;
				}
			}
			
			CurrentGear = AppropriateGear;
		}	
	}
}


	