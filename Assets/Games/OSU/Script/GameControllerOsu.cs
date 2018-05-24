using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerOsu : MonoBehaviour {

	private bool revMode = false;
	public Transform prefab;
	public Transform prefabExemple;
	public Transform arrowLeftPrefab;
    public Transform arrowRightPrefab;
	public Text textLeft;
	public Text textRight;
	public Text resultTop;
	public Text resultBot;
    public GameObject blur;

	private float timer;
	private int lvl = 0;
    private int nbBall;
    private float xMax = 15.5f;
    private float xMin = -15.5f;
    private float yMax = 8f;
    private float yMin = -8f;
    private GameObject arrowLeft;
    private GameObject arrowRight;
	private float radius;
	private int compteur = 1; //Servira pour connaitre l'avancement du jeu, si compteur > nbBall, la partie est gagné.
	private ArrayList myBalls = new ArrayList();
	private ArrayList myBallsExemple = new ArrayList();
	private int compteurObjetCourrant;

	// Use this for initialization
	void Start () {
		enabled = false;
		StartCoroutine ("showObjectif");
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
        string input = Input.inputString;
        switch (input)
        {
            case "v":
                GestionScenes.lance("OsuColor");
                break;
            default:
                break;
        }
        GestionScenes.clavierUtils();
		if (compteurObjetCourrant == 0 || timer <= 0) {
			endTheGame ();
		} else {
			Touch[] myTouches = Input.touches;
			for (int i = 0; i < Input.touchCount; i++) {
				Touch myTouch = Input.GetTouch (i);
				Vector2 touchPos = (Vector2)Camera.main.ScreenToWorldPoint (myTouch.position); // position du touch
				RaycastHit2D hit = Physics2D.Raycast (touchPos, -Vector2.zero);

				if (hit.collider != null) {
					switch (hit.collider.tag) {
					case "Cible":
						switch (myTouch.phase) {

						case TouchPhase.Began:
							if (revMode) {

								if (hit.collider.gameObject.Equals (myBalls [compteur - 1 + (nbBall - 1)])) { //index de la liste commence à 0 mais c'est bel et bien le premier, nbBall - 1 pour partir du dernier objet de la liste
									GameObject ball = (GameObject)myBalls [compteur - 1 + (nbBall - 1)];
									ball.SetActive (false);
									GameObject ballEx = (GameObject)myBallsExemple [compteur - 1 + (nbBall - 1)];
									ballEx.SetActive (false);
									compteur--;
									compteurObjetCourrant--;
								} else {
									relanceJeu ();
								}
							}
                            else {
								if (hit.collider.gameObject.Equals (myBalls [compteur - 1])) { //index de la liste commence à 0 mais c'est bel et bien le premier
									GameObject ball = (GameObject)myBalls [compteur - 1];
									ball.SetActive (false);
									GameObject ballEx = (GameObject)myBallsExemple [compteur - 1];
									ballEx.SetActive (false);
									compteur++;
									compteurObjetCourrant--;
								} else {
									relanceJeu ();
								}
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
			countTimer ();
		}
	}

	private Vector3 randomPosition()
	{
		float x = Random.Range(xMin, xMax);
		float y = Random.Range(yMin, yMax);
		return new Vector3(x, y, 0f);
	}

	void PopObject(){
		float x = 1f;
        Transform clone = Instantiate(arrowLeftPrefab, new Vector2(-1 + -1 * ((float)nbBall / 2), 0), Quaternion.identity);
        arrowLeft = clone.gameObject;
        clone = Instantiate(arrowRightPrefab, new Vector2(1 - 1 * ((float)nbBall / 2) + 1f * nbBall - 1, 0), Quaternion.identity);
        arrowRight = clone.gameObject;

        arrowLeft.SetActive(true);
        arrowRight.SetActive(false);

        for (int i = 0; i < nbBall; i++) { // pop des cibles
			SpriteRenderer rend;
			Color myColor = randomColor(); // choix aléatoire du material

			Vector3 spownPosition = new Vector2 (-1 * ((float)nbBall / 2) + x * i, 0.0f); // représentation des cibles pour l'utilisateur. Le vecteur prend en compte le nombre de ball en jeu pour centré sont affichage, d'où le -1*ball/2
			clone = Instantiate (prefabExemple, spownPosition, Quaternion.identity);
			myBallsExemple.Add (clone.gameObject);
			rend = clone.gameObject.GetComponent<SpriteRenderer> ();
			rend.color = myColor; 
		}
		for (int i = 0; i < nbBall; i++) { // pop des cibles
            SpriteRenderer rend;
			GameObject ballEx = (GameObject)myBallsExemple [i];
            Color myColor = ballEx.GetComponent<SpriteRenderer> ().color; // choix du material en fonction du jeu en cours

			Vector3 spownPosition;
			do{
				spownPosition = randomPosition (); //Les cibles
				//Debug.Log("ici");
			} while (Physics2D.OverlapCircle(spownPosition, radius) != null); // permet déviter le chevauchement de deux clible.

			clone = Instantiate (prefab, spownPosition, Quaternion.identity);
			myBalls.Add (clone.gameObject);
			rend = clone.gameObject.GetComponent<SpriteRenderer> ();
			rend.color = myColor;
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
		inverseMode ();
		majPositionArrow ();
	}

	void inverseMode(){
		if (revMode)
			revMode = false;
		else
			revMode = true;
	}

	private Color randomColor(){

		int choix = (int) Random.Range(1, 5);
		switch (choix)
		{
		case 1:
                return Color.black;
		case 2:
                return Color.white;
		case 3:
                return Color.red;
		default:
                return Color.blue;
		}
	}

	void majPositionArrow(){
		
		if (revMode) {
			arrowLeft.SetActive (false);
			arrowRight.SetActive (true);
		} else {
			arrowLeft.SetActive (true);
			arrowRight.SetActive (false);
		}
	}

	void endTheGame(){
		//lvl++;
		//initGame ();
		Destroy (arrowLeft);
		Destroy (arrowRight);
		foreach (GameObject ball in myBalls) {
			Destroy (ball);
		}
		foreach (GameObject ballEx in myBallsExemple) {
			Destroy (ballEx);
		}
		resultBot.text = "GAME OVER !";
		resultTop.text = "GAME OVER !";
		textLeft.text = "";
		textRight.text = "";
        StartCoroutine(FinParti());
	}

    IEnumerator FinParti()
    {
        blur.SetActive(true);
        yield return new WaitForSeconds(3);
        float fadingTime = GameObject.Find("Main Camera").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadingTime);
        GestionScenes.RetourHub();
    }

    private void initGame(){
		radius = prefab.gameObject.GetComponent<Renderer> ().bounds.size.x;
		//GestionScenes.setNbJoueur(6);
		myBalls.Clear();
		myBallsExemple.Clear();
		compteur = 1;
		nbBall = GestionScenes.getNbJoueur() * (4 + lvl);
		if (nbBall <= 4) nbBall = 6;
		PopObject ();
		compteurObjetCourrant = nbBall;
	}

	void countTimer() {
		timer = timer - Time.deltaTime;
		int tmpTimeLeft = (int)timer;
		textLeft.text = tmpTimeLeft.ToString();
		textRight.text = tmpTimeLeft.ToString();
	}

	IEnumerator showObjectif() {
		resultBot.text = "3";
		resultTop.text = "3";
		yield return new WaitForSeconds (1f);

		resultBot.text = "2";
		resultTop.text = "2";
		yield return new WaitForSeconds (1f);

		resultBot.text = "1";
		resultTop.text = "1";
		yield return new WaitForSeconds (1f);

		resultBot.text = "Trouvez les bonnes boules !";
		resultTop.text = "Trouvez les bonnes boules !";
		yield return new WaitForSeconds (1.5f);

		resultBot.text = "";
		resultTop.text = "";

		initGame ();
		timer = 120;

		enabled = true;
	}
}
