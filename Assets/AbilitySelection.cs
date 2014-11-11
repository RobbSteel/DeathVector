using UnityEngine;
using System.Collections;

public class AbilitySelection : MonoBehaviour {

	public GameObject[,] abilities = new GameObject[4,2];
	public GameObject[] possibleabilities;
	public GameObject playerweaponslot;
	public GameObject abilityslot1;
	public GameObject abilityslot2;
	public GameObject pickmarker;
	private GameObject weaponchoice;
	private GameObject ability1choice;
	private GameObject ability2choice;
	public PlayerAbilities playerchoices;
	private string xaxiscontrol;
	private string yaxiscontrol;
	private string ability1control;
	private string pickbutton;
	private string backbutton;
	private string readybutton;
	private float deadzone = .25f;
	private float currentabilityrow = 0;
	private float currentabilitycolumn = 0;
	private bool tapped = false;
	public float playernumber;
	private float picks = 0;
	private float abilitypicks = 0;
	private float weaponpicks = 0;
	private int index = 0;
	private GameObject currentselection;
	private Color playercolor;
	private PlayerAbilities playerabilities;
	// Use this for initialization
	void Start () {
		if (playernumber == 1) {
			xaxiscontrol = "Horizontal";
			yaxiscontrol = "Vertical";
			readybutton = "StartButton";
			pickbutton = "PickButton";
			backbutton = "BackButton";
			playercolor = Color.blue;
			playerabilities = GameObject.FindGameObjectWithTag("Player1Info").GetComponent<PlayerAbilities>();
		}
		if (playernumber == 2) {
			xaxiscontrol = "Horizontal2";
			yaxiscontrol = "Vertical2";
			readybutton = "StartButton2";
			pickbutton = "PickButton2";
			backbutton = "BackButton2";
			playercolor = Color.red;
			playerabilities = GameObject.FindGameObjectWithTag("Player2Info").GetComponent<PlayerAbilities>();
		}
		if (playernumber == 1) {
			xaxiscontrol = "Horizontal";
			yaxiscontrol = "Vertical";
			ability1control = "FirstAbility";
		}
		if (playernumber == 1) {
			xaxiscontrol = "Horizontal";
			yaxiscontrol = "Vertical";
			ability1control = "FirstAbility";
		}



		for (int i = 0; i < 2; i++) {
			for (int j = 0; j < 4; j++) {
				if(index < 5){
					print (index);
					abilities[j, i] = possibleabilities[index];
					index++;
				}

			}
		}

		currentselection = abilities[(int)currentabilityrow, (int)currentabilitycolumn];
		currentselection.GetComponent<SpriteRenderer>().color = Color.red;

	}

	void CheckChoice(GameObject abilitypick, PlayerAbilities playerabilities){
		if (abilitypick.tag == "Weapon" && weaponpicks < 1) {
			if (abilitypick.name == "flamethrower") {
				playerabilities.fire = true;
			}
			if (abilitypick.name == "boomerang") {
				playerabilities.boomerang = true;
			}
			if (abilitypick.name == "sniper") {
				playerabilities.sniper = true;
			}
			if (abilitypick.name == "machinegun") {
				playerabilities.machinegun = true;
			}

		}

		if (abilitypick.tag == "Ability" && abilitypicks == 0) {
			if (abilitypick.name == "mine") {
				playerabilities.ability1 = "mine";;
			}
		} 
		if (abilitypick.tag == "Ability" && abilitypicks == 1) {
			if (abilitypick.name == "mine") {
				playerabilities.ability2 = "mine";;
			}
		}

	}

