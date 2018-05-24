using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag_DropTouch : MonoBehaviour {

    public GameObject ball;

    private Rigidbody2D rb2D;
    public float speed;
    private bool clicked = false;

    private int touchId;



    void Start()
    {
        rb2D = ball.GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        transform.position = rb2D.position; //la zone de click suis la boule

        Touch[] myTouches = Input.touches;
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch myTouch = Input.GetTouch(i);
            Vector2 touchPos = (Vector2)Camera.main.ScreenToWorldPoint(myTouch.position); // position du touch
            RaycastHit2D hit = Physics2D.Raycast(touchPos, -Vector2.up);

            if (hit.collider != null && !clicked)
            {
                switch (hit.collider.tag)
                {
                    case "Joueur":
                        switch (myTouch.phase)
                        {
                            case TouchPhase.Began:
                                touchId = myTouch.fingerId;
                                clicked = true;
                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }
            }
            if (myTouch.fingerId == touchId)
            {
                switch (myTouch.phase)
                {
                    case TouchPhase.Ended:
                        clicked = false;
                        break;

                    default:
                        break;
                }

                if (clicked) AppliqueForce(touchPos);
            }
        }
    }

    void AppliqueForce(Vector2 pos)
    {
        Vector2 force = new Vector2(pos.x - rb2D.position.x, pos.y - rb2D.position.y);
        rb2D.AddForce(Vector2.ClampMagnitude(force, 0.75f) * speed, ForceMode2D.Impulse);
    }

    public void goTo(Vector3 position)
    {
        transform.position = position;
        ball.transform.position = position;
    }
}
