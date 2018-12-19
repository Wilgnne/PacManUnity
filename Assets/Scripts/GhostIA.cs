using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveController))]
public class GhostIA : MonoBehaviour 
{
	public enum Behaviours
	{
		Random, Follower, PreviewFollower, RandomFollower
	};

	MoveController _move;
	Animator _anim;

	public GameObject spawn;

	public GameObject attPos;
	public GameObject nextPos;

	public GameObject _originPos;

	public Behaviours behaviour;

	public RuntimeAnimatorController ghostController;
	public RuntimeAnimatorController eyesController;

	public float ghostSpeed;
	public float eyesSpeed;

	GameObject target;
	float time = 3f;
	float attTime = 4f;
	bool inReturn = false;

	public bool tamoEmFuga;


	// Use this for initialization
	void Start ()
	{
		_move = GetComponent<MoveController> ();
		_move.speed = ghostSpeed;
		_anim = GetComponent<Animator> ();
		target = GameObject.FindGameObjectWithTag ("PacMan");
	}

	void Update ()
	{
		if (transform.position == spawn.transform.position && 
				_anim.runtimeAnimatorController == eyesController) 
		{
			Spawn ();
			inReturn = false;
		}
		
		if (!inReturn) 
		{
			if (!_move.hit) {
				if (behaviour == Behaviours.Random)
					RamdomMoviment ();
				else if (behaviour == Behaviours.Follower)
					Follow (target, Vector2.zero);
				else if (behaviour == Behaviours.PreviewFollower)
					Follow (target, target.GetComponent<MoveController> ()._dir * 32);
				else if (behaviour == Behaviours.RandomFollower)
					Follow (target, target.GetComponent<MoveController> ()._dir * 6 +
					new Vector2 (Random.Range (-6, 7), Random.Range (-6, 7)));
			} 
			else 
			{
				Escape (target, attPos.GetComponent<CurveController> ());
			}
		} 
		else 
		{
			Follow (spawn, Vector2.zero);
		}
	}

	public void eventHit()
	{
		_move.hit = true;

	}

	public void falseHit()
	{
		_move.hit = false;
	}

	public void Return ()
	{
		_move.speed = eyesSpeed;
		_anim.runtimeAnimatorController = eyesController;
		_move.attHit ();
		inReturn = true;
	}

	public void Spawn ()
	{
		_move.speed = ghostSpeed;
		_anim.runtimeAnimatorController = ghostController;
		_move.hit = false;
	}

	public void Escape (GameObject target, CurveController _attPos)
	{
		RaycastHit2D hit = Physics2D.Linecast (transform.position, target.transform.position, 
			                   1 << LayerMask.NameToLayer ("Wall"));

		if (!hit) 
		{
			if (hit.distance < 3.5f)
				attTime = 0;
		}

		if (attTime < time) 
		{
			tamoEmFuga = true;
			Vector2 pos = (Vector2)target.transform.position;

			if (transform.position == attPos.transform.position) 
			{
				Scoot (pos, attPos.GetComponent<CurveController> ());
			}

			if (transform.position == nextPos.transform.position) 
			{
				_originPos = attPos;
				attPos = nextPos;
				Scoot (pos, attPos.GetComponent<CurveController> ());
			}

			Debug.DrawLine (transform.position, target.transform.position, Color.yellow);
			attTime += Time.deltaTime;
		}
		else 
		{
			tamoEmFuga = false;
			Debug.DrawLine (transform.position, target.transform.position, Color.white);
			RamdomMoviment ();
		}

		Debug.DrawLine (transform.position, nextPos.transform.position, Color.red);
		if (_originPos)
			Debug.DrawLine (transform.position, _originPos.transform.position, Color.green);
		Debug.DrawLine (transform.position, attPos.transform.position, Color.blue);
	}

	public void Scoot (Vector2 target, CurveController _attPos)
	{
		_move.Rays ();
		Vector2 relative = target - (Vector2)transform.position;
		if (Mathf.Abs (relative.x) > Mathf.Abs (relative.y)) 
		{
			if (relative.x > 0 && _attPos.left) 
			{
				nextPos = _attPos.left;
				_move.Left ();
			} 
			else if (relative.x < 0 && _attPos.right) 
			{
				nextPos = _attPos.right;
				_move.Right ();
			} 
			else if (relative.y > 0 && _attPos.down && (_attPos.VeryDown || inReturn)) 
			{
				nextPos = _attPos.down;
				_move.Down ();
			}
			else if (relative.y < 0 && _attPos.up) 
			{
				nextPos = _attPos.up;
				_move.Up ();
			}
			else
				RamdomNewPos (_attPos);
		}
		else 
		{
			if (relative.y > 0 && _attPos.up) 
			{
				nextPos = _attPos.up;
				_move.Up ();
			}
			else if (relative.y < 0 && _attPos.down && (_attPos.VeryDown || inReturn)) 
			{
				nextPos = _attPos.down;
				_move.Down ();
			}
			else if (relative.x > 0 && _attPos.right) 
			{
				nextPos = _attPos.right;
				_move.Right ();
			} 
			else if (relative.x < 0 && _attPos.left) 
			{
				nextPos = _attPos.left;
				_move.Left ();
			}
			else
				RamdomNewPos (_attPos);
		}
	}

