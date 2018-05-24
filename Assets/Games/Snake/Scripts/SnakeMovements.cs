using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnakeMovements : MonoBehaviour {

	public List<Transform> bodyParts = new List<Transform>(); //Liste des boules du corps du snake
    public Transform partnerAv;                               //Partner se trouvant devant moi (que je suis)
    public Transform partnerAr;                               //Partner se travant derière moi (qui me suis)

    public bool alreadyFusionSomeoneElse;                    //Boolean pour savoir si ce snake s'est déjà attacher derière un autre snake
    public bool invulnerability;                             //Boolean d'invulnérabilité, true = invulnérable

    public Color InitColor;

    private Animator animatorImune;                              //Animator de cet objet

    public int Score;

    void Start () {
                                                            //Init
        partnerAv = null;                                   //sans partenaire avantx
        partnerAr = null;                                   //sans partenaire arière
        alreadyFusionSomeoneElse = false;                   //Sans s'etre déja attaché derière quelqu'un
        Physics2D.IgnoreLayerCollision(0, 8);               //Sans invulnérabilité
        InitColor = GetComponent<SpriteRenderer>().color;   //Couleur Initiale  = (Couleur à la création)
        animatorImune = GetComponent<Animator>();           //Animator de l'imunité
        animatorImune.enabled = false;
 
    }
	
	void Update ()
    {

        
        //Partage des couleurs
        if (partnerAv != null)
        {
            GetComponent<SpriteRenderer>().color = partnerAv.GetComponent<SpriteRenderer>().color;
        }
        else if (partnerAr != null)
        {
            Color tmp = InitColor;
            tmp.r = (InitColor.r + Math.Max(partnerAr.GetComponent<SnakeMovements>().InitColor.b, Math.Max(partnerAr.GetComponent<SnakeMovements>().InitColor.r, partnerAr.GetComponent<SnakeMovements>().InitColor.g))) / 2;
            tmp.g = (InitColor.g + Math.Max(partnerAr.GetComponent<SnakeMovements>().InitColor.b, Math.Max(partnerAr.GetComponent<SnakeMovements>().InitColor.r, partnerAr.GetComponent<SnakeMovements>().InitColor.g))) / 2;
            tmp.b = (InitColor.b + Math.Max(partnerAr.GetComponent<SnakeMovements>().InitColor.b, Math.Max(partnerAr.GetComponent<SnakeMovements>().InitColor.r, partnerAr.GetComponent<SnakeMovements>().InitColor.g))) / 2;
            GetComponent<SpriteRenderer>().color = tmp;
        }
        else GetComponent<SpriteRenderer>().color = InitColor;

        //Commandes du controle clavier pour simplifications des tests sur machine
        if (gameObject.transform.parent.name == "Snake1")
        {
            if (Input.GetKey(KeyCode.Q))
            {
                currentRotation += RotationSensitivity * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                currentRotation -= RotationSensitivity * Time.deltaTime;
            }
        }else if (gameObject.transform.parent.name == "Snake2")
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                currentRotation += RotationSensitivity * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                currentRotation -= RotationSensitivity * Time.deltaTime;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Keypad4))
            {
                currentRotation += RotationSensitivity * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.Keypad6))
            {
                currentRotation -= RotationSensitivity * Time.deltaTime;
            }
        }
	}


	public float speed = 3.5f; //Vitesse de la tete du snake

	public float currentRotation;                  // valeur de la rotation actuelle 
	public float RotationSensitivity = 50.0f;	   //Sensibilité de la rotation

    private Vector3 movementVelocity;
    [Range(0.0f, 1.0f)]
    public float overTime = 0.5f;                //Vitesse a la quelle le corps ratrappe ce qu'elle suit (la distance augmente avec la vitesse de la tete)


    void FixedUpdate(){                            //check 3 fois par frame
        if (partnerAv == null)
        {
            GetComponent<Collider2D>().isTrigger = false;       // Activer collider
            GetComponent<Rigidbody2D>().isKinematic = false;    // Activer le rigidbody uniquement sur la tete qui dirige (avant)
            MoveForward();                                      // Fait avancer le snake
            Rotation();                                         // gere la rotation du snake
        }
        else
        {
            //Si c'est la première fois que je fusionne avec quelqu'un 
            if (!alreadyFusionSomeoneElse)
            {   
                //je déplace les 2 bouttons (Up/Down) a la pos du serpent
                GetComponent<Transform>().parent.GetChild(2).GetComponent<Transform>().position = new Vector3(
                    GetComponent<Transform>().position.x,
                    GetComponent<Transform>().position.y + (GetComponent<Transform>().parent.GetChild(2).GetComponent<Transform>().position.y - GetComponent<Transform>().parent.transform.position.y),
                    GetComponent<Transform>().position.z);
                GetComponent<Transform>().parent.GetChild(3).GetComponent<Transform>().position = new Vector3(
                    GetComponent<Transform>().position.x,
                    GetComponent<Transform>().position.y + (GetComponent<Transform>().parent.GetChild(3).GetComponent<Transform>().position.y - GetComponent<Transform>().parent.transform.position.y),
                    GetComponent<Transform>().position.z);
                //Le bool passe a true puisque j'ai rencontrer une personne
                alreadyFusionSomeoneElse = true;
            }

            //Une tete qui suit, suit la queue (dernière partie de son corps) de son partenaire avant
            transform.position = Vector3.SmoothDamp(transform.position,
                partnerAv.GetComponent<SnakeMovements>().bodyParts[partnerAv.GetComponent<SnakeMovements>().bodyParts.Count-1].position,
                ref movementVelocity, 
                overTime);
            GetComponent<Rigidbody2D>().isKinematic = true;     //Une tete qui suit n'a plus de rigidbody
            GetComponent<Collider2D>().isTrigger = true;        //Une tete qui suit ne collide plus
        }
	}


	void MoveForward (){                          // Mouvement vers l'avant
		transform.position += transform.up * speed * Time.deltaTime;
	}

	void Rotation(){                                          //applique une rotation autour de l'axe Z 
		transform.rotation = Quaternion.Euler (new Vector3 (transform.rotation.x, transform.rotation.y, currentRotation));
	}

    public Transform bodyObject;                     // attachement du prefab "body" pour gerer la generation apres chaque consomation de pomme

	void OnCollisionEnter2D(Collision2D other) {                        //2D TRES IMPORTANT 
		if (other.transform.tag == "Apple") {                             // check le tag de l'objet en collision
			Destroy(other.gameObject);                                    // detruit la pomme touchée
            AddBody();
		}
        if (other.transform.tag == "ApplePourrie")                        // check le tag de l'objet en collision
        {        
            
            Destroy(other.gameObject);                                    // detruit la pomme touchée
            if(!invulnerability)                                          //Si j'y suis sensible
                RemoveBody();                                             //Enleve un bout de corps
        }
        if (other.transform.tag == "AppleGold")
        {
            Destroy(other.gameObject);                                  //Detruit la pomme touché
            AddBody();                                                  //Ajoute 2 bouts de corps    
            AddBody();
            StartCoroutine(Invulnerability(5.0f));                      //Le serpent est invulnérable pendant 5 secondes
            if (partnerAr != null)
                StartCoroutine(partnerAr.GetComponent<SnakeMovements>().Invulnerability(5.0f));
        }
    }

    void AddBody()
    {
        if (bodyParts.Count == 0)
        {                                   //si pas de corps
            Vector2 currentPos = transform.position;                  // position de la premiere boule du coups sur la tete
            Transform newBodyPart = Instantiate(bodyObject, currentPos, Quaternion.identity) as Transform;        //On instantie
            bodyParts.Add(newBodyPart);                                                                           // ajout dans la liste des bouts de corps
            newBodyPart.GetComponent<SpriteRenderer>().sortingOrder = 0;                                           //On met a la position 0 dans le layer 
            newBodyPart.transform.parent = gameObject.transform.parent;                                            //Rangement --> limite la polution visuel dans l'éditeur + simplifie certaine gestions
            newBodyPart.GetComponent<SnakeBody>().head = this.transform;                                           //On donne sa tete Au corps (au lieu de passer apr tag)
        }
        else
        {                                                              // si on a deja des boules du corps
            Vector3 currentPos = bodyParts[bodyParts.Count - 1].position;     // on fait apparaitre la prochaine boule aux coordonnées de la dernierre boule du corps
            Transform newBodyPart = Instantiate(bodyObject, currentPos, Quaternion.identity) as Transform; //On instantie
            bodyParts.Add(newBodyPart);                                                                      // ajout dans la liste des bouts de corps
            MajLayers();                                                    //Decalage de toutes les autres boules du couprs dans le layer
            newBodyPart.GetComponent<SpriteRenderer>().sortingOrder = 0;   //On place la dernierre boule en dessoud de toutes les autres
            newBodyPart.transform.parent = gameObject.transform.parent;     //Rangement --> limite la polution visuel dans l'éditeur + simplifie certaine gestions
            newBodyPart.GetComponent<SnakeBody>().head = this.transform;    //On donne sa tete Au corps (au lieu de passer apr tag)
        }
        if(partnerAr != null)
        {
            partnerAr.GetComponent<SnakeMovements>().AddBody();
        }
    }

    void RemoveBody()
    {
        if (bodyParts.Count > 1)  //si j'ai un corps
        {                                  
            Transform removedBodyPart = bodyParts[bodyParts.Count - 1];
            bodyParts.Remove(removedBodyPart);
            Destroy(removedBodyPart.gameObject);
        }
        
        if (partnerAr != null) //J'applique aussi sur mon partenaire
        {
            partnerAr.GetComponent<SnakeMovements>().RemoveBody();
        }
    }

    void MajLayers(){                    // decalage des boules du corps pour faire apparaitre une nouvelle en dessous (couche 0 du layer)
		
		for (int i = 0; i < bodyParts.Count; i++) {
			bodyParts[i].GetComponent<SpriteRenderer> ().sortingOrder = bodyParts[i].GetComponent<SpriteRenderer> ().sortingOrder+1;
		}
	}
		
    //Mouvement gauche
    public void RotateLeft()
    {
        currentRotation += RotationSensitivity * Time.deltaTime;
    }

    //Mouvement droit
    public void RotateRight()
    {
        currentRotation -= RotationSensitivity * Time.deltaTime;
    }

    //Ralentissement
    public void SlowDown()
    {
        partnerAv.GetComponent<SnakeMovements>().speed = 1.5f;
    }

    //Accélération
    public void SpeedUp()
    {   
        if(partnerAv != null) partnerAv.GetComponent<SnakeMovements>().speed = 3.5f;
    }

    //Remet la vitesse
    public void ResetVitesse()
    {
        if (partnerAv != null) partnerAv.GetComponent<SnakeMovements>().speed = 2.5f;
    }

    public IEnumerator Invulnerability(float during)
    {
        invulnerability = true;                     //Set bool a true
        animatorImune.enabled = true;
        yield return new WaitForSeconds(during);    //Attends during secondes
        invulnerability = false;                    //Reset bool a false1
        animatorImune.enabled = false;
        yield return null;                          //Sort du Coroutine        
    }
}

