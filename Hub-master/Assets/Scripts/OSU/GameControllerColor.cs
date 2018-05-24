using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerColor : MonoBehaviour {

	public int nbBall;
	public Transform prefab;
	public Transform prefabExemple;
	public float xMax;
	public float xMin;
	public float yMax;
	public float yMin;
	public Material black;
	public Material white;
	public Material red;
	public Material blue;

	private float radius;
	private int compteur = 1; //Servira pour connaitre l'avancement du jeu, si compteur > nbBall, la partie est gagné.
	private ArrayList myBalls = new ArrayList();
	private ArrayList myBallsExemple = new ArrayList();
	private ArrayList myMaterialsTab = new ArrayList();
	private int compteurObjetCourrant;

	// Use this for initialization
	void Start () {
		radius = prefab.gameObject.GetComponent<Renderer> ().bounds.size.x;
		PopObject ();
		compteurObjetCourrant = nbBall;
	}

	// Update is called once per frame
	/**
	 * Met à jour le jeu à chaque `frame` en regardant si le joueur a touché une boule du jeu;
	 * si oui:  on vérifie si le game object est le bon, puis le sort du plateau
	 * 			si le game object n'est pas le bon: ré-affiche tous les game objects, puis le joueur recommence la partie en changeant de 'mode'
	 * si non: on ne fait rien
	 * 
	 * Si le nombre de boules est égal à 0, alors la partie se termine
	 * */
	void Update () {

		if (compteurObjetCourrant == 0) {
			endTheGame ();
		} else {
			Touch[] myTouches = Input.touches;
			for (int i = 0; i < Input.touchCount; i++) {
				Touch myTouch = Input.GetTouch (i);
				Vector2 touchPos = (Vector2)Camera.main.ScreenToWorldPoint (myTouch.position); // position du touch
				RaycastHit2D hit = Physics2D.Raycast (touchPos, -Vector2.up);

				if (hit.collider != null) {
					switch (hit.collider.tag) {
					case "Cible":
						switch (myTouch.phase) {
							
						case TouchPhase.Began:
							Transform test = Instantiate (prefabExemple, new Vector2 (0, 0), Quaternion.identity); //création d'un gameObject, car nous ne pouvons pas comparais une instance de material avec une matérial non instancier (list de material)
							test.gameObject.GetComponent<Renderer> ().material = (Material)myMaterialsTab [compteur - 1];
							string nameTest = test.gameObject.GetComponent<Renderer> ().material.name;
							string gameObjectTouchName = hit.collider.gameObject.GetComponent<Renderer> ().material.name;
							Destroy (test.gameObject);

							if (gameObjectTouchName == nameTest) { //test si c'est bien la bonne couleur qui est hit 
								GameObject ball = hit.collider.gameObject;
								ball.SetActive (false);
								GameObject ballEx = (GameObject)myBallsExemple [compteur - 1];
								ballEx.SetActive (false);
								compteur++;
								compteurObjetCourrant--;
							} else {
								relanceJeu ();
							}
							break;

						default:
							break;
						}
						break;

					default:
						break;
					}
				}
			}
		}
		/*else if (Input.GetMouseButtonDown (0)) {
			Vector2 touchPos = (Vector2)Camera.main.ScreenToWorldPoint (Input.mousePosition); // position du touch
			RaycastHit2D hit = Physics2D.Raycast (touchPos, -Vector2.up);
			if (hit.collider != null) {
				switch (hit.collider.tag) {
				case "Cible":
					Transform test = Instantiate (prefabExemple, new Vector2 (0, 0), Quaternion.identity); //création d'un gameObject, car nous ne pouvons pas comparais une instance de material avec une matérial non instancier (list de material)
					test.gameObject.GetComponent<Renderer> ().material = (Material)myMaterialsTab [compteur - 1];
					string nameTest = test.gameObject.GetComponent<Renderer> ().material.name;
					string gameObjectTouchName = hit.collider.gameObject.GetComponent<Renderer> ().material.name;
					Destroy (test.gameObject);

					if (gameObjectTouchName == nameTest) { //test si c'est bien la bonne couleur qui est hit 
						GameObject ball = hit.collider.gameObject;
						ball.SetActive (false);
						GameObject ballEx = (GameObject)myBallsExemple [compteur - 1];
						ballEx.SetActive (false);
						compteur++;
						compteurObjetCourrant--;
					} else {
						relanceJeu ();
					}
					break;
				default:
					break;
				}
			}
		}*/
	}

	private Vector3 randomPosition()
	{
		float x = Random.Range(xMin, xMax);
		float y = Random.Range(yMin, yMax);
		return new Vector3(x, y, 0f);
	}

	void PopObject(){
		float x = 1f;

		for (int i = 0; i < nbBall; i++) { // crée les différentes matérials utilisable
			myMaterialsTab.Add(randomMaterial ()); // choix aléatoire du material
			//Debug.Log(myMaterialsTab[i]);
		}



		for (int i = 0; i < nbBall; i++) { // pop des cibles
			Renderer rend;
			Material myMaterial = (Material) myMaterialsTab [i]; // choix du material

			Vector3 spownPosition = new Vector2 (-1 * ((float)nbBall / 2) + x * i, 0.0f); // représentation des cibles pour l'utilisateur. Le vecteur prend en compte le nombre de ball en jeu pour centré sont affichage, d'où le -1*ball/2
			Transform clone = Instantiate (prefabExemple, spownPosition, Quaternion.identity);
			myBallsExemple.Add (clone.gameObject);
			rend = clone.gameObject.GetComponent<Renderer> ();
			rend.material = myMaterial; 
		}
		for (int i = 0; i < nbBall; i++) { // pop des cibles
			Renderer rend;
			Material myMaterial = (Material) myMaterialsTab[i]; // choix du material en fonction du jeu en cours

			Vector3 spownPosition;
			do{
				spownPosition = randomPosition (); //Les cibles
				//Debug.Log("ici");
			} while (Physics2D.OverlapCircle(spownPosition, radius) != null); // permet déviter le chevauchement de deux clible.

			Transform clone = Instantiate (prefab, spownPosition, Quaternion.identity);
			myBalls.Add (clone.gameObject);
			rend = clone.gameObject.GetComponent<Renderer> ();
			rend.material = myMaterial;
		}
	}

	void relanceJeu(){
		compteurObjetCourrant = nbBall;
		compteur = 1;
		foreach (GameObject ball in myBalls) {
			ball.SetActive(true);
		}
		foreach (GameObject ballEx in myBallsExemple		) {
			ballEx.SetActive(true);
		}
	}

	private Material randomMaterial(){

		Material materialReturned;

		int choix = (int) Random.Range(1.0f, 5f);
		switch (choix)
		{
		case 1:
			materialReturned = black;
			break;
		case 2:
			materialReturned = white;
			break;
		case 3:
			materialReturned = red;
			break;
		default:
			materialReturned = blue;
			break;
		}
		return materialReturned;
	}
		

	void endTheGame(){
		foreach (GameObject ball in myBalls) {
			Destroy (ball);
		}
		foreach (GameObject ballEx in myBallsExemple) {
			Destroy (ballEx);
		}
	}
}
