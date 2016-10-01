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
	public GameObject SpawnPlacement = null;
	public ArrayList heroSpawnpoints = new ArrayList(5);
	public ArrayList enemySpawnpoints = new ArrayList(5);
	public ArrayList instantiatedList = new ArrayList(); 

	void Start(){
		MasterObject.me.setIsGenerationComplete(false);
		leftTileType = "Desert";
		rightTileType = "Forest";
		

		TilesLeft = LoadGameObjectList(basePath + leftTileType);
		TilesRight = LoadGameObjectList(basePath + rightTileType);
		SpawnPlacement = Resources.Load(basePath + "Spawner/Spawner") as GameObject;
		GenerateHalf("left", TilesLeft);
		GenerateHalf("Right", TilesRight);
		SpawnHeroUnits(MasterObject.me.getHeroArmy());
		SpawnEnemyUnits(MasterObject.me.getEnemyArmy());

		MasterObject.me.setIsGenerationComplete(true);
		SpawnCombatManager();
	}

	private void SpawnCombatManager(){
		GameObject combatManager = new GameObject();
		CombatManager cm = combatManager.AddComponent<CombatManager>();
		combatManager.name = "CombatManager";
		combatManager.tag = "CombatManager";
		cm.SetTurnList(instantiatedList);
	}

	public GameObject HeroUnitTest;
	void SpawnHeroUnits(GameObject[] heroArmy){
		for(int i = 0; i < heroArmy.Length; i++){
			if(heroArmy[i] != null){
				GameObject spawnedUnit = Instantiate(heroArmy[i], (Vector3) heroSpawnpoints[i], Quaternion.identity) as GameObject;
				spawnedUnit.name = "hero_unit_ " + i.ToString();
				spawnedUnit.GetComponent<AttributesBase>().combatId = i;
				instantiatedList.Add(spawnedUnit);
			}
		}
	}

	public GameObject EnemyUnitTest;
	void SpawnEnemyUnits(GameObject[] enemyArmy){
		for(int i = 0; i < enemyArmy.Length; i++){
			if(enemyArmy[i] != null){
				GameObject spawnedUnit = Instantiate(enemyArmy[i], (Vector3) enemySpawnpoints[i], Quaternion.identity) as GameObject;
				spawnedUnit.name = "enemy_unit_ " + i.ToString();
				spawnedUnit.GetComponent<AttributesBase>().combatId = i + 10;
				instantiatedList.Add(spawnedUnit);
			}
		}
	}

	void GenerateHalf(string side, GameObject[] tiles){
		int start = side.Equals("left") ? 0 : 10;

		for(int i = -9; i < 1; i++){
			for(int j = start; j < start + 10; j++){
				GameObject tmpGO = Instantiate(tiles[Random.Range(0, tiles.Length)], new Vector3(j, i, 0), Quaternion.identity) as GameObject;
				tmpGO.transform.SetParent(transform);
				tmpGO.transform.name = string.Format("r_ {0:d}-c_{1:d}", j, i); // j = row, i = column
				if((j == 1 | j == 18) && i%2==0){
					GameObject tmpSpawnPoint = Instantiate(SpawnPlacement, new Vector3(j, i, 0), Quaternion.identity) as GameObject; 
					if(j == 1) {
						heroSpawnpoints.Add(tmpSpawnPoint.transform.position);
					}
					if(j == 18){
						enemySpawnpoints.Add(tmpSpawnPoint.transform.position);
					}
				}
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

	void PlaceSpawnPoints(Vector3 argPosition){
		Instantiate(SpawnPlacement, argPosition, Quaternion.identity);		
		print("Spawned entity spawner at " + argPosition); 
	}
}
