using UnityEngine;
using System.Collections;

public class GameTimer : MonoBehaviour 
{

	public float timer = 120.0f;
	// Use this for initialization
	void Start () 
	{
	
	}
	
	void Update()
	{
		timer -= Time.deltaTime;
		if (timer <= 0) 
		{
			GameOver();
		}
	}
	
	void GameOver()
	{
		Application.Quit();
		Application.LoadLevel (0);
		//timer = 30;
	}
	
	void onGUI()
	{
		GUI.depth = 3;
		GUI.Box(new Rect(10, 10, 50, 20), "" + timer.ToString("0"));
	}
}
