using UnityEngine;
using System.Collections;

public class BattlefieldGenerator : MonoBehaviour {

	public int xTiles = 20;
	public int yTiles = 10;
	private string basePath =  "Prefabs/Tiles/";

	private string leftTileType = "default";
	private string rightTileType = "default";

	private Object[] tmp;
	public GameObject[] TilesLeft;	// played side
	public GameObject[] TilesRight;

	void Start(){
		leftTileType = "Desert";
		rightTileType = "Forest";

		TilesLeft = LoadGameObjectList(basePath + leftTileType);
		TilesRight = LoadGameObjectList(basePath + rightTileType);

		GenerateHalf("left", TilesLeft);
		GenerateHalf("Right", TilesRight);

	}

	void GenerateHalf(string side, GameObject[] tiles){
		int start = side.Equals("left") ? 0 : 10;

		for(int i = -9; i < 1; i++){
			for(int j = start; j < start + 10; j++){
				Instantiate(tiles[Random.Range(0, tiles.Length)], new Vector3(j, i, 0), Quaternion.identity);
			}
		}
	}

	GameObject[] LoadGameObjectList(string path){
		GameObject[] tmpGOList;
		tmp = Resources.LoadAll(path);
		tmpGOList = new GameObject[tmp.Length];
		
		for(int i = 0; i < tmp.Length; i++){
			tmpGOList[i] = (GameObject) tmp[i];
		}

		return tmpGOList;
	}
}
