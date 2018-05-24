using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMouvements : MonoBehaviour {

	public float speed;
	private Rigidbody2D rb2;
	 

	// Use this for initialization
	void Start () {
		GameObject Boule = GameObject.Find("BouleRouge");
		rb2 = GetComponent<Rigidbody2D>();
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		Vector2 movement = new Vector2 (moveHorizontal, moveVertical);
		rb2.AddForce (movement * speed, ForceMode2D.Impulse);


		


	}
}
