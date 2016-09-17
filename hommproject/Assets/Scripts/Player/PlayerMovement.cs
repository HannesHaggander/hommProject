using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	private float lerpSpeed = 5f;

	public Vector3 currentTilePosition; //used for combat encoutners
	public Vector3 previousTilePosition; //used for combat encounters

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

	// set the location for the entity 
	void SetNewPos(int x, int y){
		if(!isTileAvailable(new Vector3(transform.position.x + x, transform.position.y + y, 0))){
			return;
		}
		
		if(Vector3.Distance(transform.position, newPos) < 0.1f){
			newPos.x = newPos.x + x;
			newPos.y = newPos.y + y;
			
		}
	}

	// move the entity towards the new location in fixed update
	void MoveToNewPos(){
		if(Vector3.Distance(transform.position, newPos) > 0.05f){
			transform.position = Vector3.Lerp(transform.position, newPos, lerpSpeed * Time.deltaTime);
			setCombatTiles = true;
		} else {
			if(setCombatTiles){
				SetCurrentAndPreviousTile();
			}
			if(pathfindingActive){
				FindClosestAdjacentToFinal();

			}
		}
	}

	private bool setCombatTiles = false;
	void SetCurrentAndPreviousTile(){
		setCombatTiles = false;
		previousTilePosition = currentTilePosition;
		currentTilePosition = newPos;

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

	// performs a check wether the next move is closer to the final position
	bool isTileCloser(Vector3 tilepos){
		return Vector3.Distance(tilepos, finalPos) < Vector3.Distance(tmpTile, finalPos);
	}

	// performs checks to see if the tile is able to be on
	bool isTileAvailable(Vector3 endPos){
		return !Physics.Linecast(transform.position, endPos, layerToStopPathfinding);
	}

	// initiate the pathfinding sequence
	void TakeFinalPath(Vector3 newFinalPos){
		pathfindingActive = true;
		finalPos = newFinalPos;
		FindClosestAdjacentToFinal();
	}

	// get the actual mouse position and move the entity toward the position
	void GetMouseToFinalPos(){
		Vector3 rawMousepos = Input.mousePosition;
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(rawMousepos);
		mousePos.x = (int) mousePos.x;
		mousePos.y = (int) mousePos.y;
		mousePos.z = 0;
		
		TakeFinalPath(mousePos);

	}

	// used for encounters to stop the player from moving
	public void ForceCancelPathFinding(){
		pathfindingActive = false;
	}
}
