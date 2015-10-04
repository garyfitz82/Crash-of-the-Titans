using UnityEngine;
using System.Collections;

public class TouchButtonTalk : TouchButtonLogic 
{

	void OnTouchBegan()
	{
		this.guiText.material.color = Color.black;
		Debug.Log("The touch has begun on " + this.name);	
	}
	
	void OnTouchEnded()
	{
		Debug.Log("The touch has ended on " + this.name);
		this.guiText.material.color = Color.white;
		StartCoroutine(StartLevel(2.0f));
		Application.LoadLevel("Test");
	}
	
	IEnumerator StartLevel(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
	}
}
	