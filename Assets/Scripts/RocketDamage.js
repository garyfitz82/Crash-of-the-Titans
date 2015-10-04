
var explosionPrefab : GameObject;
var dirtPrefab : GameObject;
var playerDamage : int = 10.0;
private var objWithPlayerHealth : GameObject;

function Start() 
{
	objWithPlayerHealth = GameObject.Find("Player");
}

//when bullet object enters player collider the player will take damage.
function OnTriggerEnter (other : Collider) 
{
	//this is where we broadcast the message ApplyDamagePlayer 
	//to the script for the player's health.
	var PlayerHealthGUI : PlayerHealthGUI = other.GetComponent (PlayerHealthGUI);
	
	if(other.gameObject.tag == "Player")
	{
		objWithPlayerHealth.BroadcastMessage("ApplyDamagePlayer", playerDamage);	
		//if the missile collides create the explosion prefab
		Instantiate(explosionPrefab, transform.position, transform.rotation);
		// Destroy the projectile
		Destroy (gameObject);
	}
	else if(other.gameObject.tag == "Ground")	
	{
		Instantiate(dirtPrefab, transform.position, transform.rotation);

		// Destroy the projectile
		Destroy (gameObject);
	}
}
