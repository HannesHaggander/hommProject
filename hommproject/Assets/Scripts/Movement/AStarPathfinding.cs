using UnityEngine;
using System.Collections;

public class AStarPathfinding : MonoBehaviour {

	
	public bool pathfindingActive = false;

	private float lerpSpeed = 5f;

	public Vector3 currentTilePosition; //used for combat encoutners
	public Vector3 previousTilePosition; //used for combat encounters

	public ArrayList BlockedNodes = new ArrayList();
	public ArrayList attemptedWalkedTiles = new ArrayList(); // on each reset empty this list, fill it with walked tiles
	[SerializeField]
	public Vector3 newPos;
	[SerializeField]
	public Vector3 finalPos;
	public Vector3 tmpTile;

	public GameObject dummyObj = null;

	public ArrayList entirePath = new ArrayList();

	public LayerMask layerToStopPathfinding;

	// set the location for the entity 
	public void SetNewPos(int x, int y){
		if(!isTileAvailable(new Vector3(transform.position.x + x, transform.position.y + y, 0))){
			return;
		}
		
		if(Vector3.Distance(transform.position, newPos) < 0.1f){
			newPos.x = newPos.x + x;
			newPos.y = newPos.y + y;
			
		}
	}

	// move the entity towards the new location in fixed update
	/*public void MoveToNewPos(){
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
	}*/

	private bool setCombatTiles = false;
	void SetCurrentAndPreviousTile(){
		setCombatTiles = false;
		previousTilePosition = currentTilePosition;
		currentTilePosition = newPos;
		attemptedWalkedTiles.Add(previousTilePosition);
	}


	// finds the path toward the final position,
	// needs to be improved so that the entire path is calculated in post movement,
	// can easily be stuck by colliders 
	//TODO move to own file so that we can use it for other entities
	void FindClosestAdjacentToFinal(GameObject fromObj){
		if(BlockedNodes.Capacity > 64){
			print("blocked nodes had 500 blocked nodes, returning");
			BlockedNodes.Clear();
			BlockedNodes = new ArrayList();
			return;
		}
		if(Vector3.Distance(fromObj.transform.position, finalPos) > 0.05f){
			Vector3 up = fromObj.transform.position + Vector3.up;
			Vector3 right = fromObj.transform.position + Vector3.right;
			Vector3 down = fromObj.transform.position + Vector3.down;
			Vector3 left = fromObj.transform.position + Vector3.left;
			
			tmpTile = fromObj.transform.position;
			
			somethingIsAvailable = false;
			//check all adjacent tiles
			
			tmpTile = CheckTile(up);
			tmpTile = CheckTile(right);
			tmpTile = CheckTile(down);
			tmpTile = CheckTile(left);
			//check best tile
			SetCurrentAndPreviousTile();
			if(tmpTile == fromObj.transform.position){
				if(!BlockedNodes.Contains(tmpTile)){
					BlockedNodes.Add(tmpTile);
				}
				fromObj.transform.position = transform.position; // reset the dummy and block the tile that caused the reset
			} else {
				if(!entirePath.Contains(tmpTile)){
					entirePath.Add(tmpTile);
				}
				dummyObj.transform.position = tmpTile;	
			}

			FindClosestAdjacentToFinal(fromObj);
		
		} else {
			// if the player is at the final position then do nothing
			pathfindingActive = false;
			return;
		}
	}


	Vector3 currentBestPosition = new Vector3(0,0,-10000);
	bool somethingIsAvailable = false;
	Vector3 CheckTile(Vector3 pos){
		if(isTileCloser(pos)){
			if(isTileAvailable(pos)){
				somethingIsAvailable = true;
				return pos;
			}
		} else if(isTileAvailable(pos) && !somethingIsAvailable){
			if(Vector3.Distance(currentBestPosition, finalPos) > Vector3.Distance(pos, finalPos)){
				currentBestPosition = pos;
				return pos;
			}
		}

		return tmpTile;
	}

	// performs a check wether the next move is closer to the final position
	bool isTileCloser(Vector3 tilepos){
		return Vector3.Distance(tilepos, finalPos) < Vector3.Distance(tmpTile, finalPos);
	}

	// performs checks to see if the tile is able to be on
	bool isTileAvailable(Vector3 endPos){
		if(Physics.Linecast(dummyObj.transform.position, endPos, layerToStopPathfinding)){
			print("not available because of linecast: " + endPos);
			return false;
		}
		
		if(BlockedNodes.Contains(endPos)){
			print("Not available because of " + endPos + " is in blocked tiles");
			return false;
		} // cant go to previously blocked nodes
	
		
		if(attemptedWalkedTiles.Contains(endPos)){
			print("not available because of " + endPos + " is walked on already");
			return false;
		}

		return true;
	}


	// initiate the pathfinding sequence
	void TakeFinalPath(Vector3 newFinalPos){
		pathfindingActive = true;
		finalPos = newFinalPos;
		FindClosestAdjacentToFinal(dummyObj);
	}

	// get the actual mouse position and move the entity toward the position
	public void GetMouseToFinalPos(){
		Vector3 rawMousepos = Input.mousePosition;
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(rawMousepos);
		mousePos.x = Mathf.RoundToInt(mousePos.x);
		mousePos.y = Mathf.RoundToInt(mousePos.y);
		mousePos.z = 0;
		if(Physics.OverlapBox(mousePos, new Vector3(0.1f, 0.1f, 0.1f), Quaternion.identity, layerToStopPathfinding).Length > 0){
			return;
		}
		
		TakeFinalPath(mousePos);
	}

	// used for encounters to stop the player from moving
	public void ForceCancelPathFinding(){
		pathfindingActive = false;
	}

	public ArrayList CalculateEntirePath(){
		attemptedWalkedTiles.Clear();
		attemptedWalkedTiles = new ArrayList();
		BlockedNodes.Clear();
		BlockedNodes = new ArrayList();
		if(dummyObj == null){
			dummyObj = new GameObject();
			Rigidbody dummyRB =  dummyObj.AddComponent<Rigidbody>();
			dummyRB.isKinematic = true;
			dummyRB.useGravity = false;
		} 
		dummyObj.transform.position = transform.position;
		dummyObj.name = "dummy";
		if(!BlockedNodes.Contains(transform.position)){
			BlockedNodes.Add(transform.position); // block the initial tile
		}
		entirePath.Clear();
		entirePath = new ArrayList();
		GetMouseToFinalPos();
		return entirePath;
	}

	LineRenderer lr = null;
	GameObject holderNewLinerenderer = null;
	public void printPath(ArrayList posList){
		if(posList == null){ print("list is null"); return;}
		
		Vector3[] linePositions = new Vector3[posList.Count+1];
		if(holderNewLinerenderer == null){
 			holderNewLinerenderer = new GameObject();
		}
		if(holderNewLinerenderer.GetComponent<LineRenderer>() == null){
			holderNewLinerenderer.transform.SetParent(transform);
			holderNewLinerenderer.AddComponent<LineRenderer>();
			lr = holderNewLinerenderer.GetComponent<LineRenderer>();
		} 
		linePositions[0] = transform.position;
		int counter = 1;
		lr.SetVertexCount(posList.Count+1);
		lr.SetWidth(0.5f, 0.5f);
		if(posList.Count > 0){
			foreach(var v in posList){
				Vector3 ok = (Vector3) v;
				ok.z = -1;
				linePositions[counter] = ok;
				counter ++;
			}
			lr.SetPositions(linePositions);
		}
	}
}
