using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour {

    public LayerMask touchInputMask; //Layer ou on regarde les touch (On peut peut etre l'enlever pour regarder tt les layers)

    private List<GameObject> touchList = new List<GameObject>(); //Tableau des Objets touchés a cette frame
    private GameObject[] touchesOld;                             //Tableau des Objets touché la frame précédente

    private RaycastHit2D hit; //Hit pour les raycast 

	// Update is called once per frame
	void Update () {

//Code Appliquer dans l'équiteur (== Mouse) Similaire au code normal commenter en dessous ----------------------------
#if UNITY_EDITOR
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0)) 
        {

            touchesOld = new GameObject[touchList.Count];
            touchList.CopyTo(touchesOld);
            touchList.Clear();

            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); 

            if (hit.collider != null)
            {
                GameObject recipient = hit.transform.gameObject;
                touchList.Add(recipient);

                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Touched", recipient);
                    recipient.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
                }
                if (Input.GetMouseButtonUp(0))
                {
                    recipient.SendMessage("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
                }
                if (Input.GetMouseButton(0))
                {
                    recipient.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
                }
            }

            foreach (GameObject g in touchesOld)
            {
                if (!touchList.Contains(g))
                {
                    g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                }
            }
            
        }
     
#endif 
//--------------------------------------------------------------------------------------------------------------------

        if(Input.touchCount > 0)
        {

            touchesOld = new GameObject[touchList.Count]; 
            touchList.CopyTo(touchesOld);                   //On met a jour les Objet de la frame prèc
            touchList.Clear();                              //On vide la list des Objets de cette frame

            foreach(Touch touch in Input.touches)
            {

                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(touch.position), Vector2.zero, touchInputMask); // 2D trés Important sinon on peut pas détecter des sprites c'est chelou

                if (hit.collider != null)
                {

                    GameObject recipient = hit.transform.gameObject;    //Objet touché
                    touchList.Add(recipient);                           //Ajout de l'objet touché a la liste des objets touchés

                    //Envoie de message a l'objet en fonction de la phase du touch qu'on regarde
                    if(touch.phase == TouchPhase.Began)
                    {
                        recipient.SendMessage("OnTouchDown", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (touch.phase == TouchPhase.Ended)
                    {
                        recipient.SendMessage("OnTouchUp", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    {
                        recipient.SendMessage("OnTouchStay", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                    if (touch.phase == TouchPhase.Canceled)
                    {
                        recipient.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }

            //On regarde si des objets touché la frame d'avant ne le sont plus cette frame (=Le touch est tjr actif mais il touch plus l'objet )
            foreach (GameObject g in touchesOld)
            {
                if (!touchList.Contains(g))
                {
                    g.SendMessage("OnTouchExit", hit.point, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
	}
}
