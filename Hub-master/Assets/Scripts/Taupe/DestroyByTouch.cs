using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTouch : MonoBehaviour {

    private Animator animator;
    private GameController gameController;

    private int scoreValue;
    public int life;

    private void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameControllerObject == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }

        animator = GetComponent<Animator>();
        if( Mathf.FloorToInt(Random.Range(0, 10)) <= 2)
        {
            life = 2;
            scoreValue = 2;
            animator.SetBool("Armored", true);
            //GetComponent<Renderer>().material = ArmureMat;
        }
        else
        {
            life = 1;
            scoreValue = 1;
            animator.SetBool("Armored", false);
            //GetComponent<Renderer>().material = NormalMat;
        }
        
        animator.SetInteger("Life", life);
    }

    void Update()
    {

        Touch[] myTouches = Input.touches;
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch myTouch = Input.GetTouch(i);
            Vector2 touchPos = (Vector2)Camera.main.ScreenToWorldPoint(myTouch.position); // position du touch
            RaycastHit2D hit = Physics2D.Raycast(touchPos, -Vector2.up);

            if (hit.collider != null)
            {
                switch (hit.collider.tag)
                {
                    case "Cible":
                        switch (myTouch.phase)
                        {
                            case TouchPhase.Began:
                                if (hit.collider.Equals(this.GetComponent<Collider2D>())) 
                                    StartCoroutine(Touched());
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

        /*
        //if(Input.GetMouseButtonDown(0))
        if ((Input.touchCount > 0))
        {
            print("touched");

            for (int i = 0; i <= Input.touchCount; i++)
            {
                RaycastHit2D raycastHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), Vector2.zero);
                //RaycastHit2D raycastHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (raycastHit.collider != null)
                {
                    print("hitted");
                    print(raycastHit.collider.name);
                    print(gameObject.GetComponent<Collider2D>().name);

                    if (raycastHit.collider.Equals(this.GetComponent<Collider2D>()))
                        {
                            print("Cible Clicked");
                            StartCoroutine(Touched());
                        }
                }
            }

        }*/

    }

    IEnumerator Touched()
    {
        print(life);
        life = life - 1;
        print("After life " + life);
        if (life <= 0)
        {
            animator.SetInteger("Life", life);
            animator.SetTrigger("Hitted");
            yield return new WaitForSeconds((float)0.5);
            Destroy(this.gameObject);
            gameController.AddScore(scoreValue);
        }
        else if(life >= 1)
        {
            animator.SetInteger("Life", life);
            animator.SetTrigger("Hitted");
            yield return new WaitForSeconds(0);
        }
    }

}
