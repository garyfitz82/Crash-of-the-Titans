// This allows the script to also be executed in editor mode
@script ExecuteInEditMode()

//Enter in a name that will show above your healthbar in the game.
//(enter name in the inspector)
var playerName : String; 
var adjustmentZ : float= 1.5f;
var adjustmentY : float= 1.5f;
var maxHealth : float= 100;
var playerHealth : float = 100;
var healthTex: Texture2D;
var healthBarHeight : int = 10;
var healthBarScale: int = 80;
var deathFX : GameObject;
var deadPlayerPrefab : GameObject; //this is the object we swap out the healthy object for when the player dies.
var lose : GUIText;

private var worldPosition : Vector3 = new Vector3();
private var screenPosition : Vector3 = new Vector3();
private var myTransform : Transform;
private var myCamera : Camera;
private var barTop : int= 1;
private var healthBarLength : float= 100;
private var labelTop : int= 18;
private var labelWidth : int = 110;
private var labelHeight : int = 15;
private var healthBarLeft : int= 110;
private var myStyle : GUIStyle= new GUIStyle();

function Awake () 
{
	myTransform = transform;
	myStyle.normal.textColor = Color.white; //change the color, size and style of the font that will display the players name.
	myStyle.fontSize = 12;
	myStyle.fontStyle = FontStyle.Bold;
	myStyle.clipping = TextClipping.Overflow;
	lose.enabled = false;
}

function Update () 
{
	healthBarLength = (playerHealth/maxHealth) * healthBarScale;
}
	
//This function is triggered from another script called RocketDamage.js. The turrets fire rockets at you that contain this script on
//themselves. When it strikes you, it sends this message ApplyDamagePlayer() to this script to basically hurt you.
function ApplyDamagePlayer (playerDamage : float) {

	playerHealth -= playerDamage; // this is where the actual deduction of health points happens.
	// Are we dead?
	if (playerHealth < 1.0)
	{
		Die();	// this then sends the processing down to the Die() function at the bottom.
	}	//Look down at that function to see what happens.
}
// This fuction takes the players transform and converts it from world position to 
// screen position, we then draw the gui bar using these coordinates
function OnGUI () 
{
	worldPosition = new Vector3(myTransform.position.x , myTransform.position.y + adjustmentY,
                        myTransform.position.z + adjustmentZ);
    screenPosition = myCamera.main.WorldToScreenPoint(worldPosition); 
    

	GUI.Box(new Rect(screenPosition.x - healthBarLeft / 2, 
			Screen.height - screenPosition.y - barTop,
				healthBarScale + 4, healthBarHeight), "");
	
	GUI.DrawTexture(new Rect(screenPosition.x - healthBarLeft / 2 + 2,
			Screen.height - screenPosition.y - barTop + 2,
				healthBarLength, healthBarHeight - 4), healthTex);
	
	GUI.Label(new Rect(screenPosition.x - labelWidth / 6,
			Screen.height - screenPosition.y - labelTop,
				labelWidth, labelHeight), playerName, myStyle);
}	

function Die () 
{	
	if (gameObject != null)
    {     
        Destroy(gameObject);
    } 
	
	Instantiate(deathFX, transform.position, transform.rotation);
	Instantiate(deadPlayerPrefab, transform.position, transform.rotation);
	lose.enabled = true;
	//yield WaitForSeconds(5);
	Application.LoadLevel("menu");
	
}

