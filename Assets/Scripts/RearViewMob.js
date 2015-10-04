#pragma strict

public var mirror : GUITexture;

function Update () {
 	
	for (var touch: Touch in Input.touches)
	{

		if (mirror.HitTest (touch.position) && touch.phase == TouchPhase.Stationary){
			camSwap(mirror);
		}
		else if (touch.phase == TouchPhase.Ended){
			//Debug.Log("Lifted off the gas");
			//motorInputTouch = 0;
		}
	}
}
 
function camSwap(currentCam : GUITexture)
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