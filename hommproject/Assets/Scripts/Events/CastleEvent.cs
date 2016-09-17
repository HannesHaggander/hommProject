using UnityEngine;
using System.Collections;

public class CastleEvent : EventBase {

	void Start(){
		base.forcedEvent = false;		
	}

	public override void myTriggerEvent(){
		print("Test");
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Jump") && base.inCollider.tag.Equals("Player")){
			myTriggerEvent();
		}
	}
}
