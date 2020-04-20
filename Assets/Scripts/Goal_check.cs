using UnityEngine;
using System.Collections;

public class Goal_check : MonoBehaviour 
{

	public bool goalTouch = false;
	public GameObject person;
	// Use this for initialization
	void Start () 
	{
	
	}

	void onCollisionEnter(Collision col)
	{
		if (person) 
		{
			//Application.LoadLevel (0);
			goalTouch = true;
		}

		if (goalTouch == true) 
		{
			Application.LoadLevel(0);
		}

	}
	// Update is called once per frame
	void Update () {
	
	}
}
