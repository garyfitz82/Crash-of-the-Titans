using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioClip))]
public class Car : MonoBehaviour 
{
	// These variables allow the script to power the wheels of the car.
	//Transform player; 
	public WheelCollider FrontLeftWheel;
	public WheelCollider FrontRightWheel;
	public WheelCollider RearLeftWheel;
	public WheelCollider RearRightWheel;
	//public bool braked = true;
	public GUIText mphDisplay;
	public float steeringSharpness = 12.0f;
	public Transform fl;
	public Transform fr;
	// These variables are for the gears, the array is the list of ratios. The script
	// uses the defined gear ratios to determine how much torque to apply to the wheels.
	public float[] GearRatio;
	public int CurrentGear = 0;
	
	// These variables are just for applying torque to the wheels and shifting gears.
	// using the defined Max and Min Engine RPM, the script can determine what gear the
	// car needs to be in.
	public float EngineTorque = 600.0f;
	public float BrakePower = 0.0f;
	public float MaxEngineRPM = 3000.0f;
	public float MinEngineRPM = 1000.0f;
	protected float EngineRPM = 0.0f;

	// Use this for initialization
	void Start () 
	{
		//alter the center of mass to make the car more stable. It's less likely to flip this way.
		rigidbody.centerOfMass = new Vector3(0.0f,0.0f,0.6f);	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void FixedUpdate()
	{
		float mph= rigidbody.velocity.magnitude * 2.237f;
		mphDisplay.text = mph.ToString("F0") + " : MPH"; // displays one digit after the dot
	
		// This is to limit the maximum speed of the car, adjusting the drag probably isn't the best way of doing it,
		// but it's easy, and it doesn't interfere with the physics processing.
		rigidbody.drag = rigidbody.velocity.magnitude / 250;

		ShiftGears();
		audio.pitch = Mathf.Abs(EngineRPM / MaxEngineRPM) + 0.5f ;
		// this line is just to ensure that the pitch does not reach a value higher than is desired.
		if (audio.pitch > 2.0f ) 
		{
			audio.pitch = 2.0f;
			//audio.volume = .5f;
		}	
	}

	public void ShiftGears ()
	{
		// this funciton shifts the gears of the vehcile, it loops through all the gears, checking which will make
		// the engine RPM fall within the desired range. The gear is then set to this "appropriate" value.
		if ( EngineRPM >= MaxEngineRPM ) 
		{
			int AppropriateGear = CurrentGear;
			
			for ( int i = 0; i < GearRatio.Length; i++ ) 
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
			
			for ( int j= GearRatio.Length-1; j >= 0; j -- ) 
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