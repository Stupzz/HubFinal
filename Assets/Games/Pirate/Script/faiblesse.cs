using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class faiblesse : MonoBehaviour {

    public Pirate pirate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Boulet") pirate.touchFaiblesse();
    }
}
