using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

    public Canon canon;
    private CircleCollider2D col;
	void Start () {
        col = GetComponent<CircleCollider2D>();
        //StartCoroutine("Pause");
        //enabled = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (Tactil.TouchedSouris(col) && this.gameObject.tag == "FireButton") canon.fire(); 
        if (Tactil.TouchedSouris(col) && this.gameObject.tag == "HealButton") canon.heal(); 
    }

    /*IEnumerator Pause()
    {
        yield return new WaitForSeconds(3);
        enabled = true;
    }*/
}
