using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTouchScriptP2 : MonoBehaviour {

    public float scale;
    public float speed;

    public GameObject arrowUp;
    public GameObject arrowDown;
    public GameObject arrowLeft;
    public GameObject arrowRight;

    private Rigidbody2D rb2D;

    private Vector2 force = new Vector2(0, 0);
    private Vector2 position;
    private int touchId;

    // Use this for initialization
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        touchId = -1;
        MajArrow();
    }

    // Update is called once per frame
    void Update()
    {
        if (touchId == -1) MajArrow();
        rb2D.AddForce(force * speed, ForceMode2D.Impulse);

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch myTouch = Input.GetTouch(i);
            Vector2 touchPos = (Vector2)Camera.main.ScreenToWorldPoint(myTouch.position); // position du touch
            RaycastHit2D hit = Physics2D.Raycast(touchPos, -Vector2.up);

            if (hit.collider != null && touchId == -1) // touchID -1 quand rien aucuns touch touchent des flèches.
            {
                switch (hit.collider.tag)
                {
                    case "ArrowUp2":
                        switch (myTouch.phase)
                        {
                            case TouchPhase.Began:
                                touchId = myTouch.fingerId;
                                force = new Vector2(0, 5);
                                break;

                            default:
                                break;
                        }
                                
                        break;

                    case "ArrowDown2":
                        switch (myTouch.phase)
                        {
                            case TouchPhase.Began:
                                touchId = myTouch.fingerId;
                                force = new Vector2(0, -5);
                                break;

                            default:
                                break;
                        }
                          
                        break;

                    case "ArrowLeft2":
                        switch (myTouch.phase)
                        {
                            case TouchPhase.Began:
                                touchId = myTouch.fingerId;
                                force = new Vector2(-5, 0);
                                break;

                            default:
                                break;
                        }
                               
                        break;

                    case "ArrowRight2":
                        switch (myTouch.phase)
                        {
                            case TouchPhase.Began:
                                touchId = myTouch.fingerId;
                                force = new Vector2(5, 0);
                                break;

                            default:
                                break;
                        }
                   
                        break;

                    default:
                        break;
                }
            }
            if (myTouch.fingerId == touchId) // application de force quand le touch est effectué sur une des flcèhes.
            {
                switch (myTouch.phase)
                {
                    case TouchPhase.Ended:
                        touchId = -1;
                        force = new Vector2(0, 0);
                        break;

                    default:
                        break;
                }
            }
        }
    }

    private void MajArrow()
    {
        position.Set(rb2D.position.x, rb2D.position.y + (2 * scale));
        arrowUp.transform.position = position;

        position.Set(rb2D.position.x - (2 * scale), rb2D.position.y);
        arrowLeft.transform.position = position;

        position.Set(rb2D.position.x + (2 * scale), rb2D.position.y);
        arrowRight.transform.position = position;

        position.Set(rb2D.position.x, rb2D.position.y - (2 * scale));
        arrowDown.transform.position = position;
    }
}
