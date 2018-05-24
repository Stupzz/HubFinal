using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tactil {

    public static bool touchCollider(Collider2D col)
    {
        Touch[] myTouches = Input.touches;
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch myTouch = Input.GetTouch(i);
            Vector2 touchPos = (Vector2)Camera.main.ScreenToWorldPoint(myTouch.position); // position du touch
            RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

            if ((hit.collider != null && hit.collider == col) && myTouch.phase == TouchPhase.Began)
            {
                return true;
            }
        }
        return false;
    }

    public static bool stillTouchedCollider(Collider2D col)
    {
        Touch[] myTouches = Input.touches;
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch myTouch = Input.GetTouch(i);
            Vector2 touchPos = (Vector2)Camera.main.ScreenToWorldPoint(myTouch.position); // position du touch
            RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

            if (hit.collider != null && hit.collider == col)
            {
                return true;
            }
        }
        return false;
    }

    public static bool TouchedSouris(Collider2D col)
    {
        if (Tactil.touchCollider(col) || Souris.collide(col)) return true;
        else return false;
    }
}
