//This script is simply what draws those neat little white spheres at every single enemy spawn point in the Unity editor.
//You can change their size, and color within this script.

var radius = 5.0;

function OnDrawGizmos() {
    // Display the radius 
    Gizmos.color = Color.white;
    Gizmos.DrawWireSphere (transform.position, radius);
}