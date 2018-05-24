using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bouton : MonoBehaviour {

    public GameObject MySnake;  //Serpent controlé par le boutton
    public string MyRole;       //Role du boutton ( utilisation L pour Rotation Left, R -- Right, U -- Up/Accéléré, D -- Down/Décéléré

    public Vector3 initPos;     //Position Initiale = position "normale" dans l'UI

    private void Start()
    {
        initPos = GetComponent<Transform>().position;
        GetComponent<SpriteRenderer>().color = MySnake.GetComponent<SpriteRenderer>().color;
    }


    private Vector3 movementVelocity;
    [Range(0.0f, 1.0f)]
    public float overTime = 0.5f;

    //Reception des messages
    private void Update()
    {
        // Le boutton essaye tjr de retourner a sa pos init (=tuto, on peut améliorer, a retravailler)
        transform.position = Vector3.SmoothDamp(transform.position,
               initPos,
               ref movementVelocity,
               overTime);

        GetComponent<SpriteRenderer>().color = MySnake.GetComponent<SpriteRenderer>().color;

        if (MySnake.GetComponent<SnakeMovements>().partnerAv == null)
        {
            switch (MyRole)
            {
                case "L":
                    gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    gameObject.GetComponent<Collider2D>().enabled = true;
                    break;
                case "R":
                    gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    gameObject.GetComponent<Collider2D>().enabled = true;
                    break;
                case "U":
                    gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    gameObject.GetComponent<Collider2D>().enabled = false;
                    break;
                case "D":
                    gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    gameObject.GetComponent<Collider2D>().enabled = false;
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (MyRole)
            {
                case "L":
                    gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    gameObject.GetComponent<Collider2D>().enabled = false;
                    break;
                case "R":
                    gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    gameObject.GetComponent<Collider2D>().enabled = false;
                    break;
                case "U":
                    gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    gameObject.GetComponent<Collider2D>().enabled = true;
                    break;
                case "D":
                    gameObject.GetComponent<SpriteRenderer>().enabled = true;
                    gameObject.GetComponent<Collider2D>().enabled = true;
                    break;
                default:
                    break;
            }
        }
    }

    void OnTouchDown()
    {
        //On regarde quel Move doit faire le button ( en fonction de son role)
        switch (MyRole)
        {
            case "L":
                MySnake.GetComponent<SnakeMovements>().RotateLeft();
                break;
            case "R":
                MySnake.GetComponent<SnakeMovements>().RotateRight();
                break;
            case "U":
                MySnake.GetComponent<SnakeMovements>().SpeedUp();
                break;
            case "D":
                MySnake.GetComponent<SnakeMovements>().SlowDown();
                break;
            default:
                break;
        }
    }

    void OnTouchUp()
    {
        switch (MyRole)
        {
            case "L":
                break;
            case "R":
                break;
            case "U":
                MySnake.GetComponent<SnakeMovements>().ResetVitesse();
                break;
            case "D":
                MySnake.GetComponent<SnakeMovements>().ResetVitesse();
                break;
            default:
                break;
        }
    }

    void OnTouchStay()
    {
        switch (MyRole)
        {
            case "L":
                MySnake.GetComponent<SnakeMovements>().RotateLeft();
                break;
            case "R":
                MySnake.GetComponent<SnakeMovements>().RotateRight();
                break;
            case "U":
                MySnake.GetComponent<SnakeMovements>().SpeedUp();
                break;
            case "D":
                MySnake.GetComponent<SnakeMovements>().SlowDown();
                break;
            default:
                break;
        }
    }

    void OnTouchExit()
    {
        switch (MyRole)
        {
            case "L":
                break;
            case "R":
                break;
            case "U":
                MySnake.GetComponent<SnakeMovements>().ResetVitesse();
                break;
            case "D":
                MySnake.GetComponent<SnakeMovements>().ResetVitesse();
                break;
            default:
                break;
        }
    }

}
