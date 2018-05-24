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

    private float radius;
    private int score;

    public Text ScoreText;
    public Text TimeText;

	// Use this for initialization
	void Start () {
        //spawnValues.z = Screen.height/2;
        //spawnValues.x = Screen.width/2;
        radius = cible.GetComponent<Renderer>().bounds.size.x;
        print(radius);
        score = 0;
        StartCoroutine(SpawnTaupeNorm());
        //ChoixType = cible;
        UpdateScore();
        UpdateTime();
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
        if (gameTime > 0)
        {
            UpdateTime();
        }
        else
        {
            print("Game Over");
            //Application.Quit();
        }

        //Tests
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(Application.loadedLevel);
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
}
