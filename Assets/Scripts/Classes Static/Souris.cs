using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Souris {

	public static bool collide(Collider2D col)
    {
        Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition); // position du touch
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (Input.GetMouseButtonDown(0) && (hit.collider != null && hit.collider == col))
        {
            return true;
        }
        else return false;
    }

    public static void testCollider()
    {
        Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition); // position du touch
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (Input.GetMouseButtonDown(0) && hit.collider != null)
        {
            Debug.Log("L'objet touché est : " + hit.collider.gameObject);
        }
        else if (Input.GetMouseButtonDown(0) && hit.collider == null)
        {
            Debug.Log("aucun objet touché");
        }
    }
}
