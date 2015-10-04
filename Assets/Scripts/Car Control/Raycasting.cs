using UnityEngine;
using System.Collections;

public class Raycasting : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		
		
		Vector3 fside;
		Vector3 rside;
	    Vector3 strt;
		fside = transform.position;
		rside = transform.position;
		strt = transform.position;

		
		fside.y += .7f;
		rside.y += .7f;
		strt.y += .7f;
		
	
		
	    Vector3 forward = transform.TransformDirection(Vector3.forward) * 5;
        Debug.DrawRay(strt, transform.forward*20, Color.green);
		
	     Debug.DrawRay(rside, (transform.forward+transform.right*-.5f)*18, Color.green);
		 Debug.DrawRay(fside, (transform.forward+transform.right*.5f)*18, Color.green);
		RaycastHit  hit;
		
	
		if(Physics.Raycast(strt,transform.forward, out hit, 6)) {
			if(hit.collider.gameObject.tag == "Player"){
	
				transform.Rotate(Vector3.up, 90 * 10* Time.smoothDeltaTime);
		     Debug.DrawRay(transform.position, transform.forward, Color.red);
				Debug.Log("Avoiding Player");
			}
		}
		
		
		
		if(Physics.Raycast(rside,(transform.forward+transform.right*-.5f)*5, out hit, 5)) {
			if(hit.collider.gameObject.tag == "Player"){
		
		     transform.Rotate(Vector3.up, 90 * 4* Time.smoothDeltaTime);
		     Debug.DrawRay(transform.position, (transform.forward+transform.right*-.5f)*5, Color.red);
				Debug.Log("Avoiding Player");
			}
		}
		if(Physics.Raycast(fside,(transform.forward+transform.right*.5f)*5, out hit, 5)) {
			if(hit.collider.gameObject.tag == "Player"){
		
				transform.Rotate(Vector3.up, -90 * 4* Time.smoothDeltaTime);
		     Debug.DrawRay(transform.position , (transform.forward+transform.right*.5f)*5, Color.red);
				Debug.Log("Avoiding Player");
			}
		}
		
	}
}