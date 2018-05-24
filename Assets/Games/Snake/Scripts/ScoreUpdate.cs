using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ScoreUpdate : MonoBehaviour {

    Text MyScore;
    public GameObject MySnake;
    public Text x2text;
    
    int score = 0;

    // Use this for initialization
    void Start () {
        MyScore = GetComponent<Text>();
        x2text.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (MySnake.GetComponent<SnakeMovements>().partnerAr != null)
        {
            score = ((MySnake.GetComponent<SnakeMovements>().bodyParts.Count -1) + (MySnake.GetComponent<SnakeMovements>().partnerAr.GetComponent<SnakeMovements>().bodyParts.Count - 1));
            MySnake.GetComponent<SnakeMovements>().Score = score;
            x2text.enabled = true;
        }
        else if (MySnake.GetComponent<SnakeMovements>().partnerAv != null)
        {
            score = ((MySnake.GetComponent<SnakeMovements>().bodyParts.Count - 1) + (MySnake.GetComponent<SnakeMovements>().partnerAv.GetComponent<SnakeMovements>().bodyParts.Count - 1));
            MySnake.GetComponent<SnakeMovements>().Score = score;
            x2text.enabled = true;
        }
        else
        {
            score = (MySnake.GetComponent<SnakeMovements>().bodyParts.Count - 1);
            MySnake.GetComponent<SnakeMovements>().Score = score;
            x2text.enabled = false;
        }

        MyScore.text = "Score : " + score.ToString();
        MyScore.color = MySnake.GetComponent<SpriteRenderer>().color;
    }
}
