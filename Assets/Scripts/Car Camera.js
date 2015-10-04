#pragma strict
/*
function Update () {
 	if(Input.GetKey("1"))
 	{
 		Debug.Log("Using Camera One");
  		camSwap(1);
 	}
	if(Input.GetKey("2"))
	{
  		Debug.Log("Using Camera Two");
  		camSwap(2);
 	}
 	//if(Input.GetKey("3")){
  		//Debug.Log("Using Camera Three");
  		//camSwap(3);
 	//}
}
 
function camSwap(currentCam : int)
{
 	var cameras = GameObject.FindGameObjectsWithTag("cam");
 
 		for (var cams : GameObject in cameras)
 		{
  			cams.GetComponent(Camera).enabled = false;
 		}  
 
 	var oneToUse : String = "Camera"+currentCam;
 	gameObject.Find(oneToUse).GetComponent(Camera).enabled = true;
}
*/

#pragma strict

var car : Transform;

var distance : float = 6.4;

var height : float = 1.4;

var rotationDamping : float = 3.0;

var heightDamping : float = 2.0;

var zoomRacio : float = 0.5;

var DefaultFOV : float = 60;

private var rotationVector : Vector3;

function Start () {

 

}

 

function LateUpdate () 
{

		var wantedAngle  =  rotationVector.y;
		
		var wantedHeight = car.position.y + height;
		
		var myAngle = transform.eulerAngles.y;
		
		var myHeight = transform.position.y;
		
		myAngle = Mathf.LerpAngle(myAngle,wantedAngle,rotationDamping*Time.deltaTime);
		
		myHeight = Mathf.Lerp(myHeight,wantedHeight,heightDamping*Time.deltaTime);
		
		var currentRotation = Quaternion.Euler(0,myAngle,0);
		
		transform.position = car.position;
		
		transform.position -= currentRotation*Vector3.forward*distance;
		
		transform.position.y = myHeight;
		
		transform.LookAt(car);

}

function FixedUpdate ()
{

		var localVilocity = car.InverseTransformDirection(car.rigidbody.velocity);

			if (localVilocity.z<-0.5)
			{

				rotationVector.y = car.eulerAngles.y + 180;

			}

			else 
			{

				rotationVector.y = car.eulerAngles.y;

			}

		var acc = car.rigidbody.velocity.magnitude;

		camera.fieldOfView = DefaultFOV + acc*zoomRacio;

}