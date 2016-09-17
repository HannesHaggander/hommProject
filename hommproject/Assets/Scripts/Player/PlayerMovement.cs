using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	private float lerpSpeed = 5f;

	public string horizontalInput = "Horizontal";
	public string vericalInput = "Vertical";
	public string leftMouse = "leftMouseBtn";

	private float horizontalSpeed = 0;
	private float verticalSpeed = 0;
	private bool leftmouseDown = false;

	[SerializeField]
	private Vector3 newPos;
	[SerializeField]
	private Vector3 finalPos;
	private Vector3 tmpTile;

	public LayerMask layerToStopPathfinding;

	private bool pathfindingActive = false;
	// Use this for initialization
	void Start () {
		newPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		horizontalSpeed = Input.GetAxisRaw(horizontalInput);
		verticalSpeed = Input.GetAxisRaw(vericalInput);
		if(Input.GetMouseButtonDown(0)){
			GetMouseToFinalPos();
		}
	}

	void FixedUpdate(){
		if(Mathf.Abs(horizontalSpeed) > 0 ^ Mathf.Abs(verticalSpeed) > 0){
			pathfindingActive = false;
			SetNewPos((int) horizontalSpeed, (int) verticalSpeed);
		}
		MoveToNewPos();
	}

	void SetNewPos(int x, int y){
		if(!isTileAvailable(new Vector3(transform.position.x + x, transform.position.y + y, 0))){
			return;
		}
		
		if(Vector3.Distance(transform.position, newPos) < 0.1f){
			newPos.x = newPos.x + x;
			newPos.y = newPos.y + y;
			
		}
	}

	void MoveToNewPos(){
		if(Vector3.Distance(transform.position, newPos) > 0.05f){
			transform.position = Vector3.Lerp(transform.position, newPos, lerpSpeed * Time.deltaTime);

		} else {
			if(pathfindingActive){
				FindClosestAdjacentToFinal();

			}
		}
	}


	// finds the path toward the final position,
	// needs to be improved so that the entire path is calculated in post movement,
	// can easily be stuck by colliders 
	//TODO move to own file so that we can use it for other entities
	void FindClosestAdjacentToFinal(){

		if(Vector3.Distance(transform.position, finalPos) > 0.05f){
			Vector3 up = transform.position + Vector3.up;
			Vector3 right = transform.position + Vector3.right;
			Vector3 down = transform.position + Vector3.down;
			Vector3 left = transform.position + Vector3.left;
			
			tmpTile = transform.position;

			if(isTileAvailable(up)){ 
				if(isTileCloser(up)){ 
					tmpTile = up; 
				}
			}
			if(isTileAvailable(right)){
				if(isTileCloser(right)){ 
					tmpTile = right; 
				}
			}
			if(isTileAvailable(down)){
				if(isTileCloser(down)){ 
					tmpTile = down; 
				}
			}
			if(isTileAvailable(left)){
				if(isTileCloser(left)){ 
					tmpTile = left; 
				}
			} 

			newPos = tmpTile;
			

		} else {
			// if the player is at the final position then do nothing
			pathfindingActive = false;
			return;
		}

	}

	bool isTileCloser(Vector3 tilepos){
		return Vector3.Distance(tilepos, finalPos) < Vector3.Distance(tmpTile, finalPos);
	}

	bool isTileAvailable(Vector3 endPos){
		return !Physics.Linecast(transform.position, endPos, layerToStopPathfinding);
	}

	void TakeFinalPath(Vector3 newFinalPos){
		pathfindingActive = true;
		finalPos = newFinalPos;
		FindClosestAdjacentToFinal();
	}

	void GetMouseToFinalPos(){
		Vector3 rawMousepos = Input.mousePosition;
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(rawMousepos);
		mousePos.x = (int) mousePos.x;
		mousePos.y = (int) mousePos.y;
		mousePos.z = 0;
		
		TakeFinalPath(mousePos);

	}

	public void ForceCancelPathFinding(){
		pathfindingActive = false;
	}
}
