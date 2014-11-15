using UnityEngine;
using System.Collections;

public class Flamethrower : MonoBehaviour {

	private VectorGrid m_VectorGrid;
	public Color playercolor;
	public GameObject innerflamethrower;

	// Use this for initialization
	void Start () {
		m_VectorGrid = GameObject.Find ("VectorGrid").GetComponent<VectorGrid>();
	}
	
	// Update is called once per frame
	void Update () {
		m_VectorGrid.AddGridForce (innerflamethrower.transform.position, .1f, .2f, playercolor, true);
		transform.localPosition = Vector3.zero;
	}
	

}
