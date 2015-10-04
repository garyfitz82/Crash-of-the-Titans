//This is the script that is called up from the Die() function in the PlayerHealthGUI script. That script broadcasts out a message
//to this one to start the PlayerLost() function found below. This allowed me to shoot out a message to start this function even
//though the player car had been destroyed and the PlayerHealthGUI script was on that car...(destroyed car means destroyed script :) )

var youHaveDiedText : GUIText;

function Start () {
	youHaveDiedText.active = false; // this is making sure the pop-up text to let you know you've died is hidden when game is started
}
// This is the function that will reload the level after (4) seconds have passed and will also turn on the GUIText object
//to let you know that you've died.
function PlayerLost () {

youHaveDiedText.active = true; //turns on the GUIText visibility so you can see that humiliating message.

Debug.Log ("You Died and Lost this level"); //some readout text in the console. You can comment this out, it was for testing.
	
	yield WaitForSeconds (4); // this is just a (4) second delay before the next level is loaded below.
		
//this is what reloads the current level. You can change this to whatever level you'd like to 
//load after death occurs...such as ("MainMenu") or anything you name it. Just make sure it's named here, exactly
//what it's named in Unity's Build Settings window.
Application.LoadLevel("World-01");

}