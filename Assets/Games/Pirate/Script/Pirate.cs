using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Pirate : MonoBehaviour {

    enum phase { normal, damaged, veryDamaged, destroyed };

    public int pv = 100;
    public int MaxPv; // nombre de point de vie du bateau
    public float pvPercentage = 100;
    public Rigidbody2D prefabBouletPirate;
    public Rigidbody2D prefabFume;
    public float speedBoat;
    //Gestion Des Sprites
    public Sprite eventSprite;
    public Sprite normalSprite;
    public Sprite damaged1;
    public Sprite damaged2;
    public Sprite damaged3;



    private Animator anim;

    private phase etat = phase.normal;

    //Permet de géré les déplacements des bateaux
    private Vector2 posDépart;
    private Vector2 posFinal;

    //permet de géré les evenements de clic
    private bool eventLaunchedOnTime = false;
    private int compteurEvent;
    private bool inEvenement = false;
    private float cdDamage;
    private float speedFume = 90;
    public GameObject touchAnim;

    //gérer les points faibles
    public GameObject[] listCible;
    private int indexCibleActive;

    //Permet de géré le cd des tires de boulets
    private float cdFire = 2;

    //Permet de savoir les déplacements des bateaux
    private bool inMovement = false;

    private Rigidbody2D rgbd;
    public GameObject colEvent; // collider de l'event

    void Start () {
        initRgbd();
        pvPercentage = pv * 100 / MaxPv;
        if (tag == "Princesse")
        {
            anim = GetComponent<Animator>();
        }
        else
        {
            StartCoroutine("gestionZoneFaible");
        }
        posDépart = transform.position;

       // StartCoroutine("Pause");
       // enabled = false;
        touchAnim.SetActive(false); 
    }
	
	// Update is called once per frame
	void Update () {


       cdFire -= Time.deltaTime;
        //gestion des coup de feu;
        if (cdFire <= 0)
        {
            fire();
            cdFire = 2;
        }


        if (tag != "Princesse") deplace();

        if (tag == "Princesse") gestionAnimation();
        gestionEvent();

        if (inEvenement)
        {
            if (Tactil.TouchedSouris(gameObject.GetComponent<EdgeCollider2D>())) compteurEvent--;
        }

        if (pv <= 0)
        {
            Destroy(gameObject); //détruit le bateau s'il subit trop de dégat
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Boulet" || collision.gameObject.tag == "BouletPirate")
        {
            if(tag == "Princesse") // si c'est la princesse on la touch elle pert de la vie
            {
                hitted();
            }
            else if (!inEvenement)//si c'est le pirate, il perd de la vie que s'il n'est pas en evenement (bouclier de protection)
            {
                if (tag != "EventCollider" && collision.gameObject.tag != "BouletPirate")
                {
                    hitted();
                }
            }

        }
        if(collision.gameObject.tag == "BouletHeal")
        {
            healled();
        }
    }

    void hitted()
    {
        if (tag == "Princesse")
        {
            pv--;
            pvPercentage = pv * 100 / MaxPv;
            majEtat();
        }
        else if(!inEvenement)
        {
            pv--;
            pvPercentage = pv * 100 / MaxPv;
            majEtat();
        }
    }

    void deplace()
    {
            if (!inMovement)
            {
                posFinal = new Vector2(Random.Range(posDépart.x - 10, posDépart.x + 3), Random.Range(posDépart.y - 3, posDépart.y + 3));
                rgbd.velocity = -((Vector2)transform.position - posFinal).normalized * speedBoat;
                inMovement = true;
            }
            else
            {
                if (Vector2.Distance((Vector2)transform.position, posFinal) < 0.2)
                {
                    resetForce();
                    inMovement = false;
                }
                //regarde la distance par rapport à ce vecteur, si il est proche, passe la vélocity à 0 + changement de mouvemet
            }
    }

    public void resetForce()
    {
        rgbd.velocity = Vector3.zero;
    }

    private void healled()
    {

        switch (etat)
        {
            case phase.normal:
                if (pv < MaxPv)
                    pv++;
                break;

            case phase.damaged:
                if (pvPercentage < 75)
                    pv++;
                break;

            case phase.veryDamaged:
                if (pvPercentage < 50)
                    pv++;
                break;

            case phase.destroyed:
                if (pvPercentage < 25)
                    pv++;
                break;

            default: break;
        }
        pvPercentage = pv * 100 / MaxPv;
    }

    private void zoneFaible()
    {
        StartCoroutine("faibleZone");
        indexCibleActive = Random.Range(0, listCible.Length);
        listCible[indexCibleActive].SetActive(true);
    }

    IEnumerator faibleZone()
    {
        yield return new WaitForSeconds(Random.Range(5, 10));
        listCible[indexCibleActive].SetActive(false);
    }

    private void fire()
    {
        int i = 1;
        foreach (Transform child in transform) //rempli le tableau de pos de boulet
        {
            if (i>2) // permet de skip les deux premier fils qui ne sont pas des emplacements pour les boulets
            {
                Rigidbody2D boulet = Instantiate(prefabBouletPirate, child.position, Quaternion.identity); // la zone de pop étant un enfant du canon il aura donc ca rotation.
                boulet.AddForce(((boulet.position - (Vector2)transform.position)).normalized * 15);
            }
            i++;
        }
    }

    private void launchEvent()
    {
        touchAnim.SetActive(true);
        eventLaunchedOnTime = true;
        compteurEvent = 3*GestionScenes.getNbJoueur();
        inEvenement = true;
        cdDamage = 1;
        colEvent.SetActive(true);
        if (tag != "Princesse")listCible[indexCibleActive].SetActive(false);
        spriteEvent();
    }

    private void gestionEvent()
    {

        if (inEvenement)
        {
            cdDamage -= Time.deltaTime;
            if (tag == "Princesse" && cdDamage <= 0)
            {
                hitted();
                cdDamage = 1;
                Rigidbody2D help = Instantiate(prefabFume, this.transform.position, Quaternion.identity); // la zone de pop étant un enfant du canon il aura donc ca rotation.
                help.AddForce(Vector2.right.normalized * speedFume);
            }
            if (Tactil.TouchedSouris(colEvent.GetComponent<BoxCollider2D>()))
            {
                compteurEvent--;
            }
            if (compteurEvent <= 0)
            {
                colEvent.SetActive(false);
                inEvenement = false;
                touchAnim.SetActive(false);
                spriteNormal();
            }
        }
    }

    private void spriteEvent()
    {
        GetComponent<SpriteRenderer>().sprite = eventSprite;
    }

    private void spriteNormal()
    {
        switch (etat)
        {
            case phase.normal:
                GetComponent<SpriteRenderer>().sprite = normalSprite;
                break;
            case phase.damaged:
                GetComponent<SpriteRenderer>().sprite = damaged1;
                break;
            case phase.veryDamaged:
                GetComponent<SpriteRenderer>().sprite = damaged2;
                break;
            case phase.destroyed:
                GetComponent<SpriteRenderer>().sprite = damaged3;
                break;
        }
    }

    private void majEtat()
    {
        pvPercentage = pv * 100 / MaxPv;
        if (pvPercentage <= 25)
        {
            etat = phase.destroyed;
            GetComponent<SpriteRenderer>().sprite = damaged3;
        }
        else if (pvPercentage <= 50)
        {
            if (!eventLaunchedOnTime)
            {
                launchEvent();
            }
            etat = phase.veryDamaged;
            if(!inEvenement)GetComponent<SpriteRenderer>().sprite = damaged2;
        }
        else if (pvPercentage <= 75)
        {
            etat = phase.damaged;
            GetComponent<SpriteRenderer>().sprite = damaged1;
        }
    }

    void gestionAnimation()
    {
        anim.SetInteger("PvPercentage", (int)pvPercentage);
        anim.SetBool("inEvent", inEvenement);
        anim.SetBool("enable", enabled);
    }

    public void touchFaiblesse()
    {
        if (!inEvenement)
        pv -= 2;
    }

    IEnumerator gestionZoneFaible()
    {
        yield return new WaitForSeconds(5);
        while (true)
        {
            if (!inEvenement)
            {
                if (enabled)
                {
                    zoneFaible();
                    yield return new WaitForSeconds(Random.Range(14, 29));
                }
            }
            yield return new WaitForSeconds(1);
        }
    }

    public void initRgbd()
    {
        rgbd = GetComponent<Rigidbody2D>();
    }

    /*IEnumerator Pause()
    {
        yield return new WaitForSeconds(3);
        enabled = true;
    }*/
}
