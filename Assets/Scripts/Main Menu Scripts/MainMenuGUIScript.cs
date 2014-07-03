using UnityEngine;
using System.Collections;

public class MainMenuGUIScript : MonoBehaviour {
	
	
	public Transform Career_Wording;
	public Transform QuickRace_Wording;
	public Transform Options_Wording;
	public Transform Ranking_Wording;
	public Transform Grandprix_Wording;
	public Transform TimeTrial_Wording;
	public Transform SelectYourVehicle_Wording;
	
	public Transform Career_Collider;
	public Transform QuickRace_Collider;
	public Transform Options_Collider;
	public Transform Ranking_Collider;
	public Transform Grandprix_Collider;
	public Transform TimeTrial_Collider;
	public Transform Store_Collider;
	
	public GUITexture CareerMenuLeftButton;
	public GUITexture CareerMenuRightButton;
	public GUITexture OptionMenuLeftButton;
	public GUITexture OptionMenuRightButton;
	public GUITexture HomeButton;
	
	public Transform[] carPrefabs;
	public Transform[] optionsControl;
	public Transform soundButton;
	public Transform musicButton;
	public TextMesh carSelected;
	public TextMesh trackSelected;
	public TextMesh GO;
	public TextMesh OptionsControl;
	public TextMesh OptionsAudio;
	public TextMesh OptionsCredits;
	public TextMesh CreditText;
	public TextMesh CareerUpgradeDone;
	public GameObject QuickRaceScreenItem;
	public GameObject OptionsScreenItem;

	public GameObject ControlGroup;
	public GameObject SoundGroup;
	public GameObject MusicGroup;
	public GameObject GrandprixGroup;
	public GameObject GarageGroup;
	public GameObject RankingGroup;
	public GameObject CareerGroup;
	
	private bool quickRaceIsCarSelect;
	private bool quickRaceIsTrackSelect;
	private bool optionDefault;
	
	
	private Transform currentCar;
	private Transform careerCar;
	private int carIndex;
	private int optionControlIndex;
	private bool isCurrentCarRotate;
	private bool isTouchOn;
	private bool isCarSelect;
	private float currentCarOffset;
	private Vector3 intialCamPosition;
	private Quaternion intialCamRotation;
	private Quaternion newRot;
	
	enum menuState
	{	
		MAINMENUSTATE,
		CAREERMENUSTATE,
		QUICKRACEMENUSTATE,
		OPTIONSMENUSTATE,
		STOREMENUSTATE,
		RANKINGMENUSTATE,
		GRANDPRIXMENUSTATE,
		TIMETRIALMENUSTATE
	};
	
	menuState MenuState;
	
	void Awake() {
		intialCamPosition = Camera.main.transform.position;
		intialCamRotation = Camera.main.transform.rotation;
		
		
	}
	
	void Start () {
		MenuIntialize();
	}
	
