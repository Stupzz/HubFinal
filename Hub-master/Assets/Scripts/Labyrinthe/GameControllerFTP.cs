using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControllerFTP : MonoBehaviour {


	public float decompte;
	public Text timeLeft;
	public Text timeLeft2;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (decompte != 0) {
			decompte = decompte - Time.deltaTime;
		}
		timeLeft.text = ((int)decompte).ToString ();
		timeLeft2.text = ((int)decompte).ToString ();
		if (decompte < 0){
			SceneManager.LoadScene("Panda", LoadSceneMode.Additive);
			decompte = 0;
			SceneManager.UnloadScene (SceneManager.GetSceneByName ("FTP"));
		}
	}
}