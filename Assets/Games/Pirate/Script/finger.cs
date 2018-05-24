using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finger : MonoBehaviour {

    public GameObject boat;

	void Start () {
        this.transform.position = new Vector3(boat.transform.position.x, boat.transform.position.y + 1, boat.transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {
        if (boat != null)
            this.transform.position = new Vector3(boat.transform.position.x, boat.transform.position.y + 1, boat.transform.position.z);
        else
            Destroy(this.gameObject);
        this.gameObject.SetActive(boat.GetComponent<Pirate>().enabled);
    }
}
