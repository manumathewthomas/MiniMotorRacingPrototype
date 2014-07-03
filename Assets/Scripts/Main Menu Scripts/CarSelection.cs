using UnityEngine;
using System.Collections;

public class CarSelection : MonoBehaviour {
	public GameObject carStack;
	private Vector3 newPosition;
	private bool selectUP;
	private bool selectDown;
	private bool execute;
	private int up = 0;
	private int down = 0;
	
	public AnimationClip[] carAnim;
	// Use this for initialization
	void Start () {
		selectUP = false;
		selectDown = false;
		execute = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.DownArrow) && selectDown == false && down < 4)
		{
			selectDown = true;
			
		}
		
		else if(Input.GetKeyDown(KeyCode.UpArrow) && selectUP == false &&down>0)
		{
			selectUP = true;
			
		}
		
		Debug.Log(down);
	
		if(execute == false)
			StartCoroutine("selection");
	}
	
	IEnumerator selection()
	{
		
		
		if(selectDown)
		{
			execute = true;
			carStack.animation[carAnim[down].name.ToString()].speed = 1;
			carStack.animation.Play(carAnim[down].name.ToString());
			yield return new WaitForSeconds(carStack.animation[carAnim[down].name.ToString()].length);
			down++;
			selectDown = false;
			execute = false;
			
		}
		else if(selectUP)
		{
			execute = true;
			down--;
			carStack.animation[carAnim[down].name.ToString()].speed = -1;
			carStack.animation[carAnim[down].name.ToString()].time = carStack.animation[carAnim[down].name.ToString()].length;
			carStack.animation.Play(carAnim[down].name.ToString());
			yield return new WaitForSeconds(carStack.animation[carAnim[down].name.ToString()].length);
			selectUP = false;
			execute = false;
		}
		
		
	}
}
