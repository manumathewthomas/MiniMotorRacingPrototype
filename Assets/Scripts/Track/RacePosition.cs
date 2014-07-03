using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RacePosition : MonoBehaviour {

	public GameObject[] RaceCars;
	private Checkpoint[] checkpoint;
	
	public GameObject playerCar;
	public GameObject CheckPoint;
	
	public GameObject[] decals;
	
	public TextMesh PositionText;
	public TextMesh LapText;
	public TextMesh Timer;
	
	private int playerCarNumber;
	private bool isPlayerCarSet;
	private List<GameObject> carPosition;
	
	private List<AICarControllerScript> AI;
	private MiniVechicleControllerScript Player;
	public GameObject timer;
	
	public TextMesh carSelect;
	public Camera cam;
	public Camera centeCam;
	
	public mode RaceMode;
	
	private bool isCarCountFinish;
	private bool finish;
	
	public int lapLimit;

	
	public float startLapTime =0;
	public float LapTimer =0;
	float timeTaken;
	
	
	// Use this for initialization
	void Start () {
		
			RaceMode = GameObject.Find("mode").GetComponent<mode>();
		finish = false;
		
		if(RaceMode.raceType == 1)
		{
			AI= new List<AICarControllerScript>();
			Timer.gameObject.SetActive(false);
			carPosition = new List<GameObject>();
			checkpoint = new Checkpoint[RaceCars.Length];
			isPlayerCarSet = false;
			PositionText.renderer.material.color = Color.blue;
			LapText.renderer.material.color = Color.blue;
			CheckPoint.SetActive(false);
			isCarCountFinish =false;
		
		
			for(int i =0 ;i<RaceCars.Length;i++)
			{
				AI.Add(RaceCars[i].GetComponent<AICarControllerScript>());
			
			}
		
			foreach(AICarControllerScript car in AI)
			{
				car.isTimer = false;
			}
		
	
			RaceCars[RaceMode.carType].GetComponent<AICarControllerScript>().enabled =false;
			RaceCars[RaceMode.carType].GetComponent<MiniVechicleControllerScript>().enabled = true;
			RaceCarSetup();
		}
		else
		{
			Timer.gameObject.SetActive(true);
			foreach(GameObject car in RaceCars)
			{
				if(car != RaceCars[RaceMode.carType])
				{
					car.SetActive(false);
				
				}
			}
			RaceCars[RaceMode.carType].GetComponent<AICarControllerScript>().enabled =false;
			RaceCars[RaceMode.carType].GetComponent<MiniVechicleControllerScript>().enabled = true;
			CheckPoint.SetActive(true);
			cam.gameObject.SetActive(false);
			centeCam.gameObject.SetActive(true);
			
			timer.GetComponent<Timer>().enabled = true;
			LapText.gameObject.SetActive(true);
			//PositionText.gameObject.SetActive(true);
		}
		

	}
	
	void RaceCarSetup()
	{
		for(int i= 0;i<RaceCars.Length;i++)
		{
			if(RaceCars[i].GetComponent<MiniVechicleControllerScript>().enabled && !isPlayerCarSet)
			{
				playerCar = RaceCars[i];
				playerCarNumber = i;
				isPlayerCarSet = true;
				Player = RaceCars[i].GetComponent<MiniVechicleControllerScript>();
				Player.isTimer = false;
				
			}
			else
			{
				RaceCars[i].GetComponent<AICarControllerScript>().enabled = true;
			}
			
			checkpoint[i] = RaceCars[i].GetComponent<Checkpoint>();
			carPosition.Add(RaceCars[i]);
		
		
		}

	
		
		
		
		if(isPlayerCarSet)
		{
			
			CheckPoint.SetActive(true);
			cam.gameObject.SetActive(false);
			centeCam.gameObject.SetActive(true);
			
			timer.GetComponent<Timer>().enabled = true;
			LapText.gameObject.SetActive(true);
			PositionText.gameObject.SetActive(true);
		}
		
	}
	
	
	void SelectCar()
	{
	
		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray,out hit))
			{
			
				if(hit.collider.name =="AICar"||hit.collider.name =="buggy car"||hit.collider.name == "car buggy"||hit.collider.name =="PoliceCar"||hit.collider.name=="volswagan")
				{
				
					hit.transform.GetComponent<AICarControllerScript>().enabled = false;
					hit.transform.GetComponent<MiniVechicleControllerScript>().enabled = true;
					RaceCarSetup();
					
				}
			
			}
		}
	}
	// Update is called once per frame
	void Update () {
		
		
		
		if(RaceMode.raceType == 1)
		{
			
			carPosition =carPosition.OrderByDescending(RaceCars=>RaceCars.GetComponent<Checkpoint>().rank).ToList();
			PositionText.text =" ";
			for(int i=0;i<carPosition.Count;i++)
			{
				
				carPosition[i].GetComponent<Checkpoint>().position = i+1;
				if(carPosition[i] == playerCar)
				{
					PositionText.text = "POS "+carPosition[i].GetComponent<Checkpoint>().position+"/5";
					LapText.text = "LAP "+(carPosition[i].GetComponent<Checkpoint>().curLap+1)+"/3";
					
					if(carPosition[i].GetComponent<Checkpoint>().curLap+1>3)
					{
						carPosition[i].GetComponent<MiniVechicleControllerScript>().enabled = false;
					}
					
				}
				else
				{
					if(carPosition[i].GetComponent<Checkpoint>().curLap+1>3)
					{
						carPosition[i].GetComponent<AICarControllerScript>().enabled = false;
					}
				}
			} 
			
			
			
		
		}
		else
		{
			if(RaceCars[RaceMode.carType].GetComponent<MiniVechicleControllerScript>().isTimer)
			{
				
				Timer.text = FormatTime(timeTaken);
				if(RaceCars[RaceMode.carType].GetComponent<Checkpoint>().curLap+1>3)
				{
					RaceCars[RaceMode.carType].GetComponent<MiniVechicleControllerScript>().topSpeed =0;
					RaceCars[RaceMode.carType].GetComponent<MiniVechicleControllerScript>().enabled = false;
					return;
				}
				else	
				{
					timeTaken = startLapTime + Time.time;
				}
				LapTimer +=Time.deltaTime;
				LapText.text = "LAP "+(RaceCars[RaceMode.carType].GetComponent<Checkpoint>().curLap+1)+"/3";
			}
			
		
				
		}
		
	}
	
	string FormatTime(float time)
	{
		int minutes = (int)Mathf.Floor(time/60.0f);
		int seconds = (int)Mathf.Floor(time - minutes * 60.0f);
		int milliseconds = (int)(time - Mathf.Floor(time));
		milliseconds = (int)Mathf.Floor(milliseconds * 1000.0f);
		
		string sMinutes = "00"+minutes.ToString();
		sMinutes = sMinutes.Substring(sMinutes.Length-2);
		string sSeconds = "00"+seconds.ToString();
		sSeconds = sSeconds.Substring(sSeconds.Length-2);
		
		string sMilliseconds = "000"+milliseconds.ToString();
		sMilliseconds =  sMilliseconds.Substring(sMilliseconds.Length-3);
		
		return sMinutes+":"+sSeconds+":"+sMilliseconds;
	}
}

