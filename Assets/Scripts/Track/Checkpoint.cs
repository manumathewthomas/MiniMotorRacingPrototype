using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
	public Transform[] checkpoints;
	private int curCheckPoint;
	public int rank;
	public int curLap;
	public float distanceToNextCheckpoint;
	public int position;


	
	private AICarControllerScript AICar;
	private MiniVechicleControllerScript PlayerCar;



	// Use this for initialization
	void Start () {

		curCheckPoint =0;
		curLap =-1;
		

	
	}
	
	// Update is called once per frame
	void Update () {
		
		
		if(transform.GetComponent<AICarControllerScript>().enabled)
		{
			rank = transform.GetComponent<AICarControllerScript>().rank;
			
		}
		else if(transform.GetComponent<MiniVechicleControllerScript>().enabled)
		{
			PlayerCar = transform.GetComponent<MiniVechicleControllerScript>();
			rank = PlayerCar.rank;
			//Debug.Log("rank = "+rank);
			
		}
		distanceToNextCheckpoint = Vector3.Distance(transform.position,checkpoints[curCheckPoint].position);
		
		//Debug.Log("current checkpoint = "+curCheckPoint + "Current lap ="+curLap +"Distance to next checkpoint ="+distanceToNextCheckpoint);
	
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject == checkpoints[curCheckPoint].gameObject)	
		{
			if(curCheckPoint+1<checkpoints.Length)
			{
				if(curCheckPoint == 0)
					curLap++;
				
				curCheckPoint++;
			}
			else
			{
				curCheckPoint = 0;
			
			}
			
			
		}
	}
}
