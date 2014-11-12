using UnityEngine;
using System.Collections;

public class Tower : Photon.MonoBehaviour {

	public GameObject Tower_Shot;
	public GameObject explosion;
	public GameObject tower_hit;
	private bool shot_able = true;
	public float Shot_Direction;
	public float shot_timer = 3;
	public float shields_left = 100;
	private float shields_display = 0;
	public BuildCircleMesh circular_shield;
	public ObjectiveManager game_manager;
	public VectorGrid m_VectorGrid;
	public Color tower_color;
	public string Tower_Type;

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter2D(Collider2D col){
		print ("hit");
		if (PhotonNetwork.isMasterClient) {
			if (col.tag == "Damage") {
				if (col.GetComponent<Owner> ().playerowner != gameObject && col.GetComponent<Owner> ().playerowner.tag != gameObject.tag) {	
					photonView.RPC ("TowerHit", PhotonTargets.AllBuffered, col.transform.position);
				}
			}
		}
	}

	[RPC]
	void TowerHit(Vector3 hit_marker){
		Instantiate (tower_hit, hit_marker, transform.rotation);
		if(shields_left == 0){
			Instantiate(explosion, transform.position, transform.rotation);
			 if(PhotonNetwork.isMasterClient){
				game_manager.TowerDown();
			 }
			m_VectorGrid.AddGridForce(transform.position, 1f, 1f, tower_color, true);
			gameObject.SetActive(false);
		}
		shields_left -= 10;
		shields_display += 36;
		circular_shield.startAngle = shields_display;
	}

	[RPC]
	void TowerShot(Vector3 target_direction){
		GameObject tower_projectile = (GameObject)Instantiate (Tower_Shot, gameObject.transform.position, gameObject.transform.rotation);
		tower_projectile.GetComponent<Bullet> ().direction = target_direction;
		tower_projectile.GetComponent<Owner> ().playerowner = gameObject;
	}

	Vector3 FindTarget(){
		print (Tower_Type);
		GameObject[] playertargets = GameObject.FindGameObjectsWithTag (Tower_Type);
		float target_distance = 100;
		Vector3 target_position = new Vector3 (0, 0, 0);
		for (int i = 0; i < playertargets.Length; i++) {
			print (playertargets[i]);
			if(Vector3.Distance(playertargets[i].gameObject.transform.position, gameObject.transform.position) < target_distance){
				target_distance = Vector3.Distance(playertargets[i].transform.position, transform.position);
				target_position = playertargets[i].transform.position;
			}
		}
		target_position = target_position - gameObject.transform.position;
		return target_position;
	}

	public void ResetTower(){
		shields_left = 100;
		shields_display = 0;
		circular_shield.startAngle = 0;
	}

	// Update is called once per frame
	void Update () {
		if (PhotonNetwork.isMasterClient) {
			if (shot_able) {
				if (shot_timer < 0) {
					Vector3 target_direction = FindTarget();
					photonView.RPC("TowerShot", PhotonTargets.AllBufferedViaServer, target_direction);
					shot_timer = 3;
				} else {
					shot_timer -= Time.deltaTime;
				}
			}
		}
	}
}
