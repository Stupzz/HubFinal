using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonPosition : MonoBehaviour {

    private int[] tabJoueur;

    public void Joueur(int ind)
    {
        tabJoueur = GestionScenes.arrayJoueur();
        if (tabJoueur[ind] == 0) 
            GestionScenes.ajoutTabJoueur(ind);
        else GestionScenes.retireTabJoueur(ind);
    }
}
