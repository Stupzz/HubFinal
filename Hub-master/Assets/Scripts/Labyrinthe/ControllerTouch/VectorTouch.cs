using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorTouch : MonoBehaviour {

    public GameObject ball;

    private Rigidbody2D rb2D;
    public float speed;
    private bool relacher = false;
    private bool clicked = false;
    private int touchId;

    Vector2 vectorZero = new Vector2(0f, 0f);
    Vector2 posDepart;
    Vector2 posArriver;

    // Use this for initialization
    void Start()
    {
        posDepart = vectorZero;
        posArriver = vectorZero;
        rb2D = ball.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        appliqueForce();
        transform.position = rb2D.position; //la zone de click suis la boule


        Touch[] myTouches = Input.touches;
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch myTouch = Input.GetTouch(i);
            Vector2 touchPos = (Vector2)Camera.main.ScreenToWorldPoint(myTouch.position); // position du touch
            RaycastHit2D hit = Physics2D.Raycast(touchPos, -Vector2.up);

            if (hit.collider != null)
            {
                if (clicked)
                {
                    if (myTouch.fingerId == touchId)
                    {
                        switch (hit.collider.tag)
                        {
                            case "Joueur":

                                switch (myTouch.phase)
                                {
                                    case TouchPhase.Ended:
                                        clicked = false;
                                        relacher = true;
                                        posArriver = (Vector2)Camera.main.ScreenToWorldPoint(myTouch.position);
                                        break;

                                    default:
                                        break;
                                }
                                break;

                            default:
                                if (clicked) // test si clicked, eviter de remettre une posArriver si elle a déjà été mit avec l'exit
                                {
                                    // Debug.Log("Sort de la zone de hit");
                                    clicked = false;
                                    relacher = true;
                                    posArriver = (Vector2)Camera.main.ScreenToWorldPoint(myTouch.position);
                                }
                                break;
                        }
                    }
                }
                else
                {
                    switch (hit.collider.tag)
                    {
                        case "Joueur":
                            switch (myTouch.phase)
                            {
                                case TouchPhase.Began:
                                    touchId = myTouch.fingerId;
                                    clicked = true;
                                    posDepart = (Vector2)Camera.main.ScreenToWorldPoint(myTouch.position);
                                    break;
                                default:
                                    break;
                            }
                            break;

                        default:
                            break;
                    }
                }
            }

            else
            {
                if (clicked && touchId == myTouch.fingerId) // test si clicked, eviter de remettre une posArriver si elle a déjà été mit avec l'exit
                {
                    //Debug.Log("Sort de la zone de hit");
                    clicked = false;
                    relacher = true;
                    posArriver = (Vector2)Camera.main.ScreenToWorldPoint(myTouch.position);
                }
            }
        }
    }

    void appliqueForce()
    {
        if (relacher)
        {
            Vector2 force = new Vector2(posArriver.x - posDepart.x, posArriver.y - posDepart.y);
            rb2D.AddForce(force * speed, ForceMode2D.Impulse);
            relacher = false;
            posDepart = vectorZero;
            posArriver = vectorZero;
        }
    }
}