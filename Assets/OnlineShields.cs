using UnityEngine;
using System.Collections;

public class OnlineShields : Photon.MonoBehaviour {

	public VectorGrid m_VectorGrid;
	public Color playercolor;
	public UILabel kill_label;
	public UILabel death_label;
	public int kills;
	public int deaths;
	private float teleport_time = 3;
	private float death_timer;
	private float immunity_time;
	public GameObject Current_Camera;
	public GameObject PlayerDeath;
	public GameObject Player_Avatar;
	public OnlineMovement player_movement;
	public Collider2D player_collider;
	private bool dead = false;
	private bool immune = false;
	private float shields_left = 100;
	private float shields_display = 0;
	public BuildCircleMesh circular_shield;
	public GameObject Immune_Shield;


	
	void Start(){
		m_VectorGrid = GameObject.Find("VectorGrid").GetComponent<VectorGrid>();
	}
	
	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Damage" && photonView.isMine && !immune) {
			if (col.GetComponent<Owner> ().playerowner != gameObject && col.GetComponent<Owner>().playerowner.tag != gameObject.tag) {	
				ShieldsCalculate(col.gameObject);
				if(shields_left < 0){
					Death ();
				}
				if(col.GetComponent<Owner>().playerowner.GetComponent<OnlineShields>() != null){;
					col.GetComponent<Owner>().playerowner.GetComponent<OnlineShields>().PlayerKill();
				}
			}
		}
	}

	void OnTriggerStay2D(Collider2D col){
	
	}

	void OnTriggerExit2D(Collider2D col){
	
	}

	void ShieldsCalculate(GameObject bullet){
		float damage = 0;
		float shield_hit = 0;
		switch (bullet.name) {
			case "SniperBullet(Clone)" : damage = 50; shield_hit = 180; break;
			case "Bullet(Clone)" : damage = 30; shield_hit = 108; break;
		}
		shields_left -= damage;
		shields_display += shield_hit;
		photonView.RPC ("RelayShields", PhotonTargets.All, shields_display);
	}

	[RPC]
	void RelayShields(float shields_left){
		circular_shield.startAngle = shields_left;
	}

	public void SetUpKillDeath(GameObject ui_information){
		var label_array = ui_information.GetComponentsInChildren<UILabel> ();
		kill_label = label_array [1];
		death_label = label_array [2];
	}

	public void PlayerKill(){
		photonView.RPC ("Kill", PhotonTargets.All);
	}

	[RPC]
	void Kill(){
		kills++;
		kill_label.text = "" + kills;
	}

	[RPC]
	void DeathRelay(){
		GameObject.Instantiate (PlayerDeath, transform.position, transform.rotation);
		Player_Avatar.SetActive (false);
		m_VectorGrid.AddGridForce (transform.position, .5f, .5f, playercolor, true);
		deaths++;
		death_label.text = "" + deaths;
	}

	public void Death(){
		player_collider.enabled = false;
		photonView.RPC ("DeathRelay", PhotonTargets.All);
		death_timer = 2;
		dead = true;
		player_movement.dead = true;
	}

	[RPC]
	void Revive(){
		circular_shield.startAngle = 0;
		Player_Avatar.SetActive (true);
		Immune_Shield.SetActive (true);
	}

	[RPC]
	void ShieldRemove(){
		Immune_Shield.SetActive (false);
	}

	void Update(){
		if (dead) {
			if(death_timer < 0){
				photonView.RPC ("Revive", PhotonTargets.All);
				shields_left = 100;
				shields_display = 0;
				dead = false;
				player_movement.dead = false;
				immune = true;
				immunity_time = 2;
				player_collider.enabled = true;
			} else {
				death_timer -= Time.deltaTime;
			}
		}

		if (immune) {
			if(immunity_time < 0){
				immune = false;
				photonView.RPC("ShieldRemove", PhotonTargets.All);
			} else {
				immunity_time -= Time.deltaTime;
			}
		}
	}
}
