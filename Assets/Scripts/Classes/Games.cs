using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Games : MonoBehaviour
{
    public string nomExe;
    public Sprite sprite;
    public int[] joueurPossible;

    public Games()
    {
        nomExe = null;
        sprite = null;
        joueurPossible = null;
    }

    public Games(string name, Sprite sprite, int[] joueurPossible)
    {
        this.nomExe = name;
        this.sprite = sprite;
        this.joueurPossible = joueurPossible;
    }

    // Regarde si un nombre de joueur correspond bien à un nombre de joueur de ce jeux
    public bool jouableA(int nbJoueur)
    {
        foreach(int nbJoueurPossible in joueurPossible)
        {
            if (nbJoueurPossible == nbJoueur)
            {
                return true;
            }
        }
        return false;
    }

	public ArrayList tableauDeJeuxXJoueur(Games[] tableauDeJeu, int nbJoueur){
		ArrayList gamesReturned = new ArrayList ();
		foreach (Games jeu in tableauDeJeu) {
			if (jeu.jouableA (nbJoueur))
				gamesReturned.Add (jeu);
		}
		return gamesReturned;
	}
}


