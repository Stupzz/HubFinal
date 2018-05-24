using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject cible;  // normale
    public Vector3 spawnValues;
    public float gameTime;
    public float taupeDelay;
    public float startWait;

    public GameObject blur;

    private float radius;
    private int score;

    public Text ScoreText;
    public Text TimeText;
	public Text ResultTop;
	public Text ResultBot;

	// Use this for initialization
	void Start () {
		enabled = false;
		StartCoroutine ("showObjectif");
        //spawnValues.z = Screen.height/2;
        //spawnValues.x = Screen.width/2;
	}
	
    IEnumerator SpawnTaupeNorm()
    {
        yield return new WaitForSeconds(startWait);
        while (gameTime > 2)
        {
            int myCheck;
            Vector3 spawnPosition;
            RaycastHit2D hit;
            int count = 0;
            do
            {
                count++;
                myCheck = 0;
                spawnPosition = new Vector2(Random.Range(-spawnValues.x, spawnValues.x), Random.Range(-spawnValues.y, spawnValues.y));
                //hit = Physics2D.Raycast(spawnPosition, Vector2.up);
                /*Collider[] hittedColliders  = Physics.OverlapSphere(spawnPosition, radius); 
                for (int j = 0; j < hittedColliders.Length; j++)
                {
                    if (hittedColliders[j].tag == "Cible")
                    {
                        myCheck++;
                    }
                }
            } while (myCheck > 0);*/
            } while (Physics2D.OverlapCircle(spawnPosition, radius) != null && count <= 100);
            //} while (hit.collider != null);
            if (count != 20)
            {
                Instantiate(cible, spawnPosition, Quaternion.Euler(0,0,Random.Range(0,360)));
            }
            else
            {
                yield return new WaitForSeconds(5 * taupeDelay);
            }
            

            yield return new WaitForSeconds(Random.Range(0,taupeDelay));
        }
        yield return new WaitForSeconds(5);
    }

    // Update is called once per frame
    void Update () {
        GestionScenes.clavierUtils();
        if (gameTime > 0)
        {
            UpdateTime();
        }
        else
        {
			gameOver ();
            //Application.Quit();
        }

        
        
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    public void UpdateScore()
    {
       ScoreText.text = score.ToString();
    }

    void UpdateTime()
    {
        gameTime = gameTime - Time.deltaTime;
        TimeText.text = Mathf.CeilToInt(gameTime).ToString();
        TimeText.text = Mathf.CeilToInt(gameTime).ToString();
    }

	void gameOver() {
		ScoreText.text = "";
		TimeText.text = "";
		ResultTop.text = "Score : " + score.ToString ();
		ResultBot.text = "Score : " + score.ToString ();
		enabled = false;
        StartCoroutine("Fin");
	}

	IEnumerator showObjectif() {
		ResultTop.text = "3";
		ResultBot.text = "3";
		yield return new WaitForSeconds (1f);

		ResultTop.text = "2";
		ResultBot.text = "2";
		yield return new WaitForSeconds (1f);

		ResultTop.text = "1";
		ResultBot.text = "1";
		yield return new WaitForSeconds (1f);

		ResultTop.text = "TAPEZ LES TAUPES !";
		ResultBot.text = "TAPEZ LES TAUPES !";
		yield return new WaitForSeconds (1f);

		ResultTop.text = "";
		ResultBot.text = "";

		enabled = true;

		radius = cible.GetComponent<Renderer>().bounds.size.x;
		print(radius);
		score = 0;
		StartCoroutine(SpawnTaupeNorm());
		//ChoixType = cible;
		UpdateScore();
		UpdateTime();
	}

    IEnumerator Fin()
    {
        blur.SetActive(true);
        yield return new WaitForSeconds(3);
        float fadingTime = GameObject.Find("Main Camera").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadingTime);
        GestionScenes.RetourHub();
    }
}
