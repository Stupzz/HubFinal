using UnityEngine;
using System.Collections;

public class SnakeBody : MonoBehaviour {

	private int myOrder;                         //position dans la liste des corps
	public Transform head;                       // tete a la quelle la boule du corps est attachée


	void Start(){
		//head = GameObject.FindGameObjectWithTag("Player").gameObject.transform;                           //recuperation de la tete
		for(int i = 0; i < head.GetComponent<SnakeMovements>().bodyParts.Count; i++){                     //parcour de la liste du corps
			if(gameObject == head.GetComponent<SnakeMovements>().bodyParts[i].gameObject){                //ajoute la boule dans la liste, et met a jour le INT position de la boulle
				myOrder = i;
			}
		}
        GetComponent<SpriteRenderer>().color = head.GetComponent<SpriteRenderer>().color;
    }

    void Update()
    {
        
    }

    private Vector3 movementVelocity;
	[Range(0.0f,1.0f)]
	public float overTime = 0.5f;                //Vitesse a la quelle le corps ratrappe la tete (la distance augmente avec la vitesse de la tete)

	void FixedUpdate(){
        GetComponent<SpriteRenderer>().color = head.GetComponent<SpriteRenderer>().color;
        if (myOrder == 0){     //si premier de la liste, je suis la tete
			transform.position = Vector3.SmoothDamp(transform.position, head.position, ref movementVelocity, overTime);
		}
		else{ // sinon je suis la boule devant moi
			transform.position = Vector3.SmoothDamp(transform.position, head.GetComponent<SnakeMovements>().bodyParts[myOrder-1].position, ref movementVelocity, overTime);
		}
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player"                                    //Si je me fait touché par un joueur
            && other.gameObject.transform != head                               //Qui n'est pas ma propre tete,
            && other.GetComponent<SnakeMovements>().partnerAv != head           //Qui n'est pas mon partenaireAv,
            && other.GetComponent<SnakeMovements>().partnerAr != head           //Ni mon partenaireAr
            && !head.GetComponent<SnakeMovements>().invulnerability)            //Que je suis pas invulnérable
        {
            DecoupeCorps(myOrder, other.gameObject.transform);                  //Je me fait découper
        }
    }

    void DecoupeCorps(int order, Transform otherT)
    {   
        //Découper = destruction de mon corps de l'endroit touché à ma queue 
        for (int i = head.GetComponent<SnakeMovements>().bodyParts.Count - 1; i>order ; i--)
        {
            Destroy(head.GetComponent<SnakeMovements>().bodyParts[i].gameObject);
            head.GetComponent<SnakeMovements>().bodyParts.Remove(head.GetComponent<SnakeMovements>().bodyParts[i]);
        }

        if (otherT.GetComponent<SnakeMovements>().partnerAv == null && !otherT.GetComponent<SnakeMovements>().invulnerability)
        {                                                              //si celui qui me touche n'est pas en coop derriere
            if (head.GetComponent<SnakeMovements>().partnerAv == null && head.GetComponent<SnakeMovements>().partnerAr == null)    //Si je suis solo
            {
                if (otherT.GetComponent<SnakeMovements>().partnerAv == null && otherT.GetComponent<SnakeMovements>().partnerAr == null) //si celui qui me touche est solo
                {
                    head.GetComponent<SnakeMovements>().partnerAr = otherT;                                                 //Celui qui m'a mordu est mon nouveau partnaire arière
                    otherT.GetComponent<SnakeMovements>().partnerAv = head;                                                 //Je suis le nouveau partenaire avant de celui qui m'a mordu
                   
                }
            }
            else if (head.GetComponent<SnakeMovements>().partnerAv == null && head.GetComponent<SnakeMovements>().partnerAr != null)       // Si je suis devant en coop
            {
                head.GetComponent<SnakeMovements>().partnerAr.GetComponent<SnakeMovements>().ResetVitesse();      //Ma vitesse redeviens normal (je n'ai plus de personne derière pour me booster)
                head.GetComponent<SnakeMovements>().partnerAr.GetComponent<SnakeMovements>().partnerAv = null;   //je me fais detacher 
                StartCoroutine(head.GetComponent<SnakeMovements>().partnerAr.GetComponent<SnakeMovements>().Invulnerability(1.0f)); //Imune for x seconds pour ancien partenaire
                StartCoroutine(head.GetComponent<SnakeMovements>().Invulnerability(1.0f));                      //Imune for x seconde pour la tete avant
                head.GetComponent<SnakeMovements>().partnerAr = null;
                
            }
        }

        
    }

    }
