using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulet : MonoBehaviour {


    void OnTriggerEnter2D(Collider2D other)
    {
        if (this.tag != "BouletPirate")
        {
            if (other.gameObject.tag != "HelpP" && other.gameObject.tag != "ZoneTouch" && other.gameObject.tag != "FireButton" && other.gameObject.tag != "HealButton" && other.gameObject.tag != "EventCollider" && other.gameObject.tag != "Help")
            {
                Destroy(gameObject);
            }
        }
        else {
            if (other.gameObject.tag != "HelpP" && other.gameObject.tag != "ZoneTouch" && other.gameObject.tag != "FireButton" && other.gameObject.tag != "HealButton" && other.gameObject.tag != "EventCollider" && other.gameObject.tag != "Help" && other.gameObject.tag != "faiblesse")
            {
                Destroy(gameObject);
            }
        }
    }
}
