using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MasterObject : MonoBehaviour {
    public static MasterObject me = null;

    public Hashtable map_towns = new Hashtable();

    public GameObject goodUnit;
    public GameObject evilUnit;

    public GameObject[] heroArmy = new GameObject[5];
    public GameObject[] enemyArmy = new GameObject[5];

    void OnLevelWasLoaded(int level){
        if(level == 0){
            GetPlayers();
        }
    }

	void Awake () {
        // makes sure that there only is one instance of the master object
	    if(me == null)
        {
            me = this;
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
        if(map_towns.Contains(townName)){
            SavePlayers();
            SceneManager.LoadScene((string) map_towns[townName]);
        }
        else {
            print("missing scene: " + townName + "...");
        }
    }

    Hashtable playerPositions = new Hashtable();
    public void SavePlayers(){
        print("saving players");
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
        SavePlayerPosition(allPlayers);
    }
    
    public void SavePlayerPosition(GameObject[] players){
        if(players.Length > 0){
            foreach(GameObject g in players){
                playerPositions.Add(g.transform.name, g.transform.position);
            }
        }
    }

    public void GetPlayers(){
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
        if(allPlayers == null){
            print("All players is null");
            return; 
        }
        foreach(GameObject player in allPlayers){
            if(playerPositions.Contains(player.transform.name)){
                player.transform.position = (Vector3) playerPositions[player.transform.name];
            }
        }        
    }
}
