using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public int points;

	public Sprite[] num;

	public Image I1;
	public Image I10;
	public Image I100;
	public Image I1000;

	public int ghost = 1;

	// Use this for initialization
	void Start () {
		ghost = 1;
	}
	
	// Update is called once per frame
	void Update () {
		bool notHit = true;
		foreach (GameObject ob in GameObject.FindGameObjectsWithTag("ghost")) 
		{
			if (ob.GetComponent<MoveController> ().hit) 
			{
				notHit = false;
			}
		}

		if (notHit)
			ghost = 1;

		if (points > 9999)
			points = 0;

		int i1000 = (int)points / 1000;
		int i100 = (int)(points - (i1000*1000)) / 100;
		int i10 = (int)(points - (i1000*1000) - (i100*100)) / 10;
		int i1 = (int)(points - (i1000*1000) - (i100*100) - (i10*10));


		I1000.sprite = num [i1000];
		I100.sprite = num [i100];
		I10.sprite = num [i10];
		I1.sprite = num [i1];
	}
}