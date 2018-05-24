using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(MeshCollider))]
[System.Serializable]
public class Boundary {
	public float xMin, xMax, yMin, yMax;
}

public class ControlsTeam1 : MonoBehaviour {

    public GameObject Player;
	public Boundary boundary;
    float speed;
    public GameObject limitOther;

    private Rigidbody2D rb2D;
    private Vector3 positionOfScreen;
    private Vector3 offsetValue;

    private bool isDragging;
    private int draggingTouch;

    // Use this for initialization
    void Start () {
		rb2D = Player.GetComponent<Rigidbody2D> ();
        Input.multiTouchEnabled = true;
        setSpeed(1);
	}
	
	// Update is called once per frame
	void Update () {
		if ((Input.touchCount > 0)) {
			for (int i = 0; i <= Input.touchCount; i++) {
				RaycastHit2D raycastHit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.touches[i].position), Vector2.zero);

				if (raycastHit.collider != null) {
					switch (raycastHit.collider.tag) {
					case "F_Up":
						MoveUp ();
						break;
					case "F_Down":
						MoveDown ();
						break;
					case "F_Right":
						MoveRight ();
						break;
					case "F_Left":
						MoveLeft ();
						break;
					case "B_Move":
                            if (Input.touches[i].phase.Equals(TouchPhase.Began) && !isDragging)
                            {
                                isDragging = true;
                                draggingTouch = Input.touches[i].fingerId;
                                positionOfScreen = Camera.main.WorldToScreenPoint(this.transform.position);
                                offsetValue = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[i].position.x, Input.touches[i].position.y, positionOfScreen.z));
                            }
						break;
					default:
						break;
					}


                    if (Input.touches[i].fingerId == draggingTouch)
                    {
                        if (Input.touches[i].phase.Equals(TouchPhase.Ended))
                        {
                            isDragging = false;
                        }

                        if (isDragging)
                        {
                            Vector3 currentScreenSpace = new Vector3(Input.touches[i].position.x, Input.touches[i].position.y, positionOfScreen.z);

                            //converting screen position to world position with offset changes.
                            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offsetValue;

                            //It will update target gameobject's current postion.
                            this.transform.position = currentPosition;
                        }
                    }
                }
			}
		}

        Physics2D.IgnoreLayerCollision(8, 9);
        Physics2D.IgnoreLayerCollision(8, 10);
        Physics2D.IgnoreLayerCollision(8, 12);
        Physics2D.IgnoreLayerCollision(8, 13);
    }

	void MoveUp() {
        Vector2 movement = (new Vector2(0, 1));
        rb2D.AddForce(movement * speed, ForceMode2D.Impulse);
    }

	void MoveDown() {
        Vector2 movement = (new Vector2(0, -1));
        rb2D.AddForce(movement * speed, ForceMode2D.Impulse);
    }

	void MoveRight() {
        Vector2 movement = (new Vector2(1, 0));
        rb2D.AddForce(movement * speed, ForceMode2D.Impulse);
    }

	void MoveLeft() {
        Vector2 movement = (new Vector2(-1, 0));
        rb2D.AddForce(movement * speed, ForceMode2D.Impulse);
    }

    public void setSpeed( int s)
    {
        this.speed = s;
    }
}
