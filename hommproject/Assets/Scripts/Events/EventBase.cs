using UnityEngine;
using System.Collections;

public class EventBase : MonoBehaviour {

	public bool forcedEvent = false;
	[SerializeField]
	protected GameObject inCollider = null;
	
	public ArrayList allowedTags = null;

	void Awake(){
		allowedTags = new ArrayList(){"Player", "Hero", "Self"};
	}

	public virtual void myTriggerEvent(){
		print("Should be overwritten by some other function");
	}

	public void OnTriggerEnter(Collider c){
		inCollider = c.gameObject;
		if(allowedTags.Contains(c.tag)){
			print("player entered");
			if(forcedEvent){
				myTriggerEvent();
				c.GetComponent<PlayerMovement>().GetComponent<AStarPathfinding>().ForceCancelPathFinding();
			}
		}
	}

	public void OnTriggerExit(Collider c){
		inCollider = null;
		if(allowedTags.Contains(c.tag)){
			print("player exit");
		}
	}
}
