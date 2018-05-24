using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showResults : MonoBehaviour {


    public float DelayResults;
    public List<GameObject> Snakes;

    // Use this for initialization
    void Start () {
        gameObject.GetComponent<Text>().supportRichText = true;
        DelayResults = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (DelayResults == 5)
        {
            AffichageResultats();
            DelayResults -= Time.deltaTime;
        }
		if (DelayResults > 0)
        {
            DelayResults -= Time.deltaTime;
        }
        else if (DelayResults < 0)
        {
            // return hub
        }
	}

    public void AffichageResultats()
    {
       /* Snakes.Sort((a, b) => 
        int.Parse(a.transform.GetChild(a.transform.childCount - 2).GetComponent<Text>().text).CompareTo(int.Parse(b.transform.GetChild(b.transform.childCount - 2).GetComponent<Text>().text)));*/
        string tmp = "";
        string tmpColor;
        for(int i = 0; i< Snakes.Count; i++)
        {
            tmp = Snakes[i].transform.GetChild(Snakes[i].transform.childCount - 2).GetComponent<Text>().text;
            tmpColor = ColorUtility.ToHtmlStringRGB(Snakes[i].transform.GetChild(4).GetComponent<SpriteRenderer>().color);
            gameObject.GetComponent<Text>().text += "<color=" +tmpColor+">"+(i+1)+" </color>";
            gameObject.GetComponent<Text>().text += "<color=" + tmpColor + "> "+Snakes[i].name + " </color>";
            gameObject.GetComponent<Text>().text += "    ";
            gameObject.GetComponent<Text>().text += "<color=" + tmpColor + "> "+tmp + "</color>";
            gameObject.GetComponent<Text>().text += "\n";
            
        }

        gameObject.GetComponent<Text>().enabled = true;
        new WaitForSeconds(5);
        //Go hub
    }
}
