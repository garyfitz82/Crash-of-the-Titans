@script ExecuteInEditMode()

var playerName : String;
var adjustmentZ : float= 1.5f;
var adjustmentY : float= 1.5f;
var maxHealth : float= 100;
var health : float = 100;
var healthTex: Texture2D;
var healthBarHeight : int = 10;
var healthBarScale: int = 80;
var deathFX : GameObject;
var deadPrefab : GameObject;
var isInView : boolean = false;

private var worldPosition : Vector3= new Vector3();
private var screenPosition : Vector3= new Vector3();
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
	myCamera = Camera.main;
	myStyle.normal.textColor = Color.green;
	myStyle.fontSize = 10;
	myStyle.fontStyle = FontStyle.Normal;
	myStyle.clipping = TextClipping.Overflow;
	GUI.color = Color.yellow;
	isInView = false;
}

function Update () 
{
	healthBarLength = (health/maxHealth) * healthBarScale;

}

function ApplyDamage (damage : float) 
{
	
	// Apply damage
	health -= damage;
		
	// Are we dead?
	if (health < 1.0)
	
	Die();
	
	isInView = true;
		yield WaitForSeconds (1);
	isInView = false;
		
}

function OnGUI () 
{
	if (!isInView) return;
	
	worldPosition = new Vector3(myTransform.position.x , myTransform.position.y + adjustmentY,
                        myTransform.position.z + adjustmentZ);
	screenPosition = myCamera.WorldToScreenPoint(worldPosition);

	GUI.Box(new Rect(screenPosition.x - healthBarLeft / 2,
			Screen.height - screenPosition.y - barTop,
				healthBarScale + 4, healthBarHeight), "");
	
	GUI.DrawTexture(new Rect(screenPosition.x - healthBarLeft / 2 + 2,
			Screen.height - screenPosition.y - barTop + 2,
				healthBarLength, healthBarHeight - 4), healthTex);
	
	GUI.Label(new Rect(screenPosition.x - labelWidth / 2.5,
			Screen.height - screenPosition.y - labelTop,
				labelWidth, labelHeight), playerName, myStyle);
}	


function Die () {
	Destroy(gameObject);
	Instantiate(deathFX, transform.position, transform.rotation);
	Instantiate(deadPrefab, transform.position, transform.rotation);
	
	
}
