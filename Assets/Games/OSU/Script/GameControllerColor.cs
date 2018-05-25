using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerColor : MonoBehaviour {

	public Transform prefab;
	public Transform prefabExemple;
	public Text textLeft;
	public Text textRight;
	public Text resultTop;
	public Text resultBot;
    public GameObject blur;

    private int compteurSequence;
	private float timer;
    private int nbBall;
    private float xMax = 15.5f;
    private float xMin = -15.5f;
    private float yMax = 8f;
    private float yMin = -8f;
    private float radius;
	private int compteur = 1; //Servira pour connaitre l'avancement du jeu, si compteur > nbBall, la partie est gagné.
	private ArrayList myBalls = new ArrayList();
    private List<GameObject> myBallsExemple = new List<GameObject>();
	private int compteurObjetCourrant;

	// Use this for initialization
	void Start () {
		enabled = false;
		StartCoroutine ("showObjectif");
        compteurSequence = 0;
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

        countTimer();
        string input = Input.inputString;
        switch (input)
        {
            case "v":
                GestionScenes.lance("Osu");
                break;
            default:
                break;
        }
        GestionScenes.clavierUtils();

        if (compteurObjetCourrant == 0)
        {
            compteurObjetCourrant = nbBall;
            compteur = 1;
            PopObject();
            compteurSequence++;
        }
        else if (timer <= 0)
        {
            endTheGame();
        }
        else
        {
            Touch[] myTouches = Input.touches;
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch myTouch = Input.GetTouch(i);
                Vector2 touchPos = (Vector2)Camera.main.ScreenToWorldPoint(myTouch.position); // position du touch
                RaycastHit2D hit = Physics2D.Raycast(touchPos, -Vector2.zero);

                if (hit.collider != null)
                {
                    switch (hit.collider.tag)
                    {
                        case "Cible":
                            switch (myTouch.phase)
                            {

                                case TouchPhase.Began:
                                    if (hit.collider.gameObject.GetComponent<SpriteRenderer>().color.Equals(myBallsExemple[compteur - 1].GetComponent<SpriteRenderer>().color)) //test si c'est bien la bonne couleur qui est hit 
                                    {
                                        GameObject ball = hit.collider.gameObject;
                                        ball.SetActive(false);
                                        GameObject ballEx = (GameObject)myBallsExemple[compteur - 1];
                                        ballEx.SetActive(false);
                                        compteur++;
                                        compteurObjetCourrant--;
                                    }
                                    else
                                    {
                                        relanceJeu();
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
	}

	private Vector3 randomPosition()
	{
		float x = Random.Range(xMin, xMax);
		float y = Random.Range(yMin, yMax);
		return new Vector3(x, y, 0f);
	}

	void PopObject(){
		float x = 1f;
        Transform clone;

        clearJeu();

        for (int i = 0; i < nbBall; i++)
        { // pop des cibles
            SpriteRenderer rend;
            Color myColor = randomColor(); // choix aléatoire du material

            Vector3 spownPosition = new Vector2(-1 * ((float)nbBall / 2) + x * i, 0.0f); // représentation des cibles pour l'utilisateur. Le vecteur prend en compte le nombre de ball en jeu pour centré sont affichage, d'où le -1*ball/2
            clone = Instantiate(prefabExemple, spownPosition, Quaternion.identity);
            myBallsExemple.Add(clone.gameObject);
            rend = clone.gameObject.GetComponent<SpriteRenderer>();
            rend.color = myColor;
        }
        for (int i = 0; i < nbBall; i++)
        { // pop des cibles
            SpriteRenderer rend;
            GameObject ballEx = (GameObject)myBallsExemple[i];
            Color myColor = ballEx.GetComponent<SpriteRenderer>().color; // choix du material en fonction du jeu en cours

            Vector3 spownPosition;
            do
            {
                spownPosition = randomPosition(); //Les cibles
            } while (Physics2D.OverlapCircle(spownPosition, radius) != null); // permet déviter le chevauchement de deux clible.

            clone = Instantiate(prefab, spownPosition, Quaternion.identity);
            myBalls.Add(clone.gameObject);
            rend = clone.gameObject.GetComponent<SpriteRenderer>();
            rend.color = myColor;
        }
	}

	void relanceJeu(){
		compteurObjetCourrant = nbBall;
		compteur = 1;
		foreach (GameObject ball in myBalls) {
			ball.SetActive(true);
		}
		foreach (GameObject ballEx in myBallsExemple) {
			ballEx.SetActive(true);
		}
	}

    private Color randomColor()
    {

        int choix = (int)Random.Range(1, 5);
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


    void endTheGame(){
		foreach (GameObject ball in myBalls) {
			Destroy (ball);
		}
		foreach (GameObject ballEx in myBallsExemple) {
			Destroy (ballEx);
		}

        if (compteurSequence > 0)
        {
            resultBot.text = "Nombre de séquences effectuées : " + compteurSequence;
        }
        else
        {
            resultBot.text = "Vous n'avez effectué aucune séquence!";
        }
		resultTop.text = resultBot.text;
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

		resultBot.text = "Suivre les séquences de couleur!";
		resultTop.text = "Suivre les séquences de couleur!";
		yield return new WaitForSeconds (1.5f);

		resultBot.text = "";
		resultTop.text = "";

		radius = prefab.gameObject.GetComponent<Renderer> ().bounds.size.x;
		nbBall = GestionScenes.getNbJoueur() * 4;
		if (nbBall <= 4) nbBall = 6;
		PopObject ();
		compteurObjetCourrant = nbBall;
		timer = 120f;

		enabled = true;
	}

    private void clearJeu()
    {
        foreach(GameObject ball in myBalls)
        {
            Destroy(ball);
        }

        foreach (GameObject ball in myBallsExemple)
        {
            Destroy(ball);
        }
        myBalls.Clear();
        myBallsExemple.Clear();
    }
}