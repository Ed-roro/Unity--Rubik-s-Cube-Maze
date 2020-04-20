using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Position : MonoBehaviour 
{
	public Vector3[] goalPosition;
	// Use this for initialization
	void Start () 
	{
		int randNum = Random.Range (0, goalPosition.Length);
		transform.position = goalPosition [randNum];
	}

	void Update()
	{

	}

}