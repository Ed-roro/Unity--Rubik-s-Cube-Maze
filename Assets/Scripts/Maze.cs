using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour 
{
	[System.Serializable]
	public class Cell
	{
		public bool visited;
		public GameObject north; //1
		public GameObject east; //2
		public GameObject west; //3
		public GameObject south; //4
	}

	public GameObject Wall;
	public float wallLength = 1.0f;
	public int xSize = 15;
	public int ySize = 15;
	private Vector3 initialPos;
	private GameObject wallHolder;
	private Cell[] cells;
	private int currentCell = 0; //make public if you want to see current cell
	private int totalCells;
	private int visitedCells = 0;
	private bool startedBuilding = false;
	private int currentNeighbor = 0;
	private List<int> lastCells;
	private int backingUp = 0;
	private int wallToBreak = 0;
	//public float timer = 120.0f;
	public bool startTime = false;

	// Use this for initialization
	void Start () 
	{
		CreateWalls ();
	}

	void CreateWalls()
	{
		wallHolder = new GameObject ();
		wallHolder.name = "Maze";

		initialPos = new Vector3 ((-xSize / 2) + wallLength / 2, 20.3f, (-ySize / 2) + wallLength / 2);
		Vector3 myPos = initialPos;
		GameObject tempWall;

		//For x-Axis
		for (int i =0; i<ySize; i++) 
		{
			for(int j =0; j<=xSize; j++)
			{
				myPos = new Vector3(initialPos.x + (j*wallLength)- wallLength/2, 20.3f, initialPos.z + (i*wallLength)- wallLength/2);
				tempWall = Instantiate(Wall,myPos,Quaternion.identity) as GameObject;
				tempWall.transform.parent = wallHolder.transform;

			}
		}

		//For y-Axis
		for (int i =0; i<=ySize; i++) 
		{
			for(int j =0; j<xSize; j++)
			{
				myPos = new Vector3(initialPos.x + (j*wallLength), 20.3f, initialPos.z + (i*wallLength)- wallLength);
				tempWall = Instantiate(Wall,myPos,Quaternion.Euler(0.0f,90.0f,0.0f)) as GameObject;
				tempWall.transform.parent = wallHolder.transform;
				
			}
		}

		CreateCells ();
	}

	void CreateCells()
	{
		lastCells = new List<int> ();
		lastCells.Clear ();
		totalCells = xSize * ySize;
		GameObject[] allWalls;
		int children = wallHolder.transform.childCount;
		allWalls = new GameObject[children];
		cells = new Cell[xSize * ySize];
		int eastWestProcess = 0;
		int childProcess = 0;
		int termCount = 0;

		//Getting All the children
		for (int i=0; i<children; i++) 
		{
			allWalls[i]= wallHolder.transform.GetChild(i).gameObject;
		}

		//Assign Walls to the cells
		for (int cellProcess=0; cellProcess<cells.Length; cellProcess++) 
		{
			if(termCount == xSize)
			{
				eastWestProcess ++;
				termCount = 0;
			}

			cells[cellProcess] = new Cell();
			cells[cellProcess].east = allWalls[eastWestProcess];
			cells[cellProcess].south = allWalls[childProcess+(xSize+1)*ySize];


			eastWestProcess++;

			termCount++;
			childProcess++;
			cells[cellProcess].west = allWalls[eastWestProcess];
			cells[cellProcess].north = allWalls[(childProcess+(xSize+1)*ySize)+xSize-1];
		}

		CreateMaze ();
	}

	void CreateMaze()
	{
		if (visitedCells < totalCells) 
		{
			if(startedBuilding)
			{
				GetNeighbors ();
				if(cells[currentNeighbor].visited == false && cells[currentCell].visited == true)
				{
					BreakWall();
					cells[currentNeighbor].visited = true;
					visitedCells++;
					lastCells.Add(currentCell);
					currentCell = currentNeighbor;

					if(lastCells.Count > 0)
					{
						backingUp = lastCells.Count - 1;
					}
				}
			}

			else
			{
				currentCell = Random.Range(0,totalCells);
				cells[currentCell].visited = true;
				visitedCells ++;
				startedBuilding = true;
			}

			Invoke("CreateMaze", 0.0f);
		}

		else if(visitedCells == totalCells)
		{
			Debug.Log("Finished");
			startTime = true;
		}
	}


	void BreakWall()
	{
		switch (wallToBreak) 
		{
		case 1: {Destroy(cells[currentCell].north); break;}
		case 2: {Destroy(cells[currentCell].east); break;}
		case 3: {Destroy(cells[currentCell].west); break;}
		case 4: {Destroy(cells[currentCell].south); break;}
		}
	}

	void GetNeighbors()
	{
		int length = 0;
		int[] neighbors = new int[4];
		int[] connectingWall = new int[4];
		int check = 0;

		check = ((currentCell + 1) / xSize);
		check -= 1;
		check *= xSize;
		check += xSize;

		//west side XD
		if (currentCell + 1 < totalCells && (currentCell+1)!= check) 
		{
			if(cells[currentCell+1].visited == false)
			{
				neighbors[length] = currentCell+1;
				connectingWall[length] = 3;
				length++;
			}
		}


		//east side XD
		if (currentCell - 1 >= 0 && currentCell != check) 
		{
			if(cells[currentCell-1].visited == false)
			{
				neighbors[length] = currentCell-1;
				connectingWall[length] = 2;
				length++;
			}
		}


		//north side XD
		if (currentCell + xSize < totalCells) 
		{
			if(cells[currentCell + xSize].visited == false)
			{
				neighbors[length] = currentCell + xSize;
				connectingWall[length] = 1;
				length++;
			}
		}


		//south side XD
		if (currentCell - xSize >= 0) 
		{
			if(cells[currentCell - xSize].visited == false)
			{
				neighbors[length] = currentCell - xSize;
				connectingWall[length] = 4;
				length++;
			}
		}


		if (length != 0) 
		{
			int chosenOne = Random.Range (0, length);
			currentNeighbor = neighbors [chosenOne];
			wallToBreak = connectingWall[chosenOne];
		} 

		else 
		{
			if(backingUp > 0)
			{
				currentCell = lastCells[backingUp];
				backingUp--;
			}
		}


	}
	
	// Update is called once per frame
	void Update () 
	{
		/*if (startTime == true) 
		{
			timer -= Time.deltaTime;
			if (timer <= 0) 
			{
				GameOver();

			}
		}*/
	}
	

	void onGUI()
	{
		//GUI.Box(new Rect(10, 10, 50, 20), "" + timer.ToString("0"));
	}
}
