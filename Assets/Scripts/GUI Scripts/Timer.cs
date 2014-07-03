using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Timer : MonoBehaviour {
	
	public GameObject[] RaceCars;
	public float endTime,dismissTime;
	public TextMesh Counter;
	private List<AICarControllerScript> AI;
	private MiniVechicleControllerScript Player;
	private bool finish;


	// Use this for initialization
	void Start () {
		Counter.transform.position = new Vector3(34,0,20);
		Counter.fontSize = 1300;
		AI = new List<AICarControllerScript>();
		for(int i= 0;i<RaceCars.Length;i++)
		{
			if(RaceCars[i].GetComponent<MiniVechicleControllerScript>().enabled )
			{
				Player = RaceCars[i].GetComponent<MiniVechicleControllerScript>();
				 Player.isTimer = false;
			}
			else
			{
				AI.Add(RaceCars[i].GetComponent<AICarControllerScript>());
			}
		}
		
		foreach(AICarControllerScript car in AI)
		{
			car.isTimer = false;
		}
		endTime = Time.time + 3.9f;
		dismissTime = Time.time + 5.0f;
		Counter = GameObject.Find("Counter").GetComponent<TextMesh>();
		Counter.text = "3";
		
		finish = false;
		
		
		
	}
	
	// Update is called once per frame
	

	void Update () {
	
		int timeLeft = (int)(endTime - Time.time);
		switch(timeLeft)
		{
		case 3:
		case 2:
			Counter.renderer.material.color = Color.red;
			break;
		case 1:
			Counter.renderer.material.color = Color.yellow;
			break;
		
		}
		if(timeLeft <= 0 && !finish)
		{
			timeLeft = 0;
			Counter.renderer.material.color = Color.green;
			Counter.text = "GO";
			Player.isTimer = true;
			foreach(AICarControllerScript car in AI)
			{
				car.isTimer =true;
			}
			int dismissTimeLeft = (int) (dismissTime - Time.time);
			if(dismissTimeLeft <=0)
			{
				Counter.renderer.enabled =false;
			}
		}
		else
		Counter.text = timeLeft.ToString();
		
		
		if(Player.GetComponent<MiniVechicleControllerScript>().enabled == false && !finish)
		{
			finish = true;
			
			StartCoroutine(Restart());
			
		}
		
		if(finish)
		{
			Counter.fontSize = 500;
			Counter.text = "Game Over";
		}
	}
	
	IEnumerator Restart()
	{
		Counter.renderer.enabled =true;
		yield return new WaitForSeconds(3);
		Application.LoadLevel(0);
	}
}
