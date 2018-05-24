using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class ChangeColor : MonoBehaviour {

	//public  myPanel;
	// Use this for initialization
	void Start () {
		
	}

	public void UpdateColor() {
		Image myPanel = GetComponent<Image>();
		myPanel.color = new Color(0,200,0);
	}
}
