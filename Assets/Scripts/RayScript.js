function Update () {
		var fwd = transform.TransformDirection (Vector3.forward);
		var right = transform.TransformDirection(Vector3.right);
		var left = right*-1;
		//if (Physics.Raycast(transform.position, fwd, 100)) {
			//print ("There is something in front of the object!");
			Debug.DrawRay(transform.position + Vector3(0,0.5,0), fwd*10, Color.green);
		//}
		
		//if (Physics.Raycast(transform.position, right, 100)) {
		//	print ("There is something to the right of the object!");
			Debug.DrawRay(transform.position + Vector3(0,0.5,0), right*10, Color.green);
			
			Debug.DrawRay(transform.position + Vector3(0,0.5,0), left*10, Color.green);
			
			Debug.DrawRay(transform.position + Vector3(1,0.5,0), fwd*10, Color.green);
			
			Debug.DrawRay(transform.position - Vector3(1,-0.5,0), fwd*10, Color.green);
		//}
	}
	