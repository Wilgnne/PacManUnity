using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class MoveController : MonoBehaviour 
{
	Rigidbody2D _rb;
	Animator _anim;


	[System.NonSerialized]
	public Vector2 _dir;

	bool move, right, left, up, down;

	public float speed;
	public bool hit;

	void Awake()
	{
		Rays ();
	}

	// Use this for initialization
	void Start () 
	{
		move = true;
		_rb = GetComponent<Rigidbody2D> ();	
		_anim = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () 
	{
		if ((_dir == Vector2.up && !up) || (_dir == Vector2.down && !down) || (_dir == Vector2.left && !left) || (_dir == Vector2.right && !right)) 
		{
			_dir = Vector2.zero;
		}
	}

	public void LateUpdate ()
	{
		attHit ();
		if (move && _dir != Vector2.zero) 
		{
			_rb.MovePosition (_rb.position + _dir);
			StartCoroutine ("Move");
		}
	}

	public void attHit()
	{
		//hit = _anim.GetCurrentAnimatorStateInfo (0).IsName ("death");
	}

	public void Up ()
	{
		if (up && _dir != Vector2.up) 
		{
			_anim.SetTrigger ("up");
			_dir = Vector2.up;
			StopCoroutine ("Move");
			move = true;
		}
	}
	public void Down ()
	{
		if (down && _dir != Vector2.down) 
		{
			_anim.SetTrigger ("down");
			_dir = Vector2.down;
			StopCoroutine ("Move");
			move = true;
		}
	}
	public void Right ()
	{
		if (right && _dir != Vector2.right) 
		{
			_anim.SetTrigger ("right");
			_dir = Vector2.right;
			StopCoroutine ("Move");
			move = true;
		}
	}
	public void Left ()
	{
		if (left && _dir != Vector2.left) 
		{
			_anim.SetTrigger ("left");
			_dir = Vector2.left;
			StopCoroutine ("Move");
			move = true;
		}
	}

	IEnumerator Move()
	{
		move = false;
		yield return new WaitForSeconds (1 / speed);
		move = true;
	}

	void DrawLine()
	{
		Debug.DrawLine ((Vector2)transform.position, (Vector2)transform.position + (Vector2.right*1.5f));
		Debug.DrawLine ((Vector2)transform.position, (Vector2)transform.position + (Vector2.left*1.5f));
		Debug.DrawLine ((Vector2)transform.position, (Vector2)transform.position + (Vector2.down*1.5f));
		Debug.DrawLine ((Vector2)transform.position, (Vector2)transform.position + (Vector2.up*1.5f));
	}

	public void Rays()
	{
		right = !Physics2D.Linecast ((Vector2)transform.position, (Vector2)transform.position + (Vector2.right * 1.5f), 1 << LayerMask.NameToLayer ("Wall"));
		left = !Physics2D.Linecast ((Vector2)transform.position, (Vector2)transform.position + (Vector2.left * 1.5f), 1 << LayerMask.NameToLayer ("Wall"));
		down= !Physics2D.Linecast ((Vector2)transform.position, (Vector2)transform.position + (Vector2.down * 1.5f), 1 << LayerMask.NameToLayer ("Wall"));
		up = !Physics2D.Linecast ((Vector2)transform.position, (Vector2)transform.position + (Vector2.up * 1.5f), 1 << LayerMask.NameToLayer ("Wall"));
		DrawLine ();
	}
}