	void MenuIntialize()
	{
		SubMenuHiddenItems();
		ShowAllMainMenuWordings();
		MenuState = menuState.MAINMENUSTATE;
		QuickRaceScreenItem.SetActive(false);
		OptionsScreenItem.SetActive(false);
		GrandprixGroup.SetActive(false);
		GarageGroup.SetActive(false);
		RankingGroup.SetActive(false);
		CareerGroup.SetActive(false);
		
		optionDefault = false;
		carIndex = 0;
		optionControlIndex = 0;
		isCurrentCarRotate = true;
		isCarSelect = false;
		Camera.main.transform.position =intialCamPosition;
		Camera.main.transform.rotation =intialCamRotation;
	}
	
	
	void Update ()
	{
		if(MenuState == menuState.MAINMENUSTATE)
		{
			
		}
		else
		{
			HideAllMainMenuWordings();
			
		}
		
		if(MenuState == menuState.CAREERMENUSTATE)
		{
			CareerMenu();
		}
		else if(MenuState == menuState.QUICKRACEMENUSTATE)
		{
			QuickRaceMenu();
		}
		else if(MenuState == menuState.GRANDPRIXMENUSTATE)
		{
			CareerMenu();
		}
		else if(MenuState == menuState.TIMETRIALMENUSTATE)
		{
			QuickRaceMenu();
		}
		else if(MenuState == menuState.OPTIONSMENUSTATE)
		{
			OptionsMenu();
		}
		else if(MenuState == menuState.STOREMENUSTATE)
		{
			GarageGroup.SetActive(true);
						
		}
		else if(MenuState == menuState.RANKINGMENUSTATE)
		{
			RankingGroup.SetActive(true);
		}
		
		
		TouchMenuItems();	
		
	}
	
	
	void CareerMenu()
	{
					
		if(currentCar)
		{
			if(isCurrentCarRotate)
			{
				currentCar.Rotate(-Vector3.up * 5 * Time.deltaTime);
			}
			
			else
			{
				
				
				if(Mathf.Abs(currentCar.rotation.y)-Mathf.Abs(newRot.y) <0.0001&& !isCarSelect)
				{
					//currentCar.rigidbody.isKinematic = false;
					//currentCar.rigidbody.useGravity = true;
					animation.Play("cameraToSecondDivAnimation");
					isCarSelect = true;
					
					if(MenuState == menuState.GRANDPRIXMENUSTATE)
					{
						GrandprixGroup.SetActive(true);
						
					}
					
					if(MenuState == menuState.CAREERMENUSTATE)
					{
						careerCar = Instantiate(currentCar,new Vector3(301.2f,1.56f,578.2f),currentCar.rotation) as Transform;;
					
						
						CareerGroup.SetActive(true);
						
						careerCar.position = new Vector3(301.6f,1.56f,578.2f);
						careerCar.eulerAngles = new Vector3(0,90,0);
				
					}
				}
				else
				{
					newRot = Quaternion.Euler(new Vector3(0,currentCarOffset - currentCar.eulerAngles.y,0));
					currentCar.rotation = Quaternion.Slerp(currentCar.rotation,newRot,Time.deltaTime);
				}
			}
			
			
		
			if(Input.GetMouseButtonDown(0))
				{
					RaycastHit hit = new RaycastHit();
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
					if(Physics.Raycast(ray,out hit))
					{
						if(hit.transform.name == currentCar.name)
						{	
							isCurrentCarRotate = false;	
							SubMenuHiddenItems();	
							
						}
			
						if(hit.collider.name == "Grandprix1" ||
							hit.collider.name == "Grandprix2" || 
							hit.collider.name == "Grandprix3" ||
							hit.collider.name == "Grandprix4" ||
							hit.collider.name == "Grandprix5" ||
							hit.collider.name == "Asian Championship" ||
							hit.collider.name == "Brazil Championship" ||
							hit.collider.name == "Europe Championship" ||
							hit.collider.name == "World Championship" ||
							hit.collider.name == "Australian Championship")
							{
								Application.LoadLevel(1);
							}
						
						if(hit.collider.name =="Done Wording")
						{
							animation.Play("cameraToThirdDiv");
						}
						
						if(hit.collider.name =="Flag5")
						{
							Application.LoadLevel(1);
						}
				
					}
					
					if(CareerMenuRightButton.HitTest(Input.mousePosition,Camera.main) && isTouchOn == false && carIndex<carPrefabs.Length-1)
					{
						Destroy(currentCar.gameObject);
						carIndex++;
						
						currentCar= Instantiate(carPrefabs[carIndex],new Vector3(296.7675f,2.1f,576f),carPrefabs[carIndex].rotation) as Transform;
						isTouchOn = true;
					}
					
					else if(CareerMenuLeftButton.HitTest(Input.mousePosition,Camera.main) && isTouchOn == false && carIndex > 0)
					{
						Destroy(currentCar.gameObject);
						carIndex--;
						
						currentCar= Instantiate(carPrefabs[carIndex],new Vector3(296.7675f,2.1f,576f),carPrefabs[carIndex].rotation) as Transform;
						isTouchOn = true;
					}
				}
				
				if(Input.GetMouseButtonUp(0))
				{
					if(CareerMenuRightButton.HitTest(Input.mousePosition,Camera.main) && isTouchOn == true)
					{
						isTouchOn = false;
					}
					
					else if(CareerMenuLeftButton.HitTest(Input.mousePosition,Camera.main) && isTouchOn == true)
					{
						isTouchOn = false;
					}
				}
			
		}
		
	
		
	}
	
	
	
