using UnityEngine;
using System.Collections;

public class MasterObject : MonoBehaviour {
    public static MasterObject me = null;

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

    [SerializeField]
    bool isGenerationComplete = false;
    public bool isCombatMapgenerationComplete(){
        return isGenerationComplete;
    }

    public void setIsGenerationComplete(bool b){
        isGenerationComplete = b;
    }

    GameObject[] heroArmy = new GameObject[5];
    GameObject[] enemyArmy = new GameObject[5];
    
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

    public GameObject[] getEnmyArmy(){
        return enemyArmy;
    }
}
