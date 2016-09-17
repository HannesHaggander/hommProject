using UnityEngine;
using System.Collections;

public class EventBase : MonoBehaviour {

	public bool forcedEvent = false;
	[SerializeField]
	protected GameObject inCollider = null;

	public virtual void myTriggerEvent(){
		print("Should be overwritten by some other function");
	}

	public void OnTriggerEnter(Collider c){
		inCollider = c.gameObject;
		if(c.tag.Equals("Player")){
			if(forcedEvent){
				myTriggerEvent();
				c.GetComponent<PlayerMovement>().ForceCancelPathFinding();
			}
		}
	}

	public void OnTriggerExit(Collider c){
		inCollider = null;
		if(c.tag.Equals("Player")){
			
		}
	}
}
