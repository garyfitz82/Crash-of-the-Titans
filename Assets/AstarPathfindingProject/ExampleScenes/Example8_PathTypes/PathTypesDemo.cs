using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

/** Demos different path types.
 * This script is an example script demoing a number of different path types included in the project.
 * Since only the Pro version has access to many path types, it is only included in the pro version
 * \astarpro
 * 
 */
public class PathTypesDemo : MonoBehaviour {
	
	public int activeDemo = 0;
	
	public Transform start;
	public Transform end;
	
	public Vector3 pathOffset;
	
	public Material lineMat;
	public Material squareMat;
	public float lineWidth;
	
	public int searchLength = 1000;
	public int spread = 100;
	public float replaceChance = 0.1F;
	public float aimStrength = 0;
	//public LineRenderer lineRenderer;
	
	List<GameObject> lastRender = new List<GameObject>();
	
	List<Vector3> multipoints = new List<Vector3>();
	
	// Update is called once per frame
	void Update () {
		
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		Vector3 zeroIntersect = ray.origin + ray.direction * (ray.origin.y / -ray.direction.y);
		end.position = zeroIntersect;
		
		if (Input.GetKey (KeyCode.LeftShift) && Input.GetMouseButtonDown (0)) {
			
			multipoints.Add (zeroIntersect);
			
		}
		
		if (Input.GetKey (KeyCode.LeftControl) && Input.GetMouseButtonDown (0)) {
			multipoints.Clear ();
		}
		
		if (Input.GetMouseButtonDown (0)) { //Has the previous path returned or some timeout exceeded
			DemoPath ();
		}
		
	}
	
	/** Draw some helpful gui */
	public void OnGUI () {
		GUILayout.BeginArea (new Rect (5,5,200,Screen.height-10),"","Box");
		
		switch (activeDemo) {
		case 0:
			GUILayout.Label ("Basic path. Finds a path from point A to point B."); break;
		case 1:
			GUILayout.Label ("Multi Target Path. Finds a path quickly from one point to many others in a single search."); break;
		case 2:
			GUILayout.Label ("Randomized Path. Finds a path with a specified length in a random direction or biased towards some point when using a larger aim strenggth."); break;
		case 3:
			GUILayout.Label ("Flee Path. Tries to flee from a specified point. Remember to set Flee Strength!"); break;
		case 4:
			GUILayout.Label ("Finds all nodes which it costs less than some value to reach."); break;
		}
		
		GUILayout.Space (5);
		
		GUILayout.Label ("Note that the paths are rendered without ANY post-processing applied, so they might look a bit edgy");
		
		GUILayout.Space (5);
		
		GUILayout.Label ("Click anywhere to recalculate the path");
		
		if (activeDemo == 2 || activeDemo == 3 || activeDemo == 4) {
			GUILayout.Label ("Search Distance ("+searchLength+")");
			searchLength = Mathf.RoundToInt (GUILayout.HorizontalSlider (searchLength,0,100000));
		}
		
		if (activeDemo == 2 || activeDemo == 3) {
			GUILayout.Label ("Spread ("+spread+")");
			spread = Mathf.RoundToInt (GUILayout.HorizontalSlider (spread,0,40000));
			
			GUILayout.Label ("Replace Chance ("+replaceChance+")");
			replaceChance = GUILayout.HorizontalSlider (replaceChance,0,1);
			
			GUILayout.Label ((activeDemo == 2 ? "Aim strength" : "Flee strength") + " ("+aimStrength+")");
			aimStrength = GUILayout.HorizontalSlider (aimStrength,0,1);
		}
		
		if (activeDemo == 1) {
			GUILayout.Label ("Hold shift and click to add new target points. Hold ctr and click to remove all target points");
		}
		
		if (GUILayout.Button ("A to B path")) activeDemo = 0;
		if (GUILayout.Button ("Multi Target Path")) activeDemo = 1;
		if (GUILayout.Button ("Random Path")) activeDemo = 2;
		if (GUILayout.Button ("Flee path")) activeDemo = 3;
		if (GUILayout.Button ("Constant Path")) activeDemo = 4;
		
		GUILayout.EndArea ();
	}
	