	void QuickRaceMenu()
	{	
	
			
			if(Input.GetMouseButtonDown(0) )
			{
				RaycastHit hit = new RaycastHit();
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				
				if(Physics.Raycast(ray,out hit))
				{
					
					if(hit.collider.name == "car 1" ||
						hit.collider.name == "car 2" || 
						hit.collider.name == "car 3" || 
						hit.collider.name == "car 4" || 
						hit.collider.name == "car 5" || 
						hit.collider.name == "car 6" || 
						hit.collider.name == "car 7" || 
						hit.collider.name == "car 8" || 
						hit.collider.name == "car 9" || 
						hit.collider.name == "car 10" || 
						hit.collider.name == "car 11" || 
						hit.collider.name == "car 12" || 
						hit.collider.name == "car 13" || 
						hit.collider.name == "car 14" || 
						hit.collider.name == "car 15" || 
						hit.collider.name == "car 16" || 
						hit.collider.name == "car 17" || 
						hit.collider.name == "car 18" || 
						hit.collider.name == "car 19" || 
						hit.collider.name == "car 20")
						{
							carSelected.renderer.enabled = true;
							carSelected.text = hit.collider.name + " Selected";
							quickRaceIsCarSelect = true;
						
						}
					
						if(hit.collider.name == "track 1" ||
						hit.collider.name == "track 2" || 
						hit.collider.name == "track 3" || 
						hit.collider.name == "track 4" || 
						hit.collider.name == "track 5" || 
						hit.collider.name == "track 6" || 
						hit.collider.name == "track 7" || 
						hit.collider.name == "Random Track" )
						{
							trackSelected.renderer.enabled = true;
							trackSelected.text = hit.collider.name + " Selected";
							quickRaceIsTrackSelect = true;
						
						}
					
					if(quickRaceIsCarSelect == true && quickRaceIsTrackSelect == true)
					{
						GO.renderer.enabled = true;
						if(hit.collider.name == "go")
						{
							Application.LoadLevel(1);
						}
						
					}
				}
			
		}
		
	}
	
	
	void OptionsMenu()
	{
		
		if(optionDefault)
		{
			OptionsMenuControl();
			optionDefault =!optionDefault;
		}
		
	
		
				if(Input.GetMouseButtonDown(0) )
				{
					RaycastHit hit = new RaycastHit();
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
					if(Physics.Raycast(ray,out hit))
					{
					
						if(hit.collider.name == "ControlCollider")
						{
							CreditText.renderer.enabled = false;
							foreach(Transform control in optionsControl)
							{
								control.renderer.enabled = false;
							}
							OptionsMenuControl();
							
							
							SoundGroup.SetActive(false);
							MusicGroup.SetActive(false);
							
						}
					
						if(hit.collider.name == "AudioCollider")
						{
							OptionsControl.renderer.material.color = Color.white;	
							OptionsAudio.renderer.material.color = Color.yellow;	
							OptionsCredits.renderer.material.color = Color.white;	
							OptionMenuLeftButton.guiTexture.enabled = false;
							OptionMenuRightButton.guiTexture.enabled = false;
							CreditText.renderer.enabled = false;
							ControlGroup.SetActive(false);
							SoundGroup.SetActive(true);
							MusicGroup.SetActive(true);
						}
					
						if(hit.collider.name == "CreditsCollider")
						{
							OptionsControl.renderer.material.color = Color.white;	
							OptionsAudio.renderer.material.color = Color.white;	
							OptionsCredits.renderer.material.color = Color.yellow;	
							OptionMenuLeftButton.guiTexture.enabled = false;
							OptionMenuRightButton.guiTexture.enabled = false;
							CreditText.renderer.enabled = true;
							ControlGroup.SetActive(false);
							SoundGroup.SetActive(false);
							MusicGroup.SetActive(false);
						}
				
				if(hit.collider.name == "Sound Slider")
							{
								soundButton.position = new Vector3(hit.point.x,soundButton.position.y,soundButton.position.z);
							}
					
							if(hit.collider.name == "Music Slider")
							{
								musicButton.position = new Vector3(hit.point.x,musicButton.position.y,musicButton.position.z);
							}
					
						
					
					}
						if(OptionMenuRightButton.HitTest(Input.mousePosition,Camera.main) && isTouchOn == false && optionControlIndex<optionsControl.Length-1)
						{
					
							optionsControl[optionControlIndex].renderer.enabled = false;
							optionControlIndex++;
					
							optionsControl[optionControlIndex].renderer.enabled = true;
							isTouchOn = true;
						}
				
						else if(OptionMenuLeftButton.HitTest(Input.mousePosition,Camera.main) && isTouchOn == false && optionControlIndex > 0)
						{
							optionsControl[optionControlIndex].renderer.enabled = false;
							optionControlIndex--;
					
							optionsControl[optionControlIndex].renderer.enabled = true;
							isTouchOn = true;
						}		
				}
			
//					if(Input.touchCount > 0 && touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved )
//					{
//						RaycastHit hit = new RaycastHit();
//						Ray ray = Camera.main.ScreenPointToRay(touch.position);
//			
//						if(Physics.Raycast(ray,out hit))
//						{
//							if(hit.collider.name == "Sound Slider")
//							{
//								soundButton.position = new Vector3(hit.point.x,soundButton.position.y,soundButton.position.z);
//							}
//					
//							if(hit.collider.name == "Music Slider")
//							{
//								musicButton.position = new Vector3(hit.point.x,musicButton.position.y,musicButton.position.z);
//							}
//						}
//					}
					
				
					if(Input.GetMouseButtonUp(0) )
					{
						if(OptionMenuRightButton.HitTest(Input.mousePosition,Camera.main) && isTouchOn == true)
						{
							isTouchOn = false;
						}
				
						else if(OptionMenuLeftButton.HitTest(Input.mousePosition,Camera.main) && isTouchOn == true)
						{
							isTouchOn = false;
						}
					}
				Debug.Log(optionControlIndex);
				
			
				
	}
	
