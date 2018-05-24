using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gestionJoueur : MonoBehaviour {


    public Text myText;
	// Use this for initialization
	void Start () {
        myText.text = "1";
        GestionScenes.setNbJoueur(1);
	}
	
	// Update is called once per frame
	void Update () {
        string nbJoueur = Input.inputString;
        switch (nbJoueur)
        {
            case "1":
                myText.text = "1";
                GestionScenes.setNbJoueur(1);
                break;
            case "2":
                myText.text = "2";
                GestionScenes.setNbJoueur(2);
                break;
            case "3":
                myText.text = "3";
                GestionScenes.setNbJoueur(3);
                break;
            case "4":
                myText.text = "4";
                GestionScenes.setNbJoueur(4);
                break;
            case "5":
                myText.text = "5";
                GestionScenes.setNbJoueur(5);
                break;
            case "6":
                myText.text = "6";
                GestionScenes.setNbJoueur(6);
                break;
            default:
                break;
        }
	}
}
