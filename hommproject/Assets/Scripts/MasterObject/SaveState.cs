using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveState : MonoBehaviour {

	private String filePath;
	private String SaveFilePath;
	private String LoadFilePath;

	void Awake(){
		filePath = Application.persistentDataPath + "/gameinfo";
		SaveFilePath = "";
		LoadFilePath = "";
	}

	// Save operations //
	public void SaveData(){
		if(SaveFilePath.Length == 0){
			SaveFilePath = filePath + ".dat";
		}
		BinaryFormatter bf = new BinaryFormatter();
		
		if(File.Exists(SaveFilePath)){
			File.Delete(SaveFilePath);
		}
		FileStream fileStream = File.Open(SaveFilePath, FileMode.OpenOrCreate);		 
		GameData data = new GameData();

		//write data
		//data.entityGameObjects = GetAllEntities();
		SaveEntityPositions(data);

		bf.Serialize(fileStream, data);
		fileStream.Close();
	}

	public void SaveData(String gameName){
		SaveFilePath = filePath + "_" + gameName + ".dat";
		SaveData();
	}

	// Load operations //
	public GameData LoadData(){
		if(LoadFilePath.Length == 0){
			LoadFilePath = filePath + ".dat";
		}

		if(File.Exists(filePath)){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream fileStream = File.Open(LoadFilePath, FileMode.Open);
			GameData gameData = (GameData) bf.Deserialize(fileStream);
			fileStream.Close();
			return gameData;
			
		}
		GameData nullData = new GameData();
		return nullData;
	}

	public GameData LoadData(String gameName){
		LoadFilePath =  filePath + "_" + gameName + ".dat";
		return LoadData();
	}

//////////////////////////////////////////////////////////////////////////////

	private ArrayList GetAllEntities(){
		return MasterObject.me.GetAllEntities();
	}
	
	private void SaveEntityPositions(GameData paramGameData){
		
		foreach(object o in MasterObject.me.GetAllEntities()){
			GameObject g = (GameObject) o;
			print("key: '" + g.transform.name + "' x: " + g.transform.position.x + " y: " + g.transform.position.y);
			paramGameData.entityPositionsX.Add(g.transform.name, g.transform.position.x);
			paramGameData.entityPositionsY.Add(g.transform.name, g.transform.position.y);
		}
	}

	private void localPrint(String s){
		print("SaveState :: " + s);
	}
}

////////////////////////////////////////////////////////////////////////

[Serializable]
public class GameData{
	//public ArrayList entityGameObjects = new ArrayList();
	public Hashtable entityPositionsX = new Hashtable();
	public Hashtable entityPositionsY = new Hashtable();

}