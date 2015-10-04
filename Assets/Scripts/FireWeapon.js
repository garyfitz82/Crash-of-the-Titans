var hitDecal : GameObject;
private var decalClone : GameObject;
var GunSmoke : GameObject;
var shotSound: AudioClip; // drag a shot sound here, if any
var firingLight : Light; 
var sparksPrefab : GameObject; // drag the sparks prefab here, if any
var dirtPrefab : GameObject;
var muzzleFlash : GameObject; // this is the model for the muzzle flash effect
var muzzleFlashLength : float = 0.1; // this is how long the muzzle flash will last while firing
var maxScale : float = 0.25; // this is the muzzle flashes scale maximum
var damageAmount : float = 10.0;
var shootButton : GUITexture;

function Start() 
{
	muzzleFlash.renderer.enabled = false;
	firingLight.enabled = false;
}

function Update() 
{
	for(var touch : Touch in Input.touches)
	 {
		if ( shootButton.HitTest (touch.position) && touch.phase == TouchPhase.Began)
		{
			Shoot();
		}
	}
}

function Shoot()
{
	if(GunSmoke)
	{ //Check if the object exists
		Instantiate (GunSmoke, transform.position, transform.rotation);
		muzzleFlash.renderer.enabled = true;
		firingLight.enabled = true;
		yield WaitForSeconds (muzzleFlashLength);
		muzzleFlash.renderer.enabled = false;
		firingLight.enabled = false;
	}
	
	if (shotSound)
	{ 
		audio.PlayOneShot(shotSound); // play the shot sound
	    var hit: RaycastHit;
	}   
	if (Physics.Raycast(transform.position, transform.forward, hit, 800.0))
	{
	 		Debug.DrawLine (transform.position, hit.point);
		
	        // prepare rotation to instantiate sparks
			var rot = Quaternion.FromToRotation(Vector3.up, hit.normal);
		if (hit.transform.tag == "Enemy")  // if enemy hit
		{ 
			hit.transform.SendMessage("ApplyDamage", (damageAmount), SendMessageOptions.DontRequireReceiver); // and consume its health
			if (sparksPrefab)
			{ 
				Instantiate(sparksPrefab, hit.point, rot);
			}
		}
		else if (hit.transform.tag == "Ground")
		{ // if ground hit
			Instantiate(dirtPrefab, hit.point, rot);
			decalClone = Instantiate(hitDecal, hit.point, rot);
	
	    }
    }
}
