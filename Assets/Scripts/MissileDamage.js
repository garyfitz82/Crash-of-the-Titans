
var playerDamage : float = 10.0;
var playerHealthRef : GameObject;
var healthScript : PlayerHealthGUI;
function Start() {

playerHealthRef = GameObject.Find("Player");
healthScript = playerHealthRef.GetComponent(PlayerHealthGUI);

}

//when bullet object enters player collider the player will take damage.
function OnTriggerEnter(other : Collider) {
	
	//this is where we find and go hook into the other script for the player's health.
	
	if(other.tag == "Player")
	{
		Debug.Log("Collision");
		//other.GetComponent(PlayerHealthGUI).ApplyDamagePlayer(playerDamage);
		other.SendMessage("ApplyDamagePlayer", playerDamage);
	}
}
	
