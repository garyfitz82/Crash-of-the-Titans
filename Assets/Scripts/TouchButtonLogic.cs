using UnityEngine;
using System.Collections;

public class TouchButtonLogic : MonoBehaviour 
{

	void Update () 
	{
		// is theere a touch on screen
		if(Input.touches.Length <= 0)
		{
			// if no touches execute this 
		}
		else // if there is a touch
		{
			// loop through the touches
			for(int i = 0; i < Input.touchCount; i++)
			{
				// execute this code for the current touch	
				if(this.guiText.HitTest(Input.GetTouch(i).position))
				{
					// if true run this
					if(Input.GetTouch(i).phase == TouchPhase.Began)
					{
						// used to sen message b/c function is not present in this script 
						this.SendMessage("OnTouchBegan");
						//OnTouchBegan(); // can't do this
						//Debug.Log("The touch has begun on " + this.name);	
					}
					
					if(Input.GetTouch(i).phase == TouchPhase.Ended)
					{
						this.SendMessage("OnTouchEnded");
						//Debug.Log("The touch has ended on " + this.name);	
					}
				}
				
			}
		}
	}
}
