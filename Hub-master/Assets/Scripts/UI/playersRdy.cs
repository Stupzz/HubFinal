using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playersRdy : MonoBehaviour {

	public Text TextObject;

	public void onClick() {
		int nbRdy = int.Parse(TextObject.text);
		++nbRdy;
		TextObject.text = nbRdy.ToString();
	}
}
