using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

	void Start () {
        StartCoroutine("Delete");
	}


    IEnumerator Delete()
    {
        yield return new WaitForSeconds(10);
        Destroy(this.gameObject);
    }
}
