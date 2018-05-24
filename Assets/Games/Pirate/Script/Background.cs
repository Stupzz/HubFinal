using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    private Animator anim;

	void Start () {
        anim = GetComponent<Animator>();
        //StartCoroutine("Pause");
	}

    /*IEnumerator Pause()
    {
        yield return new WaitForSeconds(3);
        anim.SetBool("animStarted", true);
    }*/

    public void setAnim(bool active)
    {
        anim.SetBool("animStarted", active);
    }
}