	void OptionsMenuControl()
	{
		ControlGroup.SetActive(true);
		OptionsControl.renderer.material.color = Color.yellow;	
		OptionsAudio.renderer.material.color = Color.white;	
		OptionsCredits.renderer.material.color = Color.white;
		OptionMenuLeftButton.guiTexture.enabled = true;
		OptionMenuRightButton.guiTexture.enabled = true;
		optionsControl[optionControlIndex].renderer.enabled = true;


	}
	
	void TouchMenuItems()
	{
		
			
			if(Input.GetMouseButtonDown(0))
			{
				RaycastHit hit = new RaycastHit();
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				
				if(Physics.Raycast(ray,out hit))
				{
					
					if(hit.collider.tag == "Career_collider")
					{
						MenuState = menuState.CAREERMENUSTATE;
						StartCoroutine(CareerCarSelection());
						Debug.Log("Carrer HIT!");
					}
			
					if(hit.collider.tag  == "Grandprix_collider")
					{
						MenuState = menuState.GRANDPRIXMENUSTATE;
						StartCoroutine(CareerCarSelection());
						Debug.Log("Grandprix HIT!");
						
					}
			
					if(hit.transform.tag == "Options_collider")
					{
						
						MenuState = menuState.OPTIONSMENUSTATE;
						StartCoroutine(OptionsSelection());
						Debug.Log("Options HIT!");
					}
				
					if(hit.transform.tag == "Quickrace_collider")
					{
						animation.Play("Quickrace_Mainmenu_Animation");
						QuickRaceScreenItem.SetActive(true);
						MenuState = menuState.QUICKRACEMENUSTATE;
						Debug.Log("Quickrace HIT!");
					}
				
					if(hit.transform.tag == "Ranks_collider")
					{
						MenuState = menuState.RANKINGMENUSTATE;
						Debug.Log("Ranks HIT!");
					}
			
					if(hit.transform.tag == "Store_collider")
					{
						animation.Play("Store_Mainmenu_Animation");
						MenuState = menuState.STOREMENUSTATE;
						Debug.Log("Store HIT!");
					}
				
					if(hit.transform.tag == "Timetrials_collider")
					{
						MenuState = menuState.TIMETRIALMENUSTATE;
						animation.Play("Quickrace_Mainmenu_Animation");
						QuickRaceScreenItem.SetActive(true);
						Debug.Log("Timetrials HIT!");
					}
					
					
					if(HomeButton.HitTest(Input.mousePosition,Camera.main))
					{
						if(currentCar)
						{
							Destroy(currentCar.gameObject);
							Destroy(careerCar.gameObject);
						}
						MenuIntialize();
						Debug.Log("asd");
					}
				}
			
		
		}
	}
	
	
	void SubMenuHiddenItems() 
	{
		SelectYourVehicle_Wording.renderer.enabled =false;
		CareerMenuLeftButton.guiTexture.enabled =false;
		CareerMenuRightButton.guiTexture.enabled =false;
		OptionMenuLeftButton.guiTexture.enabled =false;
		OptionMenuRightButton.guiTexture.enabled =false;
		carSelected.renderer.enabled = false;
		trackSelected.renderer.enabled = false;
		GO.renderer.enabled = false;
	}
	
