using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(Collider))]
public class CastleEvent : EventBase {

	public string ownedBy = "";
	public int townID = 0;

	void Start(){
		base.forcedEvent = false;	
		if(!MasterObject.me.isTownRegistered(transform.name)){
			MasterObject.me.RegisterTown(transform.name, "townscreen");	
		}
	}

	public override void myTriggerEvent(){
		ownedBy = base.inCollider.transform.root.name;
		MasterObject.SaveState.SaveData();
		MasterObject.me.loadTown(transform.name);
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Jump") 
				&& base.inCollider != null 
				&& base.allowedTags.Contains(base.inCollider.tag)){ //base.inCollider.tag.Equals("Player")){
			myTriggerEvent();
		}
	}
}
