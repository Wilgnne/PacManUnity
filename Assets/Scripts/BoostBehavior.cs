using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "PacMan") 
		{
			foreach (GameObject ob in GameObject.FindGameObjectsWithTag("ghost")) 
			{
				ob.GetComponent<Animator> ().SetTrigger ("death");
			}
			Destroy (gameObject);
		}
	}
}