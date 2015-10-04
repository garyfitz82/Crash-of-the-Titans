
// UnityGUI-based pause menu

#pragma strict
var pause : GUITexture;
var skin:GUISkin;
var startPaused:boolean = true;
var menutop:int=25;
var hudColor:Color = Color.white;
private var toolbarInt:int=0;
private var toolbarStrings: String[]= ["Audio","System"];
private var shoot : AudioSource;
// fill in the credit info for your game
var credits:String[]=[
	"Crash of th titans, Final Year Project by Gary Fitzgerald"] ;

enum Page {
	None,Main,Options,Restart,Credits
}

private var savedTimeScale:float;
private var currentPage:Page;

function Start() 
{
 shoot = GameObject.FindGameObjectWithTag("Bullet").GetComponentInChildren(AudioSource);	

 for(var touch : Touch in Input.touches)
	{
		if(pause.HitTest (touch.position) && touch.phase == TouchPhase.Began)
		{
			PauseGame();
		}
  
 	}
}

function Update() {
	for(var touch : Touch in Input.touches)
		{
		if(pause.HitTest (touch.position) && touch.phase == TouchPhase.Began)
			{
				switch (currentPage) 
				{
				case Page.None: PauseGame(); break; // if the pause menu is not displayed, then pause
				case Page.Main: UnPauseGame(); break; // if the main pause menu is displaying, then unpause
				default: currentPage = Page.Main; // any subpage goes back to main page
			}
		}
}
}

function OnGUI () {
	if (IsGamePaused()) {
		if (skin != null) {
			GUI.skin = skin;
		} else {
			GUI.color = hudColor;
		}
		switch (currentPage) {
			case Page.Main: ShowPauseMenu(); break;
			case Page.Options: ShowOptions(); break;
			case Page.Credits: ShowCredits(); break;
		}
	}	
}
// credits

function ShowCredits() {
	BeginPage(300,600);
	for (var credit in credits) {
		GUILayout.Label(credit);
	}
	EndPage();
}

// options

function ShowOptions() {
	BeginPage(600,600);
	toolbarInt = GUILayout.Toolbar (toolbarInt, toolbarStrings);
	switch (toolbarInt) {
		case 0: ShowAudio(); break;
		case 1: ShowSystem(); break;
	}
	EndPage();
}

function Available(isAvailable) {
	return isAvailable ? "Available" : "Not Available";
}

function ShowSystem() {
	GUILayout.Label("Graphics: "+SystemInfo.graphicsDeviceName+" "+
	SystemInfo.graphicsMemorySize+"MB\n"+
	SystemInfo.graphicsDeviceVersion+"\n"+
	SystemInfo.graphicsDeviceVendor);
	GUILayout.Label("Shadows: "+ Available(SystemInfo.supportsShadows));
	GUILayout.Label("Image Effects: "+Available(SystemInfo.supportsImageEffects));
	GUILayout.Label("Render Textures: "+Available(SystemInfo.supportsRenderTextures));
}

function ShowAudio() {
	GUILayout.Label("Volume");
	AudioListener.volume = GUILayout.HorizontalSlider(AudioListener.volume,0.0,1.0);
	shoot.volume = AudioListener.volume;
	
}

function BeginPage(width:int,height:int) {
	GUILayout.BeginArea(Rect((Screen.width-width)/2,menutop,width,height));
}

function EndPage() {
	// show Back button if not Main page
	if (currentPage != Page.Main && GUILayout.Button("Back")) {
		currentPage = Page.Main;
	}
	GUILayout.EndArea();
}

function ShowPauseMenu() 
{
	BeginPage(200,600);
	if (GUI.Button(Rect(0,0,200,60),"Play")) 
	{
		UnPauseGame();
	}
	if (GUI.Button (Rect(0,100,200,60),"Options")) 
	{
		currentPage = Page.Options;
	}
	
	if (GUI.Button (Rect(0,200,200,60),"Restart")) 
	{
		Restart();
	}
	
	if (GUI.Button (Rect(0,300,200,60),"Credits")) 
	{
		currentPage = Page.Credits;
	}
#if !UNITY_WEBPLAYER && !UNITY_EDITOR
	if (GUI.Button (Rect(0,400,200,60),"Quit")) 
	{
		Application.Quit();
	}
#endif
	EndPage();
}

function PauseGame() 
{
	savedTimeScale = Time.timeScale;
	Time.timeScale = 0;
	AudioListener.pause = true;
	currentPage = Page.Main;
}

function UnPauseGame() 
{
	Time.timeScale = savedTimeScale;
	AudioListener.pause = false;
	currentPage = Page.None;
}

function Restart()
{
	Application.LoadLevel("test");
	UnPauseGame();
}
static function IsGamePaused() 
{
	return Time.timeScale==0;
}