	void HideAllMainMenuWordings()
	{
		Career_Wording.renderer.enabled = false;
		QuickRace_Wording.renderer.enabled = false;
		Options_Wording.renderer.enabled = false;
		Ranking_Wording.renderer.enabled = false;
		Grandprix_Wording.renderer.enabled = false;
		TimeTrial_Wording.renderer.enabled = false;
		
		Career_Collider.collider.enabled = false;
		QuickRace_Collider.collider.enabled = false;
		Options_Collider.collider.enabled = false;
		Ranking_Collider.collider.enabled = false;
		Grandprix_Collider.collider.enabled = false;
		TimeTrial_Collider.collider.enabled = false;
		Store_Collider.collider.enabled = false;
		
		
	}
	
	void ShowAllMainMenuWordings()
	{
		Career_Wording.renderer.enabled = true;
		QuickRace_Wording.renderer.enabled = true;
		Options_Wording.renderer.enabled = true;
		Ranking_Wording.renderer.enabled = true;
		Grandprix_Wording.renderer.enabled = true;
		TimeTrial_Wording.renderer.enabled = true;
		
		Career_Collider.collider.enabled = true;
		QuickRace_Collider.collider.enabled = true;
		Options_Collider.collider.enabled = true;
		Ranking_Collider.collider.enabled = true;
		Grandprix_Collider.collider.enabled = true;
		TimeTrial_Collider.collider.enabled = true;
		Store_Collider.collider.enabled = true;
		
		
	}
	
	IEnumerator CareerCarSelection()
	{
		
		animation.Play("Career_Mainmenu_Animation");
						
		yield return new WaitForSeconds(animation["Career_Mainmenu_Animation"].length);
						
		SelectYourVehicle_Wording.renderer.enabled = true;
		CareerMenuLeftButton.guiTexture.enabled = true;
		CareerMenuRightButton.guiTexture.enabled = true;
		
	 	currentCar= Instantiate(carPrefabs[carIndex],new Vector3(296.7675f,2.1f,576f),carPrefabs[0].rotation) as Transform;
		currentCarOffset = currentCar.eulerAngles.y;
		
						
	}
	
	IEnumerator OptionsSelection()
	{
		foreach(Transform control in optionsControl)
		{
			control.renderer.enabled = false;
		}
		animation.Play("Options_Mainmenu_Animation");
		yield return new WaitForSeconds(animation["Options_Mainmenu_Animation"].length);
		CreditText.renderer.enabled = false;
		OptionsScreenItem.SetActive(true);
		SoundGroup.SetActive(false);
		MusicGroup.SetActive(false);
		optionDefault = true;
		isTouchOn = false;
		optionControlIndex = 0;
		OptionsMenu();
	}
}