	public void Follow (GameObject target, Vector2 distocion)
	{
		RaycastHit2D hit = Physics2D.Linecast (transform.position, target.transform.position, 
							1 << LayerMask.NameToLayer ("Wall"));
		if (!hit) 
		{
			if (hit.distance < 2.5f)
				attTime = 0;
		}

		if (attTime < time) 
		{
			Vector2 pos = (Vector2)target.transform.position + distocion;

			if (transform.position == attPos.transform.position) 
			{
				ChoiceDirection (pos, attPos.GetComponent<CurveController> ());
			}

			if (transform.position == nextPos.transform.position) 
			{
				_originPos = attPos;
				attPos = nextPos;
				ChoiceDirection (pos, attPos.GetComponent<CurveController> ());
			}

			Debug.DrawLine (transform.position, target.transform.position, Color.yellow);
			attTime += Time.deltaTime;
		}
		else 
		{
			Debug.DrawLine (transform.position, target.transform.position, Color.white);
			RamdomMoviment ();
		}

		Debug.DrawLine (transform.position, nextPos.transform.position, Color.red);
		if (_originPos)
			Debug.DrawLine (transform.position, _originPos.transform.position, Color.green);
		Debug.DrawLine (transform.position, attPos.transform.position, Color.blue);
	}

	public void ChoiceDirection (Vector2 target, CurveController _attPos)
	{
		_move.Rays ();
		Vector2 relative = target - (Vector2)transform.position;
		if (Mathf.Abs (relative.x) > Mathf.Abs (relative.y)) 
		{
			if (relative.x > 0 && _attPos.right) 
			{
				nextPos = _attPos.right;
				_move.Right ();
			} 
			else if (relative.x < 0 && _attPos.left) 
			{
				nextPos = _attPos.left;
				_move.Left ();
			} 
			else if (relative.y > 0 && _attPos.up) 
			{
				nextPos = _attPos.up;
				_move.Up ();
			}
			else if (relative.y < 0 && _attPos.down && (_attPos.VeryDown || inReturn)) 
			{
				nextPos = _attPos.down;
				_move.Down ();
			}
			else
				RamdomNewPos (_attPos);
		}
		else 
		{
			if (relative.y > 0 && _attPos.up) 
			{
				nextPos = _attPos.up;
				_move.Up ();
			}
			else if (relative.y < 0 && _attPos.down && (_attPos.VeryDown || inReturn)) 
			{
				nextPos = _attPos.down;
				_move.Down ();
			}
			else if (relative.x > 0 && _attPos.right) 
			{
				nextPos = _attPos.right;
				_move.Right ();
			} 
			else if (relative.x < 0 && _attPos.left) 
			{
				nextPos = _attPos.left;
				_move.Left ();
			}
			else
				RamdomNewPos (_attPos);
		}
	}

	public void RamdomMoviment ()
	{
		if (transform.position == attPos.transform.position) 
		{
			RamdomNewPos (attPos.GetComponent<CurveController>());
		}

		if (transform.position == nextPos.transform.position) 
		{
			_originPos = attPos;
			attPos = nextPos;
			RamdomNewPos (attPos.GetComponent<CurveController>());
		}

		Debug.DrawLine (transform.position, nextPos.transform.position, Color.red);
		if(_originPos)
			Debug.DrawLine (transform.position, _originPos.transform.position, Color.green);
		Debug.DrawLine (transform.position, attPos.transform.position, Color.blue);
	}

	public void RamdomNewPos (CurveController _attPos)
	{
		_move.Rays ();
		List<GameObject> array = new List<GameObject> ();

		if (_attPos.right && _originPos != _attPos.right)
			array.Add (_attPos.right);
		if (_attPos.left && _originPos != _attPos.left)
			array.Add (_attPos.left);
		if (_attPos.up && _originPos != _attPos.up)
			array.Add (_attPos.up);
		if (_attPos.down && _attPos.VeryDown && _originPos != _attPos.down)
			array.Add (_attPos.down);

		nextPos = array [Random.Range (0, array.Count)];

		if (nextPos == _attPos.right) 
		{
			_move.Right ();
		}
		if (nextPos == _attPos.left) 
		{
			_move.Left ();
		}
		if (nextPos == _attPos.up) 
		{
			_move.Up ();
		}
		if (nextPos == _attPos.down) 
		{
			_move.Down ();
		}
	}
}