	void PlayerCheck(GameObject abilitypick){
		if (playernumber == 1) {
			CheckChoice(abilitypick, playerabilities);
			if(weaponpicks == 0 && abilitypick.tag == "Weapon"){
				weaponchoice = (GameObject)Instantiate(abilitypick, playerweaponslot.transform.position, playerweaponslot.transform.rotation);
				weaponchoice.GetComponent<SpriteRenderer>().color = Color.white;
				weaponpicks++;
				picks++;
			}
			if(abilitypicks == 0 && abilitypick.tag == "Ability"){
				ability1choice = (GameObject)Instantiate(abilitypick, abilityslot1.transform.position, abilityslot2.transform.rotation);
				ability1choice.GetComponent<SpriteRenderer>().color = Color.white;
				abilitypicks++;
				picks++;
			} else if(abilitypicks == 1 && abilitypick.tag == "Ability"){
				ability2choice = (GameObject)Instantiate(abilitypick, abilityslot2.transform.position, abilityslot2.transform.rotation);
				ability2choice.GetComponent<SpriteRenderer>().color = Color.white;
				abilitypicks++;
				picks++;
			}
		}

		if (playernumber == 2) {
			CheckChoice(abilitypick, playerabilities);
			if(weaponpicks == 0 && abilitypick.tag == "Weapon"){
				weaponchoice = (GameObject)Instantiate(abilitypick, playerweaponslot.transform.position, playerweaponslot.transform.rotation);
				weaponchoice.GetComponent<SpriteRenderer>().color = Color.white;
				weaponpicks++;
				picks++;
			}
			if(abilitypicks == 0 && abilitypick.tag == "Ability"){
				ability1choice = (GameObject)Instantiate(abilitypick, abilityslot1.transform.position, abilityslot2.transform.rotation);
				ability1choice.GetComponent<SpriteRenderer>().color = Color.white;
				abilitypicks++;
				picks++;
			} else if(abilitypicks == 1 && abilitypick.tag == "Ability"){
				ability2choice = (GameObject)Instantiate(abilitypick, abilityslot2.transform.position, abilityslot2.transform.rotation);
				ability2choice.GetComponent<SpriteRenderer>().color = Color.white;
				abilitypicks++;
				picks++;
			}
		}

	}

	void ResetAbilities(){
		Destroy(weaponchoice);
		Destroy(ability1choice);
		Destroy (ability2choice);
		picks = 0;
		weaponpicks = 0;
		abilitypicks = 0;
		playerabilities.fire = false;
		playerabilities.boomerang = false;
		playerabilities.sniper = false;
		playerabilities.machinegun = false;
		playerabilities.ability1 = "";
		playerabilities.ability2 = "";
	}

	// Update is called once per frame
	void Update () {

		if (picks == 3 && Input.GetButtonDown(readybutton)) {
			Application.LoadLevel(1);
		}

		if (Input.GetButtonDown (backbutton)) {
			ResetAbilities();
		}


		if(Input.GetButtonDown(pickbutton) && picks < 3){
			print ("here");
			GameObject abilitypick = abilities[(int)currentabilityrow, (int)currentabilitycolumn];
			PlayerCheck(abilitypick);
		}

		Vector2 leftstick = new Vector2(Input.GetAxis(yaxiscontrol),Input.GetAxis(xaxiscontrol));
		Vector3 stickdirection = new Vector3(leftstick.x,leftstick.y, 0);
		if (stickdirection.magnitude > deadzone) {
		
			if(!tapped){
			if(leftstick.x > .5f){
				if(currentabilitycolumn != 0 ){
					currentabilitycolumn--;
					currentselection.GetComponent<SpriteRenderer>().color = Color.white;
					if(abilities[(int)currentabilityrow,(int)currentabilitycolumn] == null){
							currentabilityrow = 0;
					}
					
					PlayerMarkerMove();
					
				}
			}

			if(leftstick.x < -.5f){
				if(currentabilitycolumn != 1){
					currentabilitycolumn++;
					currentselection.GetComponent<SpriteRenderer>().color = Color.white;
					if(abilities[(int)currentabilityrow,(int)currentabilitycolumn] == null){
						currentabilityrow = 0;
					}
				
						PlayerMarkerMove();
				}
			}

			if(leftstick.y < -.5f){
				if(currentabilityrow != 0){
					currentabilityrow--;
					currentselection.GetComponent<SpriteRenderer>().color = Color.white;
					if(abilities[(int)currentabilityrow,(int)currentabilitycolumn] == null){
						currentabilityrow = 0;
					}
					
						PlayerMarkerMove();
				}
			}

			if(leftstick.y > .5f){
				if(currentabilityrow != 3){
					currentabilityrow++;
					currentselection.GetComponent<SpriteRenderer>().color = Color.white;
					if(abilities[(int)currentabilityrow,(int)currentabilitycolumn] == null){
						currentabilityrow = 0;
					}
				
					PlayerMarkerMove();
				}
			}
			}
		}

		if (stickdirection.magnitude < deadzone) {
			tapped = false;
		}
	}

	void PlayerMarkerMove(){
		GameObject abilitycolor = abilities[(int)currentabilityrow, (int)currentabilitycolumn];
		abilitycolor.GetComponent<SpriteRenderer>().color = playercolor;
		if (playernumber == 1) {
			pickmarker.transform.position = new Vector3(abilitycolor.transform.position.x-.26f, abilitycolor.transform.position.y + .75f, abilitycolor.transform.position.z);
		}
		if(playernumber == 2){
			pickmarker.transform.position = new Vector3(abilitycolor.transform.position.x+.26f, abilitycolor.transform.position.y + .75f, abilitycolor.transform.position.z);
		}

		currentselection = abilitycolor;
		tapped = true;
	}

}
