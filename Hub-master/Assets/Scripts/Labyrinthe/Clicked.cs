using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Clicked{

    public static bool BeClicked(GameObject GO)
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (raycastHit.collider != null)
            {

                if (raycastHit.collider.Equals(GO.GetComponent<Collider2D>()))
                {
                    return true;
                }
            }

        }

        
        if ((Input.touchCount > 0))
        {

            for (int i = 0; i <= Input.touchCount; i++)
            {
                RaycastHit2D raycastHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), Vector2.zero);
            
                if (raycastHit.collider != null)
                {
					if (raycastHit.collider.tag.Equals(GO.tag))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}
