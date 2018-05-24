using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameControllerSnake : MonoBehaviour {

    public GameObject[] cibles;
    public Vector3 spawnValues;

    

    public Text resultatsTop;
    public Text resultatsBot;

    private float delay;
    [Range(0.0f, 200.0f)]
    public float nouritureDelay;

    private bool PhaseFinal = true;

    private float radius;

    public int[] joueurs;

    private List<GameObject> MesSnakes;

    bool spawnEnable;
    // Use this for initialization
    void Start () {
        resultatsTop.enabled = false;
        resultatsBot.enabled = false;
        MesSnakes = new List<GameObject>();
       /* GestionScenes.initTab();
        GestionScenes.ajoutTabJoueur(1);
        GestionScenes.ajoutTabJoueur(4); */// a décommenter pour lancer sans le hub, avec deux joueur à la pos 1 et 4
        joueurs = GestionScenes.arrayJoueur();
        //Recuperation des joueurs
        for(int i = 0 ; i < joueurs.Length; i++ )
        {
            if (joueurs[i] == 0)
            {
                GameObject.Find("Snake" + (i + 1)).SetActive(false);
                GameObject.Find("Score Snake" + (i + 1)).SetActive(false);
                GameObject.Find("x2 Score Snake" + (i + 1)).SetActive(false);
            }
            else
            {
                GameObject.Find("Snake" + (i + 1)).SetActive(true);
                MesSnakes.Add(GameObject.Find("Snake" + (i + 1)));
            }
        }
        
        for (int i = 0; i < MesSnakes.Count; i++)
        {
            MesSnakes[i].transform.Find("SnakeHead").GetComponent<SnakeMovements>().enabled = false;
        }
        delay = 0;
        StartCoroutine(AffichageDebut());
        
    }
	
	// Update is called once per frame
	void Update () {

        if (delay > 10)
        {
            delay -= Time.deltaTime;
        }
        else if (delay > 5)
        {
            spawnEnable = false;
            delay -= Time.deltaTime;
        }
        else if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else if (delay == 0)
        {

        }
        else
        {
            if (PhaseFinal)
            {
                StartCoroutine(AffichageFin());
                PhaseFinal = false;
            }
            delay = 0;
        }

        //Presentation, help clavier
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    IEnumerator AffichageDebut()
    {
        resultatsBot.enabled = true;
        resultatsTop.enabled = true;
        resultatsBot.fontSize = 40;
        resultatsTop.fontSize = 40;


        //Décompte de départ
        resultatsBot.text = "3 !";
        resultatsTop.text = "3 !";
        yield return new WaitForSeconds(1);
        resultatsBot.text = "2 !";
        resultatsTop.text = "2 !";
        yield return new WaitForSeconds(1);
        resultatsBot.text = "1 !";
        resultatsTop.text = "1 !";
        yield return new WaitForSeconds(1);
        resultatsBot.text = "Mangez Des Pommes !";
        resultatsTop.text = "Mangez Des Pommes !";
        yield return new WaitForSeconds(1);
        resultatsBot.enabled = false;
        resultatsTop.enabled = false;

        //On active le spawn d'items
        spawnEnable = true;
        radius = cibles[0].GetComponent<Renderer>().bounds.size.x;
        StartCoroutine(SpawnNouriture());


        //on met les tetes actives
        for (int i = 0; i < MesSnakes.Count; i++)
        {
            MesSnakes[i].transform.Find("SnakeHead").GetComponent<SnakeMovements>().enabled = true;
        }

        delay = 120f;
        yield return null;
    }

    IEnumerator AffichageFin()
    {

        //On pause le jeu
        for (int i = 0; i < MesSnakes.Count; i++)
        {
            MesSnakes[i].transform.Find("SnakeHead").GetComponent<SnakeMovements>().enabled = false;
        }

        //Onfloute la scène Arrière
        GameObject.Find("BLUR").GetComponent<SpriteRenderer>().enabled = true;

        //Tri en fonction du meilleur score
        MesSnakes.Sort((a, b) => (b.transform.Find("SnakeHead").GetComponent<SnakeMovements>().Score.
               CompareTo(
               a.transform.Find("SnakeHead").GetComponent<SnakeMovements>().Score)));


        for (int i = 0; i < MesSnakes.Count; i++)
        {
            print(MesSnakes[i].transform.Find("SnakeHead").GetComponent<SnakeMovements>().Score);
            GameObject txt = GameObject.Find("Score " + MesSnakes[i].name);
            GameObject.Find("x2 Score " + MesSnakes[i].name).GetComponent<Text>().enabled = false;
            txt.GetComponent<ScoreUpdate>().enabled = false;
            if (MesSnakes[i].name == "Snake1" || MesSnakes[i].name == "Snake2" || MesSnakes[i].name == "Snake3")
            {
                txt.GetComponent<RectTransform>().position = new Vector3(txt.GetComponent<RectTransform>().position.x - 1.5f, txt.GetComponent<RectTransform>().position.y - 2, txt.GetComponent<RectTransform>().position.z);
                
            }
            else
            {
                txt.GetComponent<RectTransform>().position = new Vector3(txt.GetComponent<RectTransform>().position.x + 1.5f, txt.GetComponent<RectTransform>().position.y + 2, txt.GetComponent<RectTransform>().position.z);
                
            }

            string text = txt.GetComponent<Text>().text;
            if(i >= 1)
            {
                if (MesSnakes[i - 1].transform.Find("SnakeHead").GetComponent<SnakeMovements>().Score == MesSnakes[i].transform.Find("SnakeHead").GetComponent<SnakeMovements>().Score)
                {
                    text = GameObject.Find("Score " + MesSnakes[i - 1].name).GetComponent<Text>().text;
                }
                else
                {
                    text += "\n Rank " + (i + 1);
                }            
            }
            else text += "\n Rank " + (i + 1);

            txt.GetComponent<Text>().text = text;
        }

        yield return new WaitForSeconds(3);
        float fadingTime = GameObject.Find("GameControllerSnake").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadingTime);

        GestionScenes.RetourHub(); 
    }



    IEnumerator SpawnNouriture()
    {
        while (true)
        {
            Vector3 spawnPosition;
            do
            {
                spawnPosition = new Vector2(Random.Range(-spawnValues.x, spawnValues.x), Random.Range(-spawnValues.y, spawnValues.y));
            } while (Physics2D.OverlapCircle(spawnPosition, radius) != null);
            if (spawnEnable)
            {
                Instantiate(cibles[Random.Range(0, cibles.Length -1)], spawnPosition, Quaternion.Euler(0, 0, Random.Range(0, 360)));
            }
            else
            {
                StopCoroutine(SpawnNouriture());
            }
            yield return new WaitForSeconds(Random.Range(0, nouritureDelay));
        }        
    }
}
