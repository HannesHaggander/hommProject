using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.IO;

[RequireComponent (typeof(SaveState))]
public class MasterObject : MonoBehaviour {
    public static MasterObject me = null;
    public static SaveState saveState = null;
    public Hashtable map_towns = new Hashtable();

    public GameObject goodUnit;
    public GameObject evilUnit;

    public GameObject[] heroArmy = new GameObject[5];
    public GameObject[] enemyArmy = new GameObject[5];

    void OnLevelWasLoaded(int level){
        if(level == 0){
            SetEntitiesLoadedPositions();
        }
    }

	void Awake () {
        // makes sure that there only is one instance of the master object
	    if(me == null)
        {
            me = this;
            saveState = GetComponent<SaveState>();
            DontDestroyOnLoad(gameObject);
        } else if(me != this)
        {
            Destroy(gameObject);
        }
	}

    void Start(){
        LoadCombatScene();
    }

    public void LoadCombatScene(){

        SetArmyList(heroArmy, "Player");
        SetArmyList(enemyArmy, "NPC");
        //SceneManager.LoadScene("combatevent");
    }

    [SerializeField]
    bool isGenerationComplete = false;
    public bool isCombatMapgenerationComplete(){
        return isGenerationComplete;
    }

    public void setIsGenerationComplete(bool b){
        isGenerationComplete = b;
    }

    
    // if the army sender is he hero, save heroes army, it will always spawn on the left
    public void SetArmyList(GameObject[] army, string armyTag){
        if(armyTag.Equals("Player")){
            heroArmy = army;
        } 
        else {
            enemyArmy = army;
        }
    }

    public GameObject[] getHeroArmy(){
        return heroArmy;
    }

    public GameObject[] getEnemyArmy(){
        return enemyArmy;
    }

    public void RemoveUnitFromArmyList(GameObject unit){
        print("trying to find " + unit.name);
        for(int i = 0; i < 5; i++){
            int id = unit.GetComponent<AttributesBase>().combatId;
            if(id < 10 && heroArmy[id] != null){
                heroArmy[id] = null;
            }
            if(id >= 10 && enemyArmy[id-10] != null){
                enemyArmy[id-10] = null;
            }
        }   
        checkIfArmyDead();
    }

    private void checkIfArmyDead(){
        bool heroArmyDead = true;
        bool enemyArmyDead = true;
        foreach(GameObject g in heroArmy){
            if(g != null){
                heroArmyDead = false;
            }
        }
        foreach(GameObject g in enemyArmy){
            if(g != null){
                enemyArmyDead = false;
            }
        }

        GameObject.FindGameObjectWithTag("CombatManager").GetComponent<CombatManager>().isBattleOver(heroArmyDead, enemyArmyDead);
    }

    public Vector3 Correctmousepos(){
        Vector3 rawMousepos = Input.mousePosition;
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(rawMousepos);
		mousePos.x = Mathf.RoundToInt(mousePos.x);
		mousePos.y = Mathf.RoundToInt(mousePos.y);
		mousePos.z = 0;
        return mousePos;
    }

    public bool isTownRegistered(string townName){
        return map_towns.Contains(townName);
    }

    public void RegisterTown(string townName, string sceneName){
        print("register town: " + townName + " load scene: " + sceneName);
        map_towns.Add(townName, sceneName);
    }

    public string getTown(string townName){
        return (string) map_towns[townName];
    }

    public void loadTown(string townName){
        saveState.SaveTmp();
        if(map_towns.Contains(townName)){
            SceneManager.LoadScene((string) map_towns[townName]);
        }
        else {
            print("missing scene: " + townName + "...");
        }
    }
    
    public ArrayList allEntities = new ArrayList();
    public void RegisterEntity(GameObject entity){
        if(!allEntities.Contains(entity)){
            allEntities.Add(entity);
        }        
    }    

    public ArrayList GetAllEntities(){
        print("<<<CHANGE THIS LATER; REGISTER ALL ENTITIES ON SPAWN>>>");
        ArrayList entities = new ArrayList();
		GameObject[] playerPos = GameObject.FindGameObjectsWithTag("Hero");
		GameObject[] entityPos = GameObject.FindGameObjectsWithTag("Entity");

		if(playerPos.Length > 0){
			foreach(GameObject g in playerPos){
				print("Added Hero: '" + g.transform.name + "'");
				entities.Add(g);
			}	
		}

		if(entityPos.Length > 0){
			foreach(GameObject g in entityPos){
				print("Added Entity: " + g.transform.name);
				entities.Add(g);
			}
		}

		return entities;
    }

    public void SetEntitiesLoadedPositions(){
        
        Debug.Log("getting pos\n==============");
        GameData tmpLoadedData = saveState.LoadTmpSave();
        if(tmpLoadedData != null){
            foreach(GameObject g in tmpLoadedData.getEntities()){
                LocalPrint(g.ToString());
            }
        }
    }

    private void LocalPrint(string s){
        Debug.Log("MasterObject :: " + s);
    }
}
