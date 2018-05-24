using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Random=UnityEngine.Random;


public class HubManager : MonoBehaviour {
    
	/**
	 * Phase du hub:
	 * 	waitPlayer, la table attend qu'au moins un joueur soit prêt à jouer
	 * 	animationRandom, La table lance l'animation du choix du jeu et choisi un jeu en fonction du nombre de joueur
	 * 	gameLaunched, lance le jeu sélectionné à la phase précédente
	 **/
	enum HubPhase {godMod, waitPlayer, animationRandom, gameLaunched}

    public GameObject hub;
    public GameObject iconGodMod;
    public GameObject back;
    public GameObject next;
    public Text textLeft;
    public Text TextRight;
	public float timeLeft;
	public Text nbRdy;
	public Games[] AllGames;
	public SwapIcon prefabIcon;
    public Button[] buttons;
    public GameObject blur;

    HubPhase phase;
    Games game = new Games();
    private SwapIcon icon;
    private int currentGameIndex = 0;
    private int joueurPresent;

    private String gameLaunched;

    private void Start()
    {
		phase = HubPhase.waitPlayer;
        GestionScenes.initTab();
    }

    void Update()
    {

        joueurPresent = int.Parse(nbRdy.text);
        GestionScenes.clavierUtils();
        if (Input.GetKeyDown("g"))
        {
            if (phase != HubPhase.godMod)
            {
                phase = HubPhase.godMod;
                iconGodMod.GetComponent<SpriteRenderer>().sprite = ((Games)AllGames[currentGameIndex % AllGames.Length]).sprite;
            }
            else
            {
                GestionScenes.relanceScene();
            }
        }
        if (phase == HubPhase.godMod)
        {
            disableHub();
            godMod();
        }
        else
        {
            visibleHub();
        }


        if (icon == null)
        {
            switch (phase)
            {
                case HubPhase.waitPlayer:
                    waitPlayer();
                    break;

                case HubPhase.animationRandom:
                    waitAnimationRandom();
                    break;

                case HubPhase.gameLaunched:
                    GestionScenes.setNbJoueur(joueurPresent);
                    LaunchGame(game.nomExe);
                    break;

                default:
                    break;

            }
        }
    }

    //lance le jeu game
	void LaunchGame(string game) {
        gameLaunched = game;
        StartCoroutine("Lancement");
	}


    //attente de la fin de l'animation d'icon pour lancer le jeu
    IEnumerator waitAnimation()
    {
        yield return new WaitUntil(() => icon == null);
    }

    //Délai où les joueurs peuvent venir sur la table
    void waitPlayer()
    {
        if (timeLeft <= 0.0f)
        {
            phase = HubPhase.animationRandom;
            timeLeft = 10;
        }
        else
        {
            timeLeft = timeLeft - Time.deltaTime;
            int tmpTimeLeft = (int)timeLeft;
            textLeft.text = tmpTimeLeft.ToString();
            TextRight.text = tmpTimeLeft.ToString();
        }
    }

    //Si le nombre de joueur n'est pas à 0, lance une animation pour choisir un jeux aléatoire, sinon repars en attente de joueur
    void waitAnimationRandom()
    {
        if (joueurPresent > 0)
        {
            desactiveButton();
            blur.SetActive(true);
            ArrayList jeuXJoueur = game.tableauDeJeuxXJoueur(AllGames, joueurPresent);
            game = (Games)jeuXJoueur[Random.Range(0, jeuXJoueur.Count)];
            icon = Instantiate(prefabIcon) as SwapIcon;
            icon.jeux = jeuXJoueur;
            icon.FinalGames = game;
            StartCoroutine(waitAnimation());
            phase = HubPhase.gameLaunched;
        }
        else
        {
            phase = HubPhase.waitPlayer;
            GestionScenes.initTab();
            timeLeft = 10;
        }
    }

    void godMod()
    {
         if (Tactil.touchCollider(next.GetComponent<BoxCollider2D>())  || Souris.collide(next.GetComponent<BoxCollider2D>()))
         {
             iconGodMod.GetComponent<SpriteRenderer>().sprite = ((Games)AllGames[++currentGameIndex % AllGames.Length]).sprite;
         }
         if (Tactil.touchCollider(back.GetComponent<BoxCollider2D>()) || Souris.collide(back.GetComponent<BoxCollider2D>()))
        {
            if (currentGameIndex == 0) currentGameIndex = AllGames.Length; // permet d'éviter un null pointeur index array
            iconGodMod.GetComponent<SpriteRenderer>().sprite = ((Games)AllGames[--currentGameIndex % AllGames.Length]).sprite;
         }
         if (Tactil.touchCollider(iconGodMod.GetComponent<BoxCollider2D>()) || Souris.collide(iconGodMod.GetComponent<BoxCollider2D>()))
         {
            LaunchGame(((Games)AllGames[currentGameIndex % AllGames.Length]).nomExe);
        }
     }

     void disableHub()
     {
         iconGodMod.SetActive(true);
         hub.SetActive(false);
     }

     void visibleHub()
     {
         iconGodMod.SetActive(false);
         hub.SetActive(true);
     }

    private void desactiveButton()
    {
        foreach(Button bouton in buttons)
        {
            bouton.interactable = false;
        }
    }

    IEnumerator Lancement()
    {
        float fadingTime = GameObject.Find("Main Camera").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadingTime);
        SceneManager.LoadScene(gameLaunched);
    }
}
