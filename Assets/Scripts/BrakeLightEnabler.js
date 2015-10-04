#pragma strict
// this is a simple script that simply enables disables the brake light
// textures if the brake button is pressed
 
var LeftBrakeLight_1 : GameObject;
var LeftBrakeLight_2 : GameObject;
var RightBrakeLight_1 : GameObject;
var RightBrakeLight_2 : GameObject;
var brakeButton : GUITexture;

function Start () 
{
	LeftBrakeLight_1.renderer.enabled = false;
	LeftBrakeLight_2.renderer.enabled = false;
	RightBrakeLight_1.renderer.enabled = false;
	RightBrakeLight_2.renderer.enabled = false;
}

function Update () 
{
	 for(var touch : Touch in Input.touches)
	 {
		if ( brakeButton.HitTest (touch.position) && touch.phase == TouchPhase.Stationary)
		{
			LeftBrakeLight_1.renderer.enabled = true;
			LeftBrakeLight_2.renderer.enabled = true;
			RightBrakeLight_1.renderer.enabled = true;
			RightBrakeLight_2.renderer.enabled = true;
		}
		
		else if (touch.phase == TouchPhase.Ended )
		{
			LeftBrakeLight_1.renderer.enabled = false;
			LeftBrakeLight_2.renderer.enabled = false;
			RightBrakeLight_1.renderer.enabled = false;
			RightBrakeLight_2.renderer.enabled = false;
		}
  	}
}