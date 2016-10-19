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
			SaveFilePath = filePath + ".save";
		}
		BinaryFormatter bf = new BinaryFormatter();
		
		FileStream fileStream = File.Open(SaveFilePath, FileMode.OpenOrCreate);		 
		GameData data = new GameData();

		//write data
		ArrayList fetchEntity = MasterObject.me.GetAllEntities();
		foreach(object o in fetchEntity){
			GameObject go = (GameObject) o;
			data.insertEntity(go.name, go.name, go.transform.position);
		}

		bf.Serialize(fileStream, data);
		fileStream.Close();
	}

	public void SaveData(String gameName){
		SaveFilePath = filePath + "_" + gameName + ".save";
		SaveData();
	}

	public void SaveTmp(){
		String tmpFilePath = filePath + "_" + "tmp" + ".save";
		localPrint(tmpFilePath);
		if(File.Exists(tmpFilePath)){
			File.Delete(tmpFilePath);
		}
		SaveData("tmp");
	}

	// Load operations //
	public GameData LoadData(){
		if(LoadFilePath.Length == 0){
			LoadFilePath = filePath + ".save";
		}

		if(File.Exists(filePath)){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream fileStream = File.Open(LoadFilePath, FileMode.Open);
			GameData gameData = (GameData) bf.Deserialize(fileStream);
			if(gameData != null){
				localPrint("entitylength: " + gameData.getEntities().Length);
			}
			else {
				localPrint("gamedata is null");
			}
			fileStream.Close();
			return gameData;
			
		}
		GameData nullData = new GameData();
		return nullData;
	}

	public GameData LoadData(String gameName){
		LoadFilePath =  filePath + "_" + gameName + ".save";
		localPrint(LoadFilePath);
		return LoadData();
	}

	public GameData LoadTmpSave(){
		GameData tmpLoad = LoadData("tmp");
		return tmpLoad;		
	}

//////////////////////////////////////////////////////////////////////////////	

	private void localPrint(String s){
		print("SaveState :: " + s);
	}
}

////////////////////////////////////////////////////////////////////////

[Serializable]
public class GameData{
	//public ArrayList entityGameObjects = new ArrayList();
	public ArrayList Entities = new ArrayList();

	public void insertEntity(String id, string name, Vector3 position){
		if(!Entities.Contains(id)){
			EntityInformation EntInf = new EntityInformation(id, name, position);
			Entities.Add(EntInf);
		}
	}

	public GameObject[] getEntities(){
		GameObject[] tmpEntities = new GameObject[Entities.Count];
		for(int i = 0; i < Entities.Count - 1; i++){
			tmpEntities[i] = (GameObject) Entities[i];
		}
		return tmpEntities;
	}
}

[Serializable]
public class EntityInformation{
	public string id = "";
	public string name = "";
	public int xPos = 0;
	public int yPos = 0;
	public int zPos = 0;
	
	public EntityInformation(string id, string name, Vector3 position){
		this.id = id;
		this.name = name;
		this.xPos = (int) Mathf.Round(position.x);
		this.yPos = (int) Mathf.Round(position.y);
		this.zPos = (int) Mathf.Round(position.z);
		Debug.Log("saved entity info: " + this.toString());
	}

	public Vector3 getPos(){
		return new Vector3(xPos, yPos, zPos);
	}

	public string toString(){
		string s = "";
		s += "id: '" + id + "' ";
		s += "name: '" + name + "' ";
		s += "Vector3("+ xPos + ", " + yPos + ", " + zPos + ")";
		return s;
	}
}