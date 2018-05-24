using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameControllerLights : MonoBehaviour {

	public Text textRight;
	public Text textLeft;
	public Sprite light_On;
	public Sprite light_Off;
	public Sprite backgroundOn;
	public Sprite backgroundOff;
	public Text resultatTop;
	public Text resultatBot;
	public Text timeTop;
	public Text timeBot;


	private float timerObjectif;
	private float timer;
	private GameObject[] lights;
	private ArrayList pos = new ArrayList();

	// Use this for initialization
	void Start () {
		enabled = false;
		timer = 120f;
		pos.Add (new Vector3 (-6, 2, 0));
		pos.Add (new Vector3 (0, 2, 0));
		pos.Add (new Vector3 (6, 2, 0));
		pos.Add (new Vector3 (-6, -2, 0));
		pos.Add (new Vector3 (0, -2, 0));
		pos.Add (new Vector3 (6, -2, 0));

		// Création du tableau de lumières et on le remplie avec les lumières
		lights = GameObject.FindGameObjectsWithTag ("Light");

		// On place les lumières
		setPosLights (lights, pos);

		StartCoroutine ("showObjectif");
	}
	
	// Update is called once per frame
	void Update () {
        GestionScenes.clavierUtils();

		//if(Input.touchCount > 0) {
			//for (int i = 0; i < Input.touchCount; i++) {
		if (!checkAllOn (lights) && timer > 0) {
			if ((Input.GetMouseButtonDown (0))) {
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
			countTimer ();
		} else {
			if (checkAllOn (lights)) {
				GameObject.Find ("Background").GetComponent<SpriteRenderer> ().sprite = backgroundOn;
                StartCoroutine("Win");
			} else {
                lose (lights);
			}
				
		}
    }

	bool checkOn (string lightName) {
		if (GameObject.Find(lightName).GetComponent<SpriteRenderer>().sprite == light_On)
			return true;
		else
			return false;
	}

	void lightOn (string lightName) {
		GameObject.Find (lightName).GetComponent<SpriteRenderer> ().sprite = light_On;
	}

	void lightOff (string lightName) {
		GameObject.Find (lightName).GetComponent<SpriteRenderer> ().sprite = light_Off;
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

	void countTimer() {
		timer = timer - Time.deltaTime;
		int tmpTimeLeft = (int)timer;
		textLeft.text = tmpTimeLeft.ToString();
		textRight.text = tmpTimeLeft.ToString();
	}

	void win(GameObject [] lights) {
		for (int i = 0; i < lights.Length; i++) {
			Destroy (lights [i]);
		}
		int tmp = (int)(120f - timer);
		textRight.text = "";
		textLeft.text = "";
		resultatBot.text = "BIEN JOUÉ !";
		resultatTop.text = "BIEN JOUÉ !";
		timeTop.text = "Fait en : "+ tmp.ToString()+ " secondes";
		timeBot.text = "Fait en : "+ tmp.ToString()+ " secondes";
		enabled = false;
        StartCoroutine("FinDePartie");
	}

	void lose(GameObject [] lights) {
		for (int i = 0; i < lights.Length; i++) {
			Destroy (lights [i]);
		}
		int tmp = (int)(120f - timer);
		textRight.text = "";
		textLeft.text = "";
		resultatBot.text = "PERDU !";
		resultatTop.text = "PERDU !";
		enabled = false;
        StartCoroutine("FinDePartie");
    }

	IEnumerator showObjectif() {
		// On allume toutes les lumières
		for (int i = 0; i < lights.Length; i++) {
			lightOn (lights [i].name);
		}
		GameObject.Find ("Background").GetComponent<SpriteRenderer> ().sprite = backgroundOn;

		yield return new WaitForSeconds (2f);

		GameObject.Find ("Background").GetComponent<SpriteRenderer> ().sprite = backgroundOff;
		//On mélange les lumières
		while (checkAllOn (lights)) {
			for (int i = 0; i < lights.Length; i++) {
				switchOnOff (lights [(int)Random.Range (0, lights.Length)].name);
				yield return new WaitForSeconds (0.1f);
			}
		}
		resultatBot.text = "3";
		resultatTop.text = "3";
		yield return new WaitForSeconds (1f);

		resultatBot.text = "2";
		resultatTop.text = "2";
		yield return new WaitForSeconds (1f);

		resultatBot.text = "1";
		resultatTop.text = "1";
		yield return new WaitForSeconds (1f);

		resultatBot.text = "Allumez toutes les lumières !";
		resultatTop.text = "Allumez toutes les lumières !";
		yield return new WaitForSeconds (1f);

		resultatBot.text = "";
		resultatTop.text = "";

		enabled = true;
	}

    IEnumerator FinDePartie()
    {
        yield return new WaitForSeconds(3);
        float fadingTime = GameObject.Find("Main Camera").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadingTime);
        GestionScenes.RetourHub();
    }

    IEnumerator Win()
    {
        yield return new WaitForSeconds(1);
        win(lights);
    }
}
