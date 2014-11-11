using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInfo : MonoBehaviour {

	public string playername;
	public GameObject current_slot;
	public string player_color;
	public string team;
	public int team_numb;
	public string weapon_type;
	public string ability_one;
	public string ability_two;
	public List<Player> team1_list = new List<Player>();
	public List<Player> team2_list = new List<Player>();

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
	}

	void Awake(){
	}

	// Update is called once per frame
	void Update () {
	
	}
}
