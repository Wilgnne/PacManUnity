using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotDestoy : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "PacMan") 
		{
			FindObjectOfType<GameController> ().points += 10;
			Destroy (gameObject);
		}
	}
}
