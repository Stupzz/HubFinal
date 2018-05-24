using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Random=UnityEngine.Random;


public class timer : MonoBehaviour {
    
	/**
	 * Phase du hub:
	 * 	waitPlayer, la table attend qu'au moins un joueur soit prêt à jouer
	 * 	animationRandom, La table lance l'animation du choix du jeu et choisi un jeu en fonction du nombre de joueur
	 * 	gameLaunched, lance le jeu sélectionné à la phase précédente
	 **/
	enum HubPhase {waitPlayer, animationRandom, gameLaunched}

    public GameObject hub;
    public Text TextObject;
    public Text TextRight;
	public float timeLeft;
	public Text nbRdy;
	public Games[] AllGames;
	public SwapIcon prefabIcon;

    HubPhase phase;
    Games game = new Games();
	private bool animationEnded = false;
    private SwapIcon icon;
    private int joueurPresent;

    private void Start()
    {
		phase = HubPhase.waitPlayer;
    }

    void Update()
    {
        joueurPresent = int.Parse(nbRdy.text);


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
                    LaunchGame(game.nomExe);
                    break;

                default:
                    break;

            }
        }
    }

    //lance le jeu game
	void LaunchGame(string game) {
        SceneManager.LoadScene(game);
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
            TextObject.text = tmpTimeLeft.ToString();
            TextRight.text = tmpTimeLeft.ToString();
        }
    }

    //Si le nombre de joueur n'est pas à 0, lance une animation pour choisir un jeux aléatoire, sinon repars en attente de joueur
    void waitAnimationRandom()
    {
        if (joueurPresent > 0)
        {
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
            timeLeft = 10;
        }
    }
}
