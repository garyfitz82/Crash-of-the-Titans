//I use this script and apply it to the bullet decal object. It basically destroys the bullet decal object after (5) seconds
//have passed. If I didn't use this, we would have tons of bullet decals sitting around in our world.

function Update () {

// Kills the game object in 5 seconds after loading the object
Destroy (gameObject, 5);

}