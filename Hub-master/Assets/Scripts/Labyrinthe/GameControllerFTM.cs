using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameControllerFTM : MonoBehaviour {

    public GameObject panda;
    public GameObject bear;
    public float time;
    public Vector2 spawnValues;
    public Text text;
	public GameObject Parent;

    public int HauteurLabyrinthe;
    public int LargeurLabyrinthe;

	// Use this for initialization
	void Start () {
        Vector2 spawnPosition;
        for (float i = 2 ; i < LargeurLabyrinthe; i=i+2)
        {
            for (float y = 2; y < HauteurLabyrinthe;  y=y+2)
            {
                spawnPosition = new Vector2(i, y);
                var NewObject= Instantiate(bear, spawnPosition, Quaternion.identity);
				NewObject.transform.parent = Parent.transform;
			}           
        }

        for (float i = 2; i < LargeurLabyrinthe; i=i+2)
        {
            for (float y = HauteurLabyrinthe + 3; y < HauteurLabyrinthe * 2 + 2; y=y+2)
            {
                spawnPosition = new Vector2(i, y);
                var NewObject = Instantiate(bear, spawnPosition, Quaternion.Euler(0, 0, 180));
				NewObject.transform.parent = Parent.transform;
            }
        }

        float posx = Mathf.Floor(Random.Range((float) 1.0, (float) LargeurLabyrinthe/2)) *2;
        float posy = Mathf.Floor(Random.Range((float) 0.5, (float) HauteurLabyrinthe/2)) *2;

        var NewObject2 = Instantiate(panda, new Vector2(posx, posy), Quaternion.identity);
		NewObject2.transform.parent = Parent.transform;
        posx = Mathf.Floor(Random.Range((float) 1.0, (float)LargeurLabyrinthe)/2) *2;
		posy = Mathf.Floor(Random.Range((float) (HauteurLabyrinthe + 3)/2, (float)(HauteurLabyrinthe * 2 + 2)/2)) *2;

        var NewObject3 = Instantiate(panda, new Vector2(posx, posy), Quaternion.Euler(0,0,180));
		NewObject3.transform.parent = Parent.transform;
    }
	
	// Update is called once per frame
	void Update () {

        if(Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                RaycastHit2D raycastHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.touches[i].position), Vector2.zero);

                if (raycastHit.collider != null)
                {
					if(raycastHit.collider.tag == "Panda")
                    {
						if (Input.touches [i].position.y <= HauteurLabyrinthe) {
						
							win (1);
						} else {
							win (2);
						}
                    }
                }
            }
        }
        
        time =  time - Time.deltaTime;
        if(time <= 0)
        {
            lose();
        }
	}

    void lose()
    {
		Result.winner = 0;
		Result.jeu = "Panda";
		Result.finJeu = true;
        text.text = "lose";
		SceneManager.UnloadScene (SceneManager.GetSceneByName ("Panda"));
    }

	void win(int equipe)
    {
		Result.winner = equipe;
        text.text = "win";
        Result.jeu = "panda";
        Result.finJeu = true;
        SceneManager.UnloadScene(SceneManager.GetSceneByName("Panda"));
    }
}
