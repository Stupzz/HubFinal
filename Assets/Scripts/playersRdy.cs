using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playersRdy : MonoBehaviour {

	public Text TextObject;
	public Text buttonText;

	private bool readyStatus = false;

	public void onClick() {
		if (!readyStatus) {
			readyStatus = true;
			int nbRdy = int.Parse (TextObject.text);
			++nbRdy;
			TextObject.text = nbRdy.ToString ();
			buttonText.text = "Annuler";
		} else {
			readyStatus = false;
			int nbRdy = int.Parse (TextObject.text);
			--nbRdy;
			TextObject.text = nbRdy.ToString ();
			buttonText.text = "Jouer";
		}
	}
}
