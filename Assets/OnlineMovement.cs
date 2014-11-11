﻿using UnityEngine;
using System.Collections;

public class OnlineMovement : Photon.MonoBehaviour {

	public float movementspeed = 1;
	public bool control_enabled = false;
	private float deadzone = .25f;
	public VectorGrid m_VectorGrid;

	private bool fire = true;
	public bool dead = false;
	private float shotcd = 0;
	private float snipercharge = .5f;
	public GameObject SniperAssist;
	public GameObject sniperbullet;

	private string xaxiscontrol;
	private string yaxiscontrol;
	private string xfirecontrol;
	private string yfirecontrol;
	private string ability1control;
	private string ability2control;
	private string ability3control;
	private Color playercolor;

	// Use this for initialization
	void Start () {
		m_VectorGrid = GameObject.Find("VectorGrid").GetComponent<VectorGrid>();
		AssignControls ();
	}
	
	void AssignControls(){
			xaxiscontrol = "Horizontal";
			yaxiscontrol = "Vertical";
			xfirecontrol = "RightStickX";
			yfirecontrol = "RightStickY";
			ability1control = "FirstAbility";
			ability2control = "SecondAbility";
	}
	// Update is called once per frame
	void Update () {
		if (control_enabled && !dead) {
			Movement ();
			ShotCooldown ();
			SniperShot();
		} 
	}
	
	public void AssignColor(string player_ID){
		Color color = Color.blue;
		switch (player_ID) {
			case "Blue": color = Color.blue; gameObject.tag = "Team1"; break;
			case "Cyan" : color = Color.cyan; gameObject.tag = "Team1"; break;
			case "Green": color = Color.green; gameObject.tag = "Team1"; break;
			case "Red" : color = Color.red; gameObject.tag = "Team2"; break;
			case "Yellow": color = Color.yellow; gameObject.tag = "Team2"; break;
			case "Magenta" : color = Color.magenta; gameObject.tag = "Team2"; break;
		}
		gameObject.GetComponentInChildren<ParticleSystem> ().startColor = color;
		gameObject.GetComponent<OnlineShields> ().playercolor = color;
		playercolor = color;
		photonView.RPC ("ColorSync", PhotonTargets.Others, player_ID);
	}

	[RPC] 
	void ColorSync(string player_ID){
		Color color = Color.blue;
		switch (player_ID) {
			case "Blue": color = Color.blue; gameObject.tag = "Team1"; break;
			case "Cyan" : color = Color.cyan; gameObject.tag = "Team1"; break;
			case "Green": color = Color.green; gameObject.tag = "Team1"; break;
			case "Red" : color = Color.red; gameObject.tag = "Team2"; break;
			case "Yellow": color = Color.yellow; gameObject.tag = "Team2"; break;
			case "Magenta" : color = Color.magenta; gameObject.tag = "Team2"; break;
		}
		gameObject.GetComponentInChildren<ParticleSystem> ().startColor = color;
		playercolor = color;
	}
	
	void Movement(){
		Vector2 leftstick = new Vector2(Input.GetAxis(yaxiscontrol),Input.GetAxis(xaxiscontrol));
		Vector3 stickdirection = new Vector3(leftstick.x,leftstick.y, 0);
		if (stickdirection.magnitude > deadzone) {
			transform.Translate (0, Input.GetAxis (yaxiscontrol) * Time.deltaTime * movementspeed, 0);
			transform.Translate (Input.GetAxis (xaxiscontrol) * Time.deltaTime * movementspeed, 0, 0);
		}
	}

	void ShotCooldown(){
		if (shotcd < 0) {
			fire = true;
		} else {
			shotcd -= Time.deltaTime;
		}
	}

	[RPC]
	void SniperRelay(Vector3 direction){
	  GameObject player1bullet = (GameObject)Instantiate (sniperbullet, transform.position, transform.rotation);
	  player1bullet.GetComponent<Sniperbullet>().playercolor = playercolor;
	  player1bullet.GetComponent<Sniperbullet>().m_VectorGrid = m_VectorGrid;
	  player1bullet.GetComponent<Owner>().playerowner = this.gameObject;
	  player1bullet.GetComponent<Sniperbullet> ().direction = direction;
	}

	[RPC]
	void SniperAssistance(bool sniper_activation){
		if (sniper_activation) {
			SniperAssist.SetActive (true);
		} else {
			SniperAssist.SetActive(false);
		}
	}

	void SniperShot(){
		if (fire) {
			Vector2 rightstick = new Vector2 (Input.GetAxis (xfirecontrol), Input.GetAxis (yfirecontrol));
			Vector3 direction = new Vector3 (rightstick.x, rightstick.y, 0);
			if (direction.magnitude > deadzone) {
				SniperAssist.SetActive(true);
				//photonView.RPC("SniperAssistance", PhotonTargets.Others, true);
				var sniperx = Input.GetAxis (xfirecontrol);
				var snipery = Input.GetAxis (yfirecontrol) * -1;
				var angle = Mathf.Atan2 (snipery, sniperx) * Mathf.Rad2Deg;
				SniperAssist.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
				snipercharge -= Time.deltaTime;
				if (snipercharge < 0) {
					GameObject player1bullet = (GameObject)Instantiate (sniperbullet, transform.position, transform.rotation);
					player1bullet.GetComponent<Sniperbullet>().playercolor = playercolor;
					player1bullet.GetComponent<Sniperbullet>().m_VectorGrid = m_VectorGrid;
					player1bullet.GetComponent<Owner>().playerowner = this.gameObject;
					var stickx = Input.GetAxis (xfirecontrol);
					var sticky = Input.GetAxis (yfirecontrol) * -1;
					var stickdirection = new Vector3(stickx, sticky, 0);
					player1bullet.GetComponent<Sniperbullet> ().direction = stickdirection;
					photonView.RPC("SniperRelay", PhotonTargets.Others, stickdirection); 
					shotcd = .1f;
					fire = false;
					snipercharge = 2f;
					SniperAssist.SetActive(false);
					//photonView.RPC("SniperAssistance", PhotonTargets.Others, false);
				}
			} else {
				SniperAssist.SetActive(false);
				//photonView.RPC("SniperAssistance", PhotonTargets.Others, false);
				snipercharge = 2f;
			}
		}
	}




}