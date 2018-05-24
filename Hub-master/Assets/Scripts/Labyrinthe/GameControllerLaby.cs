using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameControllerLaby : MonoBehaviour {


    public float decompte;
   /* public ControlsTeam1 manette1;
    public ControlsTeam2 manette2;

    public GameObject manetteO1;
    public GameObject manetteO2;*/
    public GameObject player1;
    public GameObject player2;
   
  
    public Tilemap maMap;
    public int LargeurLabyrinthe;
    public int HauteurLabyrinthe;

    public bool rollEnable;

    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {

        Physics2D.IgnoreLayerCollision(9, 11);
        Physics2D.IgnoreLayerCollision(9, 12);

        //Gestion Jeu
        TestWin();

        //Gestion Temps
		if (Result.finJeu) {
			LancerBonus (Result.jeu, Result.winner);
            player1.SetActive(true);
            player2.SetActive(true);
            /*manetteO1.SetActive(true);
            manetteO2.SetActive(true);*/
        } 
		else 
		{
			if (decompte != 0) {
				decompte = decompte - Time.deltaTime;
			}
			if (decompte < 0) {
                player1.SetActive(false);
                player2.SetActive(false);
                /*manetteO1.SetActive(false);
                manetteO2.SetActive(false);*/
				SceneManager.LoadScene ("FTP", LoadSceneMode.Additive);
				decompte = 0;
			}
		}


    }

    void TestWin()
    {
        if (player1.transform.position.x  > LargeurLabyrinthe - 2 && player1.transform.position.y < 1)
        {
            //P1 WIN
            player1.transform.position = (new Vector2(1,HauteurLabyrinthe-2));
        }
        if (player2.transform.position.x < 2 && player2.transform.position.y < HauteurLabyrinthe * 2 -1)
        {
            //P2 WIN
            player2.transform.position = (new Vector2(LargeurLabyrinthe - 2, HauteurLabyrinthe + 3));
        }

    }

	void LancerBonus(string jeu, int equipe) {
		switch (jeu) {
		    case "Panda":
			    StartCoroutine(Freeze (2, equipe));
                //RollTheLab(20);
			    Result.finJeu = false;
                decompte = 50;
			    break;
            case "Ozu":
                RollTheLab(20);
                Result.finJeu = false;
                decompte = 10;
                break;
		default:
			break;
		}
	}

    IEnumerator Freeze(int time, int equipe)
	{
		if (equipe == 0) {
            player1.GetComponent<ArrowTouchScriptP1>().speed = 0;
            player2.GetComponent<ArrowTouchScriptP2>().speed = 0;
            yield return new WaitForSecondsRealtime(time);
            player1.GetComponent<ArrowTouchScriptP1>().speed=1;
            player2.GetComponent<ArrowTouchScriptP2>().speed=1;
        }
        if (equipe == 1)
        {
            player2.GetComponent<ArrowTouchScriptP2>().speed = 0;
            yield return new WaitForSecondsRealtime(time);
            player2.GetComponent<ArrowTouchScriptP2>().speed = 1;
        }
        if (equipe == 2)
        {
            player1.GetComponent<ArrowTouchScriptP1>().speed = 0;
            yield return new WaitForSecondsRealtime(time);
            player1.GetComponent<ArrowTouchScriptP1>().speed = 1;
        }
        yield return 0;
	}


    //Fonction RollLab
    public void onClickUp(int col)
    {
        TileBase tileuUp = maMap.GetTile(new Vector3Int(col, HauteurLabyrinthe * 2 - 2, 0));
        for (int i = HauteurLabyrinthe * 2; i > 1; i--)
        {
            if (i == HauteurLabyrinthe + 3)
            {
                maMap.SetTile(new Vector3Int(col, i, 0), maMap.GetTile(new Vector3Int(col, HauteurLabyrinthe - 2, 0)));
                i = HauteurLabyrinthe - 2;
            }
            maMap.SetTile(new Vector3Int(col, i, 0), maMap.GetTile(new Vector3Int(col, i - 1, 0)));
            print(i);
        }

        maMap.SetTile(new Vector3Int(col, 1, 0), tileuUp);
    }

    public void onClickDown(int col)
    {
        TileBase tiledown = maMap.GetTile(new Vector3Int(col, 1, 0));
        for (int i = 1; i < HauteurLabyrinthe * 2; i++)
        {
            if (i == HauteurLabyrinthe - 2)
            {
                maMap.SetTile(new Vector3Int(col, i, 0), maMap.GetTile(new Vector3Int(col, HauteurLabyrinthe + 3, 0)));
                i = HauteurLabyrinthe + 3;
            }
            maMap.SetTile(new Vector3Int(col, i, 0), maMap.GetTile(new Vector3Int(col, i + 1, 0)));
        }
        maMap.SetTile(new Vector3Int(col, HauteurLabyrinthe * 2, 0), tiledown);
    }

    void RollTheLab(float time)
    {
        if (time > 0)
        {
            decompte = decompte - Time.deltaTime;
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    //Si pas sur la ligne d'un player
                    if (Mathf.CeilToInt(player1.transform.position.x) != Mathf.CeilToInt(Input.touches[i].position.x) && Mathf.CeilToInt(player2.transform.position.x) != Mathf.CeilToInt(Input.touches[i].position.x))
                    {
                        //Si on est l'équipe du bas
                        if (Input.touches[i].position.y < 2 && Input.touches[i].position.x > 2 && Input.touches[i].position.x < LargeurLabyrinthe - 2)
                        {
                            onClickUp(Mathf.CeilToInt(Input.touches[i].position.x));
                        }

                        //Si On est l'équipe du haut
                        if (Input.touches[i].position.y > HauteurLabyrinthe * 2 && Input.touches[i].position.x > 2 && Input.touches[i].position.x < LargeurLabyrinthe - 2)
                        {
                            onClickDown(Mathf.CeilToInt(Input.touches[i].position.x));
                        }
                    }
                }
            }
            time = time - Time.deltaTime;
        }
        else return;
    }
}
