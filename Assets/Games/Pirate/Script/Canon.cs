using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : MonoBehaviour {

    public bool damaged = false;
    public CircleCollider2D touchZone;
    public float speedBoulet = 25;
    public Rigidbody2D prefabBoulet;
    public Rigidbody2D prefabBouletHeal;
    public Rigidbody2D prefabFume;
    private float speedFume = 60;
    public GameObject posBoulet;

    public GameObject buttons;


    public Sprite normal;
    public Sprite endomage;

    private SpriteRenderer spriteRenderer;
	void Start () {
        Physics2D.IgnoreCollision(touchZone, this.GetComponent<EdgeCollider2D>());
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        buttons.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
        Touch[] myTouches = Input.touches;
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch myTouch = Input.GetTouch(i);
            Vector2 touchPos = (Vector2)Camera.main.ScreenToWorldPoint(myTouch.position); // position du touch
            RaycastHit2D hit = Physics2D.Raycast(touchPos, -Vector2.zero);

            if (hit.collider != null && hit.collider == touchZone /*&& Vector3.Distance(touchZone.gameObject.transform.position, touchPos) < 1.2*/)
            {
                orienter(touchPos);
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Boulet" || collision.gameObject.tag == "BouletPirate")
        {
            spriteRenderer.sprite = endomage;
            damaged = true;
        }
        else if (collision.gameObject.tag == "BouletHeal")
        {
            spriteRenderer.sprite = normal;
            damaged = false;
        }
    }


    void orienter(Vector2 position) //position inverse de laquelle le canon va tirer
    {
        float degre = Vector2.Angle( (((Vector2)touchZone.transform.position) - position), Vector2.up); // permet de déterminé l'angle entre la position du canon et la pos du touch
        if (this.tag == "canonBot")
        {
            if (degre > 90) degre = 90;
            if (position.x < touchZone.transform.position.x)// le touch est à gauche par rapport au canon, il faut appliquer un dégré négatif pour avoir la bonne orientation
            {
                degre = -degre;
            }
            this.transform.eulerAngles = new Vector3(0, 0, degre);
        }
        else
        {
            if (degre < 90) degre = 90;
            if (position.x < touchZone.transform.position.x)// le touch est à gauche par rapport au canon, il faut appliquer un dégré négatif pour avoir la bonne orientation
            {
                degre = -degre;
            }
            this.transform.eulerAngles = new Vector3(0, 0, degre);
        }
    }

    public void fire()
    {
        if (!damaged)
        {
            Rigidbody2D boulet = Instantiate(prefabBoulet, posBoulet.transform.position, Quaternion.identity); // la zone de pop étant un enfant du canon il aura donc ca rotation.
            boulet.AddForce((boulet.position - (Vector2)transform.position) * speedBoulet);
        }
        else
        {
            Rigidbody2D help = Instantiate(prefabFume, posBoulet.transform.position, Quaternion.identity); // la zone de pop étant un enfant du canon il aura donc ca rotation.
            help.AddForce(((help.position - (Vector2)transform.position)).normalized * speedFume);
        }
    }

    public void heal()
    {
        if (!damaged)
        {
            //animation.Play();
            Rigidbody2D boulet = Instantiate(prefabBouletHeal, posBoulet.transform.position, Quaternion.identity); // la zone de pop étant un enfant du canon il aura donc ca rotation.
            boulet.AddForce((boulet.position - (Vector2)transform.position) * speedBoulet);
        }
        else
        {
            Rigidbody2D help = Instantiate(prefabFume, posBoulet.transform.position, Quaternion.identity); // la zone de pop étant un enfant du canon il aura donc ca rotation.
            help.AddForce(((help.position - (Vector2)transform.position)).normalized * speedFume);
        }
    }

    public bool isDamaged()
    {
        return damaged;
    }

    public void lance()
    {
        this.gameObject.SetActive(true);
    }
}
