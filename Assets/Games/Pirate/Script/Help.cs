using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Help : MonoBehaviour {

    private void Start()
    {
        if (tag == "HelpP")
        {
            transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z - 90);
        }
        else if (transform.position.y > 0)
        {
            transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z + 180);
        }
    }

    // Update is called once per frame
    void Update () {
        this.transform.localScale += new Vector3(0.01f, 0.01f, 0f);
        if (transform.localScale.x >= 2.3 && tag != "HelpP")
        {
            Destroy(this.gameObject);
        }
        else if (tag == "HelpP")
            {
                if (transform.localScale.x >= 3) Destroy(this.gameObject);
            }
    }
}
