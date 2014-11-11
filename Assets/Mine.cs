using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {
	
	public GameObject Explosion;
	public float armingtimer = 2;
	public bool armed = false;
	public VectorGrid m_VectorGrid;
	public Color playercolor;
	// Use this for initialization

	void Start () {
		m_VectorGrid = GameObject.Find("VectorGrid").GetComponent<VectorGrid>();
	}
	
	// Update is called once per frame
	void Update () {
		if (armingtimer < 0) {
			armed = true;
		} else {
			armingtimer -= Time.deltaTime;
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		print ("collision");
		if (armed && col.tag == "Player") {
			GameObject newexplosion = (GameObject)Instantiate(Explosion, transform.position, transform.rotation);
			newexplosion.GetComponent<Owner>().playerowner = gameObject.GetComponent<Owner>().playerowner;
			m_VectorGrid.AddGridForce(transform.position, .5f, .5f, playercolor, true);
			Destroy(newexplosion, .5f);
			Destroy(gameObject);
		}
	}
}
