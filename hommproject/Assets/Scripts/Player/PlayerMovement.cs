using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public string horizontalInput = "Horizontal";
	public string vericalInput = "Vertical";

	public string pathInputTrigger = "leftMouseBtn"; //pathfinding key

	private float horizontalSpeed = 0;
	private float verticalSpeed = 0;
	private bool leftmouseDown = false;

	private AStarPathfinding pathfinding = null;
	private ArrayList path = new ArrayList();

	// Use this for initialization
	void Start () {
		if(pathfinding == null){ pathfinding = gameObject.GetComponent<AStarPathfinding>(); }

		pathfinding.newPos = transform.position;
	}

	void Update () {
		horizontalSpeed = Input.GetAxisRaw(horizontalInput);
		verticalSpeed = Input.GetAxisRaw(vericalInput);
		if(Input.GetMouseButtonDown(0)){
			path = pathfinding.CalculateEntirePath(MasterObject.me.Correctmousepos());
			pathfinding.printPath(path);
		}		
	}

	void MoveToNextPosInPath(){
		
	}

	void FixedUpdate(){
	}
}
