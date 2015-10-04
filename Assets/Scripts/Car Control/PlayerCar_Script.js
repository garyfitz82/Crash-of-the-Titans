
// These variables allow the script to power the wheels of the car.
var FrontLeftWheel : WheelCollider;
var FrontRightWheel : WheelCollider;
var RearLeftWheel : WheelCollider;
var RearRightWheel : WheelCollider;
var fl : Transform;
var fr : Transform;
// These variables are for the gears, the array is the list of ratios. The script
// uses the defined gear ratios to determine how much torque to apply to the wheels.
var GearRatio : float[];
var CurrentGear : int = 0;
var mphDisplay : GUIText;
// These variables are just for applying torque to the wheels and shifting gears.
// using the defined Max and Min Engine RPM, the script can determine what gear the
// car needs to be in.
var currentSpeed :float;
var topSpeed :float = 150.0;
var highSpeedSteer = 20.0;
var lowSpeedSteer = 45.0;
var highSpeedLevel = 50;
var EngineTorque : float = 600.0;
var MaxEngineRPM : float = 3000.0;
var MinEngineRPM : float = 1000.0;
private var EngineRPM : float = 0.0;
var braked : boolean = false;
var maxBrakeTorque : float = 100;
var speedometerDial : Texture2D;
var speedometerNeedle : Texture2D;
var idleSound : AudioClip;
var accelerateSound : AudioClip;
//var CentreOfMass : Transform;
function Start () {
	// I usually alter the center of mass to make the car more stable. I'ts less likely to flip this way.
	//rigidbody.centerOfMass = CentreOfMass.transform.position;
	rigidbody.centerOfMass = new Vector3(0, 0, .6);
	
}

function Update () {
	
	// This is to limith the maximum speed of the car, adjusting the drag probably isn't the best way of doing it,
	// but it's easy, and it doesn't interfere with the physics processing.
	rigidbody.drag = rigidbody.velocity.magnitude / 250;
	var mph = rigidbody.velocity.magnitude * 2.237;
	mphDisplay.text = "MPH: " + mph.ToString("F0");
	// Compute the engine RPM based on the average RPM of the two wheels, then call the shift gear function
	EngineRPM = (FrontLeftWheel.rpm + FrontRightWheel.rpm)/2 * GearRatio[CurrentGear];
	
	ShiftGears();
	
	// set the audio pitch to the percentage of RPM to the maximum RPM plus one, this makes the sound play
	// up to twice it's pitch, where it will suddenly drop when it switches gears.
		audio.pitch = Mathf.Abs(EngineRPM / MaxEngineRPM)+ 0.5;
	
		// this line is just to ensure that the pitch does not reach a value higher than is desired.
		if ( audio.pitch > 2.0 ) 
		{
			audio.pitch = 2.0;
		}
	// finally, apply the values to the wheels.	The torque applied is divided by the current gear, and
	// multiplied by the user input variable.
	FrontLeftWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * Input.GetAxis("Vertical");
	FrontRightWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * Input.GetAxis("Vertical");
	
	// deacceleration
	if(Input.GetAxis("Vertical") == 0)
		{
			//apply a gradual slowdown of the vehicle when the accelerator is not pressed 
			rigidbody.velocity = rigidbody.velocity*.995f;
		}
			
	// the steer angle is a value multiplied by the user input.
	var fwdSpeed = rigidbody.velocity.magnitude;
    var highSpeedFraction = fwdSpeed / highSpeedLevel;
    var actualSteerAngle = Mathf.Lerp(lowSpeedSteer, highSpeedSteer, highSpeedFraction);
	actualSteerAngle *= Input.GetAxis("Horizontal");
	FrontLeftWheel.steerAngle = actualSteerAngle;
	FrontRightWheel.steerAngle = actualSteerAngle;
	fl.localEulerAngles.y = FrontLeftWheel.steerAngle - fl.localEulerAngles.z;
	fr.localEulerAngles.y = FrontRightWheel.steerAngle - fr.localEulerAngles.z;
	//Debug.Log(actualSteerAngle);
	HandBrake();
}

function ShiftGears() {
	// this funciton shifts the gears of the vehcile, it loops through all the gears, checking which will make
	// the engine RPM fall within the desired range. The gear is then set to this "appropriate" value.
	if ( EngineRPM >= MaxEngineRPM) 
	{
		var AppropriateGear : int = CurrentGear;
		
		for ( var i = 0; i < GearRatio.length; i ++ ) 
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
		AppropriateGear = CurrentGear;
		
		for ( var j = GearRatio.length-1; j >= 0; j -- ) 
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

function HandBrake()
	{
		// apply the handbrake
		if (Input.GetKey(KeyCode.Space))
        {
            RearRightWheel.brakeTorque = 200.0f;
            RearLeftWheel.brakeTorque = 200.0f;
            RearRightWheel.sidewaysFriction.extremumSlip = 100.0f;
            RearLeftWheel.sidewaysFriction.extremumSlip = 100.0f;
            RearRightWheel.sidewaysFriction.asymptoteSlip = 100.0f;
            RearLeftWheel.sidewaysFriction.asymptoteSlip = 100.0f;
        }
        else //Remove handbrake
        {
            RearRightWheel.brakeTorque = 0;
            RearLeftWheel.brakeTorque = 0;
            RearRightWheel.sidewaysFriction.extremumSlip = 1.0f;
            RearLeftWheel.sidewaysFriction.extremumSlip = 1.0f;
            RearRightWheel.sidewaysFriction.asymptoteSlip = 2.0f;
            RearLeftWheel.sidewaysFriction.asymptoteSlip = 2.0f;
        }
	}
	
// make sure this gameobject has an audiosource attached for it to play sounds
@script RequireComponent(AudioSource)
/*
function OnGUI ()
{
	GUI.DrawTexture(Rect(0, 0, 150, 75),speedometerDial);
	//var speedFactor : float = 
}
*/