using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectStructure : MonoBehaviour {

	public RectTransform structureMenu = null;

	void OnMouseOver(){
		if(structureMenu != null && Input.GetMouseButtonDown(0)){
			structureMenu.gameObject.SetActive(true);
		}
	}

	void Update(){
		if(structureMenu != null && structureMenu.gameObject.active){
			if(Input.GetKeyDown(KeyCode.Escape)){
				structureMenu.gameObject.SetActive(false);
			}
		}
	}
}
