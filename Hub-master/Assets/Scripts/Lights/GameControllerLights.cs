using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class GameControllerLights : MonoBehaviour {

	private GameObject[] lights;
	//private ArrayList <Vector2> pos = new Array(new Vector2(-6, 2), new Vector2(0, 2), new Vector2(6, 2), new Vector2(-6, -2), new Vector2(0, -2), new Vector2(6, -2));
	private ArrayList pos = new ArrayList();


	// Use this for initialization
	void Start () {
		pos.Add (new Vector3 (-6, 2, 0));
		pos.Add (new Vector3 (0, 2, 0));
		pos.Add (new Vector3 (6, 2, 0));
		pos.Add (new Vector3 (-6, -2, 0));
		pos.Add (new Vector3 (0, -2, 0));
		pos.Add (new Vector3 (6, -2, 0));
		// Création du tableau de lumières et on le remplie avec les lumières
		lights = GameObject.FindGameObjectsWithTag ("Light");

		// On allument toutes les lumières
		for(int i = 0; i < lights.Length; i++)
		{
			lightOn (lights [i].name);
		}

		//On mélange les lumières
		while (checkAllOn (lights)) {
			scrambleLights (lights);
		}

		setPosLights (lights, pos);


	}
	
	// Update is called once per frame
	void Update () {
		//if(Input.touchCount > 0) {
			//for (int i = 0; i < Input.touchCount; i++) {
				 if ((Input.GetMouseButtonDown(0))) {
				RaycastHit2D raycastHit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);

				if (raycastHit.collider != null) {
					switch (raycastHit.collider.name) {
					case "Light_TL":
						switchOnOff ("Light_TL");
						break;
					case "Light_TC":
						switchOnOff ("Light_TC");
						break;
					case "Light_TR":
						switchOnOff ("Light_TR");	
						break;
					case "Light_BR":
						switchOnOff ("Light_BR");	
						break;
					case "Light_BC":
						switchOnOff ("Light_BC");	
						break;
					case "Light_BL":
						switchOnOff ("Light_BL");
						break;
					default:
						break;
					}
				//}
			}
		}
    }

	bool checkOn (string lightName) {
		if (GameObject.Find(lightName).GetComponent<SpriteRenderer>().color.Equals(new Color (255, 255, 0)))
			return true;
		else
			return false;
	}

	void lightOn (string lightName) {
        GameObject.Find(lightName).GetComponent<SpriteRenderer>().color = new Color (255, 255, 0);
	}

	void lightOff (string lightName) {
        GameObject.Find(lightName).GetComponent<SpriteRenderer>().color = new Color (137, 137, 137);
	}

	void switchOnOff ( string lightName) {
        //Haut Gauche
        if (lightName == "Light_TL")
        {
            //Change Haut Gauche
            if (checkOn("Light_TL"))
                lightOff("Light_TL");
            else
                lightOn("Light_TL");

            //Change Bas Centre
            if (checkOn("Light_BC"))
                lightOff("Light_BC");
            else
                lightOn("Light_BC");

            //Change Bas Gauche
            if (checkOn("Light_BL"))
                lightOff("Light_BL");
            else
                lightOn("Light_BL");
        }
        //Haute Droit
        else if (lightName == "Light_TR")
        {
            //Change Haut Gauche
            if (checkOn("Light_TL"))
                lightOff("Light_TL");
            else
                lightOn("Light_TL");
			
            //Change Bas Gauche
            if (checkOn("Light_BL"))
                lightOff("Light_BL");
            else
                lightOn("Light_BL");

            //Change Bas Droit
            if (checkOn("Light_BR"))
                lightOff("Light_BR");
            else
                lightOn("Light_BR");
        }
        //Haut Centre
        else if (lightName == "Light_TC")
        {
            //Change Haut Centre
            if (checkOn("Light_TC"))
                lightOff("Light_TC");
            else
                lightOn("Light_TC");

            //Change Haut Gauche
            if (checkOn("Light_TL"))
                lightOff("Light_TL");
            else
                lightOn("Light_TL");

            //Change Bas Centre
            if (checkOn("Light_BC"))
                lightOff("Light_BC");
            else
                lightOn("Light_BC");

            //Change Haut Droit
            if (checkOn("Light_TR"))
                lightOff("Light_TR");
            else
                lightOn("Light_TR");
        }

        //Bas Droit
        else if (lightName == "Light_BR")
        {
            //Change Bas Droit
            if (checkOn("Light_BL"))
                lightOff("Light_BL");
            else
                lightOn("Light_BL");

        }
        //Bas Gauche
        else if (lightName == "Light_BL")
        {
            //Change Bas Droit
            if (checkOn("Light_BR"))
                lightOff("Light_BR");
            else
                lightOn("Light_BR");
			
            //Change Bas Centre
            if (checkOn("Light_BC"))
                lightOff("Light_BC");
            else
                lightOn("Light_BC");

        }
        //Bas Centre
        else if (lightName == "Light_BC")
        {

            //Change Bas Gauche
            if (checkOn("Light_BL"))
                lightOff("Light_BL");
            else
                lightOn("Light_BL");

            //Change Haut Gauche
            if (checkOn("Light_TL"))
                lightOff("Light_TL");
            else
                lightOn("Light_TL");
        }
	}

	void scrambleLights(GameObject [] lights) {
		for (int i = 0; i < lights.Length; i++) {
			switchOnOff (lights [(int)Random.Range (0, lights.Length)].name);
		}
	}

	bool checkAllOn(GameObject [] lights) {
		
		for (int i = 0; i < lights.Length; i++) {
			if (!checkOn (lights [i].name))
				return false;
		}

		return true;
	}

	void setPosLights(GameObject[] lights, ArrayList pos) {
		int indice;
		for (int i = 0; i < lights.Length; i++) {
			indice = Random.Range (0, pos.Count);
			Vector3 v = (Vector3) pos [indice];
			lights [i].transform.SetPositionAndRotation (v, Quaternion.identity);
			pos.RemoveAt (indice);
		}
	}
}