	/** Get the path back */
	public void OnPathComplete (Path p) {
		
		//System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch ();
		//watch.Start ();
		
		//To prevent it from creating new GameObjects when the application is quitting when using multithreading.
		if(lastRender == null) return;
		
		if (p.error) {
			ClearPrevious ();
			return;
		}
		
		
		if (p.GetType () == typeof (MultiTargetPath)) {
			
			List<GameObject> unused = new List<GameObject> (lastRender);
			lastRender.Clear ();
			
			MultiTargetPath mp = p as MultiTargetPath;
			
			for (int i=0;i<mp.vectorPaths.Length;i++) {
				if (mp.vectorPaths[i] == null) continue;
				
				Vector3[] vpath = mp.vectorPaths[i];
				
				GameObject ob = null;
				if (unused.Count > i && unused[i].GetComponent<LineRenderer>() != null) {
					ob = unused[i];
					unused.RemoveAt (i);
				} else {
					ob = new GameObject ("LineRenderer_"+i,typeof(LineRenderer));
				}
				
				LineRenderer lr = ob.GetComponent<LineRenderer>();
				lr.sharedMaterial = lineMat;
				lr.SetWidth (lineWidth,lineWidth);
				
				lr.SetVertexCount (vpath.Length);
				for (int j=0;j<vpath.Length;j++) {
					lr.SetPosition (j,vpath[j] + pathOffset);
				}
				
				lastRender.Add (ob);
			}
			
			for (int i=0;i<unused.Count;i++) {
				Destroy (unused[i]);
			}
			
		} else if (p.GetType () == typeof (ConstantPath)) {
			
			ClearPrevious ();
			//The following code will build a mesh with a square for each node visited
			
			ConstantPath constPath = p as ConstantPath;
			List<Node> nodes = constPath.allNodes;
			
			//Nodes might contain duplicates, this will get rid of them
			//Dictionary<Node,bool> map = new Dictionary<Node, bool>();
			HashSet<Node> map = new HashSet<Node> ();
			
			Mesh mesh = new Mesh ();
			
			List<Vector3> verts = new List<Vector3>();
			
			bool drawRaysInstead = false;
			
			//This will loop through the nodes from furthest away to nearest, not really necessary... but why not :D
			//Note that the reverse does not, as common sense would suggest, loop through from the closest to the furthest away
			//since is might contain duplicates and only the node duplicate placed at the highest index is guarenteed to be ordered correctly.
			for (int i=nodes.Count-1;i>=0;i--) {
				//Check if we have already processed this node before
				if (!map.Contains (nodes[i])) {
					
					Vector3 pos = (Vector3)nodes[i].position+pathOffset;
					if (verts.Count	== 65000 && !drawRaysInstead) {
						Debug.LogError ("Too many nodes, rendering a mesh would throw 65K vertex error. Using Debug.DrawRay instead for the rest of the nodes");
						drawRaysInstead = true;
					}
					
					if (drawRaysInstead) {
						Debug.DrawRay (pos,Vector3.up,Color.blue);
						continue;
					}
					
					map.Add (nodes[i]);
					
					//Add vertices in a square
					
					GridGraph gg = AstarData.GetGraph (nodes[i]) as GridGraph;
					float scale = 1F;
					
					if (gg != null) scale = gg.nodeSize;
					
					verts.Add (pos+new Vector3 (-0.5F,0,-0.5F)*scale);
					verts.Add (pos+new Vector3 (0.5F,0,-0.5F)*scale);
					verts.Add (pos+new Vector3 (-0.5F,0,0.5F)*scale);
					verts.Add (pos+new Vector3 (0.5F,0,0.5F)*scale);
				}
			}
			
			//Build triangles for the squares
			Vector3[] vs = verts.ToArray ();
			int[] tris = new int[(3*vs.Length)/2];
			for (int i=0, j=0;i<vs.Length;j+=6, i+=4) {
				tris[j+0] = i;
				tris[j+1] = i+1;
				tris[j+2] = i+2;
				
				tris[j+3] = i+1;
				tris[j+4] = i+3;
				tris[j+5] = i+2;
			}
			
			Vector2[] uv = new Vector2[vs.Length];
			//Set up some basic UV
			for (int i=0;i<uv.Length;i+=4) {
				uv[i] = new Vector2(0,0);
				uv[i+1] = new Vector2(1,0);
				uv[i+2] = new Vector2(0,1);
				uv[i+3] = new Vector2(1,1);
			}
			
			mesh.vertices = vs;
			mesh.triangles = tris;
			mesh.uv = uv;
			mesh.RecalculateNormals ();
			
			GameObject go = new GameObject("Mesh",typeof(MeshRenderer),typeof(MeshFilter));
			MeshFilter fi = go.GetComponent<MeshFilter>();
			fi.mesh = mesh;
			MeshRenderer re = go.GetComponent<MeshRenderer>();
			re.material = squareMat;
			
			lastRender.Add (go);
			
		} else {
			
			ClearPrevious ();
			
			GameObject ob = new GameObject ("LineRenderer",typeof(LineRenderer));
			LineRenderer lr = ob.GetComponent<LineRenderer>();
			lr.sharedMaterial = lineMat;
			lr.SetWidth (lineWidth,lineWidth);
			
			lr.SetVertexCount (p.vectorPath.Length);
			for (int i=0;i<p.vectorPath.Length;i++) {
				lr.SetPosition (i,p.vectorPath[i] + pathOffset);
			}
			
			lastRender.Add (ob);
		}
	}
	
	/** Destroys all previous render objects */
	public void ClearPrevious () {
		for (int i=0;i<lastRender.Count;i++) {
			Destroy (lastRender[i]);
		}
		lastRender.Clear ();
	}
	
	/** Clears renders on application quit */
	public void OnApplicationQuit () {
		ClearPrevious ();
		lastRender = null;
	}
	
	/** Starts a path specified by PathTypesDemo::activeDemo */
	public void DemoPath () {
		
		Path p = null;
		
		if (activeDemo == 0) {
			p = new Path (start.position,end.position, OnPathComplete);
		} else if (activeDemo == 1) {
			MultiTargetPath mp = new MultiTargetPath (multipoints.ToArray (), end.position, null, OnPathComplete);
			p = mp;
		} else if (activeDemo == 2) {
			RandomPath rp = new RandomPath (start.position,searchLength, OnPathComplete);
			rp.spread = spread;
			rp.aimStrength = aimStrength;
			rp.aim = end.position;
			rp.replaceChance = replaceChance;
			
			p = rp;
		} else if (activeDemo == 3) {
			FleePath fp = new FleePath (start.position, end.position, searchLength, OnPathComplete);
			fp.fleeStrength = aimStrength;
			fp.replaceChance = replaceChance;
			fp.spread = spread;
			
			p = fp;
		} else if (activeDemo == 4) {
			ConstantPath constPath = new ConstantPath (start.position, searchLength, OnPathComplete);
			
			p = constPath;
		}
		
		if (p != null) AstarPath.StartPath (p);
	}
}
