using UnityEngine;
using System.Collections;

public class Sniperbullet : MonoBehaviour {
	
	private float speed = 7;
	public Vector3 direction;
	public VectorGrid m_VectorGrid;
	public Color playercolor;
	public GameObject shottrail;
	
	// Use this for initialization
	void Start () {
		Destroy (gameObject, 2f);
		gameObject.particleSystem.startColor = playercolor;
		shottrail.particleSystem.startColor = playercolor;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (direction.normalized * speed * Time.deltaTime);
		m_VectorGrid.AddGridForce(transform.position, .2f, .2f, playercolor, true);
	}
}