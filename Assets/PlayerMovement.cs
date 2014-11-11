using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float movementspeed = 1;
	public float projectilespeed = 200;
	public GameObject bullet;
	public GameObject sniperbullet;
	public GameObject flamethrower;
	public GameObject innerflamethrower;
	public GameObject SniperAssist;
	public GameObject LandMine;
	public GameObject BoomerangBullet;

	private bool fire = true;
	private bool ability1fire = true;
	private bool ability2fire = true;
	private bool flamesactive = false;
	private bool turretlocked = false;
	private bool firstabilityunlocked = false;

	//Weapon Unlocks
	private bool fireunlocked = false;
	private bool machinegununlocked = false;
	private bool boomerangshotunlocked = false;
	private bool snipershotunlocked = false;
	//Ability Unlocks
	private bool minesunlocked = false;
	private bool destructunlocked = false;

	public bool boomerangreturned = true;
	private float shotcd = .2f;
	private float ability1cd;
	private float ability2cd;
	private float deadzone = .25f;
	private float angularVelocity = 12;
	private float snipercharge = 2f;
	private VectorGrid m_VectorGrid;
	private PlayerShields personalshields;
	public float playernumber;
	private string xaxiscontrol;
	private string yaxiscontrol;
	private string xfirecontrol;
	private string yfirecontrol;
	private string ability1control;
	private string ability2control;
	private string ability3control;
	public string ability1type;
	public string ability2type;
	private Color playercolor;

	// Use this for initialization
	void Start () {
		m_VectorGrid = GameObject.Find("VectorGrid").GetComponent<VectorGrid>();
		personalshields = gameObject.GetComponent<PlayerShields> ();
		AssignControls ();
	}

	void AssignControls(){
	
		if (playernumber == 1) {
			xaxiscontrol = "Horizontal";
			yaxiscontrol = "Vertical";
			xfirecontrol = "RightStickX";
			yfirecontrol = "RightStickY";
			ability1control = "FirstAbility";
			ability2control = "SecondAbility";
			playercolor = Color.blue;
			personalshields.playercolor = playercolor;
			PlayerAbilities playerabilities = GameObject.FindGameObjectWithTag("Player1Info").GetComponent<PlayerAbilities>();
			AssignWeapons(playerabilities);
			for(int i = 0; i < 2; i++){
			AssignAbilities(playerabilities);
			}
		}

		if (playernumber == 2) {
			xaxiscontrol = "Horizontal2";
			yaxiscontrol = "Vertical2";
			xfirecontrol = "RightStickX2";
			yfirecontrol = "RightStickY2";
			ability1control = "FirstAbility2";
			ability2control = "SecondAbility2";
			playercolor = Color.red;
			personalshields.playercolor = playercolor;
			PlayerAbilities playerabilities = GameObject.FindGameObjectWithTag("Player2Info").GetComponent<PlayerAbilities>();
			AssignWeapons(playerabilities);
			for(int i = 0; i < 2; i++){
				AssignAbilities(playerabilities);
			}
		}
	
	}

	void AssignWeapons(PlayerAbilities playerabilities){
		if (playerabilities.fire) {
			fireunlocked = true;
		}
		if (playerabilities.machinegun) {
			machinegununlocked = true;
		}
		if (playerabilities.boomerang) {
			boomerangshotunlocked = true;
		}
		if (playerabilities.sniper) {
			snipershotunlocked = true;
		}
	}

	void AssignAbilities(PlayerAbilities playerabilities){

		if (playerabilities.ability1 == "mine" || playerabilities.ability2 == "mine") {
			minesunlocked = true;
			OpenAbilityCheck("mine", 2f);
		}
	}

	void OpenAbilityCheck(string abilitytype, float abilitycd){
		if (!firstabilityunlocked) {
			ability1type = abilitytype;
			ability1cd = abilitycd;
			firstabilityunlocked = true;
		} else {
			ability2type = abilitytype;
			ability2cd = abilitycd;
		}
	}

	// Update is called once per frame
	void FixedUpdate () {

		if (!personalshields.dead) {
			ShotCooldown ();
			Ability1Cooldown();
			Ability2Cooldown();
			Movement ();
			//ControllerInputs ();
			if(destructunlocked){
			SelfDestructInput();
			}
			if(fireunlocked){
			FireInput ();
			}
			if(minesunlocked && (Input.GetButtonDown (ability2control) || Input.GetButtonDown (ability1control))){
			MineInput ();
			}
			if(snipershotunlocked){
			SniperShot ();
			}
			if(machinegununlocked){
			RapidGunFire ();
			}
			if(boomerangshotunlocked){
			BoomerangFire ();
			}
		} 
	}

	void ShotCooldown(){
		if (shotcd < 0) {
			fire = true;
		} else {
			shotcd -= Time.deltaTime;
		}
	}

	void Ability1Cooldown(){
		if (ability1cd < 0) {
			ability1fire = true;
		} else {
			ability1cd -= Time.deltaTime;
		}
	}

	void Ability2Cooldown(){
		if (ability2cd < 0) {
			ability2fire = true;
		} else {
			ability2cd -= Time.deltaTime;
		}
	}


	void Movement(){
		Vector2 leftstick = new Vector2(Input.GetAxis(yaxiscontrol),Input.GetAxis(xaxiscontrol));
		Vector3 stickdirection = new Vector3(leftstick.x,leftstick.y, 0);
		if (stickdirection.magnitude > deadzone) {
			transform.Translate (0, Input.GetAxis (yaxiscontrol) * Time.deltaTime * movementspeed, 0);
			transform.Translate (Input.GetAxis (xaxiscontrol) * Time.deltaTime * movementspeed, 0, 0);
		}
	}

	void SelfDestructInput(){
	
	}

	void FireInput(){
		Vector2 rightstick = new Vector2(Input.GetAxis(xfirecontrol),Input.GetAxis(yfirecontrol));
		Vector3 direction = new Vector3(rightstick.x,rightstick.y, 0);
		if (direction.magnitude > deadzone && !personalshields.dead) {
			if(!flamesactive){
				flamethrower.SetActive(true);
				flamesactive = true;
			}
			var stickx = Input.GetAxis (xfirecontrol);
			var sticky = Input.GetAxis (yfirecontrol) * -1;
			var angle = Mathf.Atan2 (sticky, stickx) * Mathf.Rad2Deg;
			flamethrower.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
			m_VectorGrid.AddGridForce (innerflamethrower.transform.position, .1f, .2f, playercolor, true);
		} else {
			flamethrower.SetActive(false);
			flamesactive = false;
		}
	}


	void MineInput(){
		if (ability1fire) {
			if (Input.GetButtonDown (ability1control) && ability1type == "mine") {
				GameObject player1bullet = (GameObject)Instantiate (LandMine, transform.position, transform.rotation);
				player1bullet.GetComponent<Owner> ().playerowner = this.gameObject;
				player1bullet.GetComponent<Mine> ().playercolor = playercolor;
				ability1cd = 2f;
				ability1fire = false;
			}
		}
		if (ability2fire) {
			if (Input.GetButtonDown (ability2control) && ability2type == "mine") {
				GameObject player1bullet = (GameObject)Instantiate (LandMine, transform.position, transform.rotation);
				player1bullet.GetComponent<Owner> ().playerowner = this.gameObject;
				player1bullet.GetComponent<Mine> ().playercolor = playercolor;
				ability2cd = 2f;
				ability2fire = false;
			}
		}
	}



	void SniperShot(){
		if (fire) {
			Vector2 rightstick = new Vector2 (Input.GetAxis (xfirecontrol), Input.GetAxis (yfirecontrol));
			Vector3 direction = new Vector3 (rightstick.x, rightstick.y, 0);
			if (direction.magnitude > deadzone) {
				SniperAssist.SetActive(true);
				var sniperx = Input.GetAxis (xfirecontrol);
				var snipery = Input.GetAxis (yfirecontrol) * -1;
				var angle = Mathf.Atan2 (snipery, sniperx) * Mathf.Rad2Deg;
				SniperAssist.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
				snipercharge -= Time.deltaTime;
				if (snipercharge < 0) {
					GameObject player1bullet = (GameObject)Instantiate (sniperbullet, transform.position, transform.rotation);
					player1bullet.GetComponent<Sniperbullet>().playercolor = playercolor;
					player1bullet.GetComponent<Owner>().playerowner = this.gameObject;
					var stickx = Input.GetAxis (xfirecontrol);
					var sticky = Input.GetAxis (yfirecontrol) * -1;
					player1bullet.GetComponent<Sniperbullet> ().direction = new Vector3 (stickx, sticky, 0);
					shotcd = 1f;
					fire = false;
					snipercharge = 2;
					SniperAssist.SetActive(false);
				}
			} else {
				SniperAssist.SetActive(false);
				snipercharge = 2;
			}
		}
	}

	void BoomerangFire(){
		if (boomerangreturned) {
			Vector2 rightstick = new Vector2(Input.GetAxis(xfirecontrol),Input.GetAxis(yfirecontrol));
			Vector3 direction = new Vector3(rightstick.x,rightstick.y, 0);
			if (direction.magnitude > deadzone) {
				var stickx = Input.GetAxis (xfirecontrol);
				var sticky = Input.GetAxis (yfirecontrol) * -1;
				GameObject bulletshot = (GameObject)Instantiate (BoomerangBullet, transform.position, transform.rotation);
				bulletshot.GetComponent<Owner>().playerowner = this.gameObject;
				bulletshot.GetComponent<BoomerangShot> ().direction = new Vector3 (stickx, sticky, 0);
				bulletshot.GetComponent<BoomerangShot>().playercolor = playercolor;
				boomerangreturned = false;
			}
		}
	}

	void RapidGunFire(){

		if (fire) {
			Vector2 rightstick = new Vector2(Input.GetAxis(xfirecontrol),Input.GetAxis(yfirecontrol));
			Vector3 direction = new Vector3(rightstick.x,rightstick.y, 0);
			if (direction.magnitude > deadzone) {
				var stickx = Input.GetAxis (xfirecontrol);
				var sticky = Input.GetAxis (yfirecontrol) * -1;
				GameObject bulletshot = (GameObject)Instantiate (bullet, transform.position, transform.rotation);
				bulletshot.GetComponent<Owner>().playerowner = this.gameObject;
				bulletshot.GetComponent<Bullet> ().direction = new Vector3 (stickx, sticky, 0);
				bulletshot.GetComponent<Bullet>().playercolor = playercolor;
				shotcd = .2f;
				fire = false;
			}
		}

	}

	void ControllerInputs(){
		if (fire) {
			Vector2 rightstick = new Vector2(Input.GetAxis("RightStickX"),Input.GetAxis("RightStickY"));
			Vector3 direction = new Vector3(rightstick.x,rightstick.y, 0);
			if (direction.magnitude > deadzone) {
				//var currentRotation = Quaternion.LookRotation(Vector3.forward, direction);
				//transform.rotation = Quaternion.Lerp(transform.rotation, currentRotation, Time.deltaTime * angularVelocity);
				GameObject player1bullet = (GameObject)Instantiate (bullet, transform.position, transform.rotation);
				player1bullet.GetComponent<Owner>().playerowner = this.gameObject;
				var stickx = Input.GetAxis ("RightStickX");
				var sticky = Input.GetAxis ("RightStickY") * -1;
				player1bullet.GetComponent<Bullet> ().direction = new Vector3 (stickx, sticky, 0);
				shotcd = .5f;
				fire = false;
			}
		}
	}

}
