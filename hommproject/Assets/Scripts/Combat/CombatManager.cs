using UnityEngine;
using System.Collections;
using System;
public class CombatManager : MonoBehaviour {

	public ArrayList turnList = new ArrayList(); 
	private GameObject turnlistUnitsTurn = null;
	private GameObject unitsTurn = null;
	private int turnCounter = 0;
	private int turnNumber = 1;

	void Update(){
		if(Input.GetKeyDown(KeyCode.K)){
			endUnitsTurn();
		}
	}

	public void SetTurnList(ArrayList speedList){
		turnList = SortBySpeed(speedList);
		getUnitTurn();
	}

	private void getUnitTurn(){
		print("turncounter: " + turnCounter + " size of list: " + turnList.Count);

		turnlistUnitsTurn = (GameObject) turnList[turnCounter];
		unitsTurn = turnlistUnitsTurn;
		print("CombatManger: its " + unitsTurn.name + "'s turn");
		unitsTurn.GetComponent<AttributesBase>().SetMyTurn(true);
	}

	private void endUnitsTurn(){
		turnCounter++;
		if(turnCounter == (turnList.Count)){
			print("resetting turncounter to 0");
			turnCounter = 0;
		}
		unitsTurn.GetComponent<AttributesBase>().SetMyTurn(false);
		print("<<<<<<<< Turn: " + turnNumber + " ended >>>>>>>>>>>");
		turnNumber++;
		getUnitTurn();
	}

	public ArrayList SortBySpeed(ArrayList speedList){
		bool swapped = true;
		while (swapped){ 
			swapped = false;
			for(int i = 0; i < speedList.Count; i++){
				int aSpeed = -1;
				int bSpeed = -1;
				aSpeed = ((GameObject) speedList[i]).GetComponent<AttributesBase>().getSpeed();
				if(i+1 < speedList.Count){
					bSpeed = ((GameObject) speedList[i+1]).GetComponent<AttributesBase>().getSpeed();
				} 
				else {
					break;
				}

				if(aSpeed < bSpeed){
					object tmp = speedList[i+1];
					speedList[i+1] = speedList[i];
					speedList[i] = tmp;
					swapped = true;
				}
			}
		}

		print(">>>sorted list");
		foreach(object o in speedList){
			GameObject g = o as GameObject;
			print("name " + g.name + ", speed " + g.GetComponent<AttributesBase>().getSpeed());
		}
		print("<<<");
		return speedList;
	}

	public void RemoveUnitFromList(GameObject unit){
		if(turnList.Contains(unit)){
			turnList.Remove(unit);	
			print("removed " + unit.name + " from the turn list");
		}
		else {
			print("could not remove " + unit.name + " from turn list");
		}
		MasterObject.me.RemoveUnitFromArmyList(unit);
	}

	public void isBattleOver(bool heroarmydead, bool enemyarmydead){
		print("is battle over?");
		if(heroarmydead){
			print("enemy won!");
		}
		if(enemyarmydead){
			print("hero won!");
		}
		// do end scene stuff. (save, go to world overview)

	}
}
