using UnityEngine;
using System.Collections;

public class PlayerAbilities : MonoBehaviour {

	public bool fire = false;
	public bool machinegun = false;
	public bool sniper = false;
	public bool boomerang = false;
	public string ability1;
	public string ability2;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
