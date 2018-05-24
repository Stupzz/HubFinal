using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Collections;
using UnityEngine.UI;


public class ChangeColor : MonoBehaviour {

	private bool readyStatus = false;
	// Use this for initialization
	void Start () {
		
	}

	public void UpdateColor(string playerName) {
		Image myPanel = GetComponent<Image> ();
		if (!readyStatus) {
			switch (playerName) {
			case "P1":
				myPanel.color = new Color (255, 0, 0);
				readyStatus = true;
				break;
			case "P2":
				myPanel.color = new Color (255, 0, 255);
				readyStatus = true;
				break;
			case "P3":
				myPanel.color = new Color (0, 0, 255);
				readyStatus = true;
				break;
			case "P4":
				myPanel.color = new Color (255, 255, 0);
				readyStatus = true;
				break;
			case "P5":
				myPanel.color = new Color (0, 255, 0);
				readyStatus = true;
				break;
			case "P6":
				myPanel.color = new Color (0, 255, 255);
				readyStatus = true;
				break;
			default:
				Debug.Log ("Change color default");
				break;
			}
		} else {
			myPanel.color = new Color32 (0xFF, 0xFF, 0xFF, 0x64);
			readyStatus = false;
		}
	}
}
