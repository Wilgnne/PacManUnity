using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveController))]
public class PlayerBehaviour : MonoBehaviour 
{
	MoveController _move;

	public GameObject[] disbleOnDeath;

	// Use this for initialization
	void Start ()
	{
		_move = GetComponent<MoveController> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		_move.Rays ();
		if (Input.GetAxisRaw ("Horizontal") > 0) 
		{
			_move.Right ();
		}
		else if (Input.GetAxisRaw ("Horizontal") < 0) 
		{
			_move.Left ();
		} 
		else if (Input.GetAxisRaw ("Vertical") > 0) 
		{
			_move.Up ();
		} 
		else if (Input.GetAxisRaw ("Vertical") < 0) 
		{
			_move.Down ();
		}
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		GameController gc = FindObjectOfType<GameController> ();
		if (col.tag == "ghost") 
		{
			if (col.GetComponent<MoveController> ().hit) {
				if (gc.ghost < 16)
					gc.ghost *= 2;
				else
					gc.ghost = 1;
				
				gc.points += 100*gc.ghost;

				col.GetComponent<GhostIA> ().Return ();
			}
			else 
			{
				_move.enabled = false;
				foreach (GameObject ob in disbleOnDeath) 
				{
					ob.SetActive (false);
				}
				GetComponent<Animator> ().SetTrigger ("death");
				this.enabled = false;
			}
		}
	}
}