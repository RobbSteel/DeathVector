using UnityEngine;
using System.Collections;

public class PlayerShields : MonoBehaviour {

	public GameObject deathmarker;
	public GameObject playeravatar;
	public VectorGrid m_VectorGrid;
	public float shieldstrength = 100;
	public bool dead = false;
	public bool suddendeath = false;
	private float respawntime = 2;
	public Color playercolor;
	public float kills;
	public float deaths;
	public UILabel killslabel;
	public UILabel deathslabel;

	// Use this for initialization
	void Start () {
		m_VectorGrid = GameObject.Find("VectorGrid").GetComponent<VectorGrid>();
	}
	
	// Update is called once per frame
	void Update () {
		if (dead) {
			if (respawntime < 0) {
				shieldstrength = 100;
				playeravatar.SetActive (true);
				dead = false;
				respawntime = 2;
			} else {
				respawntime -= Time.deltaTime;
			}
		}
	}

	void ShieldCheck(Collider2D col){

		if (col.gameObject.name == "SniperBullet(Clone)") {
			shieldstrength -= 110;
		}

		if (col.gameObject.name == "Explosion(Clone)") {
			shieldstrength -= 80;
		}

		if (col.gameObject.name == "Flamethrower") {
			shieldstrength -= 2;
		}

		if (col.gameObject.name == "Bullet(Clone)") {
			shieldstrength -= 25;
		}

		if (col.gameObject.name == "BoomerangShot(Clone)") {
			shieldstrength -= 60;
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (!dead) {
			if (col.transform.GetComponent<Owner> ().playerowner != this.gameObject || col.gameObject.name == "Explosion(Clone)") {
				ShieldCheck (col);
			}

			if (col.tag == "Damage" && shieldstrength <= 0 && col.transform.GetComponent<Owner> ().playerowner != this.gameObject) {
				Death ();
				CalculateKill(col);
			} else if (col.tag == "Damage" && shieldstrength <= 0 && col.gameObject.name == "Explosion(Clone)") {
				Death ();
				CalculateKill(col);
			}
		}
	}

	public void Death(){
		GameObject deathmark = (GameObject)Instantiate (deathmarker, transform.position, transform.rotation);
		deathmark.transform.GetChild (0).GetComponent<ParticleSystem> ().startColor = playercolor;
		deathmark.transform.GetChild (1).GetComponent<ParticleSystem> ().startColor = playercolor;
		m_VectorGrid.AddGridForce (transform.position, .5f, .5f, playercolor, true);
		dead = true;
		if (!suddendeath) {
			playeravatar.SetActive (false);
		} else {
			GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().suddendeathplayers--;
			Destroy(gameObject);
		}

	}
	
	void OnTriggerStay2D(Collider2D col){

		if (!dead) {
			if (col.transform.GetComponent<Owner> ().playerowner != this.gameObject && col.gameObject.name == "Flamethrower") {
				ShieldCheck (col);
			}

			if (col.tag == "Damage" && shieldstrength <= 0 && (col.transform.GetComponent<Owner> ().playerowner != this.gameObject || col.gameObject.name == "Mine(Clone)")) {
				Death ();
				CalculateKill(col);
			}
		}
	}

	public void KillAcquired(){
		kills++;
		killslabel.text = "" + kills;
	}

	void CalculateKill(Collider2D col){
		GameObject killer = col.transform.GetComponent<Owner> ().playerowner;
		PlayerShields playerstats = killer.GetComponent<PlayerShields> ();
		if (col.gameObject.name != "Explosion(Clone)") {
			playerstats.KillAcquired ();
		} else if(col.gameObject.name == "Explosion(Clone)" && killer != this.gameObject) {
			playerstats.KillAcquired ();
		}
		deaths++;
		deathslabel.text = "" + deaths;
	}
}
