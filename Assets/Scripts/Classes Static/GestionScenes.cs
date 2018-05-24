using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GestionScenes {

    private static int nbJoueur = 0;
    private static int[] tabJoueur;


    //Gestion scene
    public static void RetourHub()
    {
        SceneManager.LoadScene("_HUB");
    }

    public static void lance(string scene)
    {
        SceneManager.LoadScene(scene);

    }
    public static void relanceScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    //Gestion du joueur 
    public static int getNbJoueur()
    {
        return nbJoueur;
    }
    public static void setNbJoueur(int value)
    {
        nbJoueur = value;
    }

    //gestion tableau joueur

    public static void initTab()
    {
        tabJoueur = new int[] { 0, 0, 0, 0, 0 ,0 };
    }

    public static void ajoutTabJoueur(int index)
    {
        tabJoueur[index] = 1;
    }

    public static void retireTabJoueur(int index)
    {
        tabJoueur[index] = 0;
    }

    public static int[] arrayJoueur()
    {
        return tabJoueur;
    }
    
    //Gestion touche Clavier

    public static void clavierUtils()
    {
        string input = Input.inputString;
        switch (input)
        {
            case "h":
                GestionScenes.RetourHub();
                break;
            case "q":
                Application.Quit();
                break;
            case "r":
                GestionScenes.relanceScene();
                break;
            default:
                break;
        }
    }
}
