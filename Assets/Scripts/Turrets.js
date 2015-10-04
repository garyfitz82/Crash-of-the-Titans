
//This is how far away you have to be before the turret begin to shoot at you. It can be adjusted in the inspector.
var distanceToEngage = 15.0;
var distance : float;
//the player are the target and this gets 
//automatically assigned down in the Start() function.
private var target : Transform; 
var ammoPrefab : Transform; 
private var fireCycle : float;
var fireDelay : float; //adjust this to time how often the turrets can fire

function Start() 
{
	target = GameObject.FindWithTag ("Player").transform; //This is where we assign the target so the turrets know what to shoot at.
}

function Update() 
{
// checks if the player is within range
	if(target != null)
	{
		distance = Vector3.Distance(target.position, transform.position);

		if(distance < distanceToEngage)
		{		
			//Follows target
			transform.LookAt(target);
		}
		if(Time.time > fireCycle && distance < distanceToEngage)
		{
			//Fire
			fireCycle = Time.time + fireDelay;
			Fire();
		}
	}
}


//This is the code block that sends out a rocket to fire at the player.

function Fire()
{

	var ammo = Instantiate(ammoPrefab,transform.Find("FirePoint").transform.position , transform.Find("FirePoint").transform.rotation);

	ammo.rigidbody.AddForce(transform.forward * 3500);
	ammo.rigidbody.AddForce(transform.up * 130);

}