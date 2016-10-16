using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour {

	public void ButtonExitTown(){
		SceneManager.LoadScene("overworld");
	}
}
