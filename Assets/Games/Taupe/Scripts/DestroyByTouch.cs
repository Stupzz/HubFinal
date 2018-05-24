using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTouch : MonoBehaviour
{

    private Animator animator;
    private GameController gameController;

    private int scoreValue;
    public int life;

    private float touchPrecedent = 0;

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
        if (Mathf.FloorToInt(Random.Range(0, 10)) <= 2)
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
        touchPrecedent -= Time.deltaTime;
        if (Tactil.TouchedSouris(GetComponent<Collider2D>()) && touchPrecedent < 0) StartCoroutine(Touched());

        /*Touch[] myTouches = Input.touches;
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch myTouch = Input.GetTouch(i);
            Vector2 touchPos = (Vector2)Camera.main.ScreenToWorldPoint(myTouch.position); // position du touch
            RaycastHit2D hit = Physics2D.Raycast(touchPos, -Vector2.zero);

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
        }*/
    }


    IEnumerator Touched()
    {
        touchPrecedent = 0.5f;
        print(life);
        life--;
        print("After life " + life);
        if (life <= 0)
        {
            animator.SetInteger("Life", life);
            animator.SetTrigger("Hitted");
            yield return new WaitForSeconds((float)0.5);
            Destroy(this.gameObject);
            gameController.AddScore(scoreValue);
        }
        else if (life >= 1)
        {
            animator.SetInteger("Life", life);
            animator.SetTrigger("Hitted");
            yield return new WaitForSeconds(0.5f);
        }
    }
}

