using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveController : MonoBehaviour {

	public GameObject right, left, up, down;
	public float disRight, disLeft, disUp, disDown;

	public bool VeryDown = true;

	void Start()
	{
		RaycastHit2D[] r = Physics2D.LinecastAll ((Vector2)transform.position, (Vector2)transform.position + (Vector2.right * 300), 768);
		RaycastHit2D[] l = Physics2D.LinecastAll ((Vector2)transform.position, (Vector2)transform.position + (Vector2.left * 300), 768);
		RaycastHit2D[] d = Physics2D.LinecastAll ((Vector2)transform.position, (Vector2)transform.position + (Vector2.down * 300), 768);
		RaycastHit2D[] u = Physics2D.LinecastAll ((Vector2)transform.position, (Vector2)transform.position + (Vector2.up * 300), 768);

		if (r.Length > 1) 
		{
			if (r [1].transform.tag != "Wall") 
			{
				right = r [1].transform.gameObject;
				disRight = Mathf.Abs(Vector2.Distance (r [1].transform.position, transform.position));
			}
			else
				disRight = float.PositiveInfinity;
		}
		if (l.Length > 1) 
		{
			if (l [1].transform.tag != "Wall") 
			{
				left = l [1].transform.gameObject;
				disLeft = Mathf.Abs(Vector2.Distance (l [1].transform.position, transform.position));
			}
			else
				disLeft = float.PositiveInfinity;
		}
		if (d.Length > 1) 
		{
			if (d [1].transform.tag != "Wall") 
			{
				down = d [1].transform.gameObject;
				disDown = Mathf.Abs(Vector2.Distance (d [1].transform.position, transform.position));
			}
			else
				disDown = float.PositiveInfinity;
		}
		if (u.Length > 1) 
		{
			if (u [1].transform.tag != "Wall") {
				up = u [1].transform.gameObject;
				disUp = Mathf.Abs (Vector2.Distance (u [1].transform.position, transform.position));
			} else
				disUp = float.PositiveInfinity;
		}

		//Destroy (this.GetComponent<BoxCollider2D> ());
	}

	public List<GameObject> options()
	{
		List<GameObject> array = new List<GameObject> ();

		if (right)
			array.Add (right);
		if (left)
			array.Add (left);
		if (up)
			array.Add (up);
		if (down)
			array.Add (down);
		return array;
	}
}
