using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public string pathInputTrigger = "leftMouseBtn"; //pathfinding key

	public float movementSpeed = 10;
	private bool leftmouseDown = false;
	private bool followPath = false;

	private AStarPathfinding pathfinding = null;
	private ArrayList path = new ArrayList();

	// Use this for initialization
	void Start () {
		if(pathfinding == null){ pathfinding = gameObject.GetComponent<AStarPathfinding>(); }
	}

	void Update () {
		if(Input.GetMouseButtonDown(0) && !followPath){
			travelCounter = 0;
			path = pathfinding.CalculateEntirePath(MasterObject.me.Correctmousepos());
			pathfinding.printPath(path);
		}

		if(Input.GetKeyDown(KeyCode.E)){
			print("moving");
			followPath = true;
		}
	}

	void FixedUpdate(){
		if(followPath){
			TravelPath();
		}
	}

	private int travelCounter = -1;
	/* Lerping through the path and setting the final position to ensure it's
	in the exact spot
	*/
	private void TravelPath(){
		if(path != null && path.Count > 0 && travelCounter >= 0 && travelCounter <= path.Count-1){
			//transform.position = (Vector3) path[travelCounter];
			if(Vector3.Distance(transform.position, (Vector3) path[travelCounter]) < 0.1f){
				transform.position = (Vector3) path[travelCounter];
				travelCounter++;
			}
			else {
				transform.position = Vector3.Lerp(transform.position, (Vector3) path[travelCounter], movementSpeed * Time.deltaTime);
			}

		}
		else {
			print("path: " + (path == null ? "null" : "set | ")
					+ " travel counter: " + travelCounter);
			followPath = false;
		}
	}
}
