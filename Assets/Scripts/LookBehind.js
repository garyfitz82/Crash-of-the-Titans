#pragma strict

function Update () {
 	
	if(Input.GetKeyDown("4"))
	{
  		Debug.Log("Using Camera Two");
  		camSwap(2);
 	}
 	else if(Input.GetKeyUp("4"))
 	{
 		camSwap(1);
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
  			cams.GetComponent(AudioListener).enabled = false;
 		}  
 
 	var oneToUse : String = "Camera"+currentCam;
 	gameObject.Find(oneToUse).GetComponent(Camera).enabled = true;
 	gameObject.Find(oneToUse).GetComponent(AudioListener).enabled = true;
}