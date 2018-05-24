using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SwapIcon : MonoBehaviour {
	public const int nbFrameTotal = 110;
	private Vector3 basicScale = new Vector3 (0.3f, 0.3f, 0f);

	public ArrayList jeux;
    public Games FinalGames;
	private SpriteRenderer spriteRenderer;

    public int countSprite;
	private int countFrame;
	private int spriteCourrant;
    private float nbFrametoChangeSprite;
	private bool Stop;
	private bool slowStarted;
	private bool augmenteIcon;

    // Use this for initialization
    void Start () {
		transform.localScale = basicScale;
		countFrame = 1;
		countSprite = 0;
		nbFrametoChangeSprite = 2;
        spriteCourrant = calculSpriteInitial();
        StartCoroutine(Timer());
		spriteRenderer = GetComponent<SpriteRenderer> ();
		Stop = false;
		slowStarted = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {

		if (!Stop) {
			if (countFrame++ % (int)nbFrametoChangeSprite == 0) { //Permet de faire ralentir le changement de sprite
				spriteRenderer.sprite = nextSprite ();
				countSprite++;
				countFrame = 1;
				if (slowStarted)
					nbFrametoChangeSprite *= 1.1f; // on augmente le nb de frame à atteindre pour simuler un ralentissement
			}

			if (countSprite >= nbFrameTotal) {
				Stop = true;
			}
		}
		if (augmenteIcon) {
			this.transform.localScale += new Vector3 (0.01f, 0.01f, 0f);
			if (transform.localScale.x >= 2.3) {
				augmenteIcon = false;
			}
		}
	}


    private Sprite nextSprite()
    {
		return  ((Games)jeux[++spriteCourrant % jeux.Count]).sprite;
    }

    // attente avant de faire ralentir la hasard + Permet de faire la dernière animation
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(3);
        slowStarted = true;

        while (!Stop)
        {
            yield return null;
        }

        augmenteIcon = true;

        while (augmenteIcon)
        {
            yield return null;
        }
        Destroy(this.gameObject);
    }

    private int calculSpriteInitial()
    {
		int indexSpriteInit = jeux.IndexOf(FinalGames) - (nbFrameTotal % jeux.Count); //formule qui permet de connaitre sur quel frame partir pour tomber sur le jeux voulu
        if (indexSpriteInit < 0)
			indexSpriteInit += jeux.Count;

        return indexSpriteInit;
    }
}
