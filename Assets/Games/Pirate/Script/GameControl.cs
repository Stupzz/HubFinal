using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {

    public Canon[] mesCanons;
    public GameObject[] listBoat;
    public GameObject background;
    public GameObject[] listButton;
    public GameObject princesse;
    public GameObject pirate;
    public float timer = 120;
    public Text timerText;

    public Text botText;
    public Text TopText;

    public GameObject blur;


    private List<Canon> canonActif;
    private bool AllDamaged;
    private bool canonDetruit = false;
    private bool gagne;

    void Start () {
        timerText.text = Mathf.RoundToInt(timer).ToString();
        canonActif = new List<Canon>();
        /*GestionScenes.initTab();
        GestionScenes.ajoutTabJoueur(1);
        GestionScenes.ajoutTabJoueur(4); */// a décommenter pour lancer sans le hub
        int[] joueur = GestionScenes.arrayJoueur();
        for (int i = 0; i < joueur.Length; i++)
        {
            if (joueur[i] == 1)
            {
                mesCanons[i].lance();
                canonActif.Add(mesCanons[i]);
            }
        }

        foreach (GameObject gameObject in listBoat)
        {
            gameObject.GetComponent<Pirate>().initRgbd();
        }
        gestionPause(false);
        StartCoroutine("Lancement");
        StartCoroutine("Pause");

    }

    // Update is called once per frame

    void Update()
    {
        GestionScenes.clavierUtils();

        gestionCanon(canonActif);
        gestionBoat();

        timer -= Time.deltaTime;
        timerText.text = Mathf.RoundToInt(timer).ToString();

        if (timer <= 0)
        {
            sceneFin("Gagné");
        }
    }







    void afficheCanon(Canon[] player)
    {
        foreach(Canon canon in player)
        {
            canon.lance();
        }
    }

    private void gestionCanon(List<Canon> listC)
    {
        bool allDamaged = true;
        foreach(Canon canon in listC)
        {
            if (!canon.isDamaged())
            {
                allDamaged = false;
                break;
            }
        }
        if (allDamaged)
        {
            canonDetruit = true;
            sceneFin("Perdu");
        }
    }

    private void sceneFin(string etat)
    {
        gestionPause(false);

        switch (etat)
        {
            case "Perdu":
                gagne = false;
                break;

            case "Gagné":
                gagne = true;
                break;
            default:
                break;
        }
        StartCoroutine("FinParti");
    }

    IEnumerator Pause()
    {
        yield return new WaitForSeconds(5);
        enabled = true;
        gestionPause(true);
    }

    private void gestionPause(bool isActive)
    {
        enabled = isActive;
        foreach (GameObject gameObject in listBoat)
        {
            if (gameObject != null)
            {
                gameObject.GetComponent<Pirate>().enabled = isActive;
                if (!isActive) gameObject.GetComponent<Pirate>().resetForce();
            }
        }

        foreach (GameObject gameObject in listButton)
        {
            gameObject.GetComponent<Fire>().enabled = isActive;
        }

        background.GetComponent<Background>().setAnim(isActive);
        background.GetComponent<Background>().enabled = isActive;
        background.GetComponent<Animator>().enabled = isActive;
    }

    private void gestionBoat()
    {
        if (princesse == null) sceneFin("Perdu");
        else if (pirate == null) sceneFin("Gagné");
    }

    IEnumerator FinParti()
    {
        blur.SetActive(true);
        if (gagne) botText.text = "Vous avez defendu la princesse avec succès!";
        else if (canonDetruit) botText.text = "Perdu, vous n'avez plus de canon pour defendre la princesse!";
        else botText.text = "Perdu, vous n'avez pas réussi à défendre la princesse!";
        TopText.text = botText.text;
        yield return new WaitForSeconds(5);
        GestionScenes.RetourHub();
    }

    IEnumerator Lancement()
    {
        botText.text = "3!";
        TopText.text = botText.text;
        yield return new WaitForSeconds(1);
        botText.text = "2!";
        TopText.text = botText.text;
        yield return new WaitForSeconds(1);
        botText.text = "1!";
        TopText.text = botText.text;
        yield return new WaitForSeconds(1);
        botText.text = "Defendez la princesse!";
        TopText.text = botText.text;
        yield return new WaitForSeconds(2);
        botText.text = "";
        TopText.text = "";
    }
}
