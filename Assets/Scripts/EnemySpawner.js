// Instantiates enemyPrefab at the location of all game objects tagged "Spawn".

var enemyPrefab : GameObject;


function Start() {

var enemies = GameObject.FindGameObjectsWithTag ("Spawn");
for (var spawn in enemies)
    Instantiate (enemyPrefab, spawn.transform.position, spawn.transform.rotation);
}
