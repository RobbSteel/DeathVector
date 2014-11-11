using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	private float speed = 2;
	public Vector3 direction;
	public VectorGrid m_VectorGrid;
	public Color playercolor;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, 3f);
		m_VectorGrid = GameObject.Find("VectorGrid").GetComponent<VectorGrid>();
		gameObject.particleSystem.startColor = playercolor;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (direction.normalized * speed * Time.deltaTime);
		m_VectorGrid.AddGridForce(transform.position, .05f, .05f, playercolor, true);
	}
}
