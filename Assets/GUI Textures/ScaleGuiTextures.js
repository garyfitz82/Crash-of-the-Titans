//Aspect Correction Script for Unity - Zerofractal 2006
//This script can be placed on any GUI element to make its aspect correct to 1:1. 
//The scale is based on the height of the element. 
//It works with textures of equal or unequal width and height, such as 1024x512 maps. 
//Unlike other scripts, this one takes the aspect from the actual screen making it useful for 4:3 16:9 etc etc.
 
function Awake() {
   RefreshAspect();
}
 
function RefreshAspect() {
   var aspect: float = 1/Camera.main.aspect;
   //transform.localScale.x = transform.localScale.y * aspect * guiElement.texture.width/guiElement.texture.height;
   transform.position = transform.localPosition;
}