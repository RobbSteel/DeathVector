using UnityEngine;
using System.Collections;

public class BoomerangShot : MonoBehaviour {

	private float speed = 2f;
	private float returntime = .8f;
	private bool firingout = true; 
	public GameObject playershooter;
	private PlayerShields playershields;
	public Vector3 direction;
	public VectorGrid m_VectorGrid;
	public Color playercolor;

	// Use this for initialization
	void Start () {
		m_VectorGrid = GameObject.Find("VectorGrid").GetComponent<VectorGrid>();
		playershooter = gameObject.GetComponent<Owner> ().playerowner;
		playershields = playershooter.GetComponent<PlayerShields> ();
		gameObject.particleSystem.startColor = playercolor;
	}
	
	// Update is called once per frame
	void Update () {

		if (playershields.dead) {
			Destroy(gameObject);
		}

		if (firingout) {
			if (returntime < 0) {
				firingout = false;
			} else {
				transform.Translate (direction.normalized * speed * Time.deltaTime);
				m_VectorGrid.AddGridForce(transform.position, .1f, .1f, playercolor, true);
				returntime -= Time.deltaTime;
			}
		} else {
			transform.position = Vector3.MoveTowards(gameObject.transform.position, playershooter.transform.position, speed * Time.deltaTime);
			m_VectorGrid.AddGridForce(transform.position, .1f, .1f, playercolor, true);
			if(transform.position == playershooter.transform.position){
				playershooter.GetComponent<PlayerMovement>().boomerangreturned = true;
				Destroy (gameObject);
			}
		}
	}
}
