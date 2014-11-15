using UnityEngine;
using System.Collections;

public class OnlineJoin : Photon.MonoBehaviour {

	public GameObject Online_player;
	public GameObject[] team1_list;
	public GameObject[] team2_list;

	// Use this for initialization
	void Start () {
		PhotonNetwork.isMessageQueueRunning = true;
		GameObject Start_Camera = GameObject.FindGameObjectWithTag ("MainCamera");
		GameObject player_object = (GameObject)Instantiate(Online_player, Vector3.zero, Quaternion.identity);
		Start_Camera.transform.parent = player_object.transform;
		Start_Camera.transform.position = new Vector3(player_object.transform.position.x, player_object.transform.position.y, -.5f);
		PlayerInfo player_info = GameObject.FindGameObjectWithTag ("PlayerInfo").GetComponent<PlayerInfo> ();
		OnlineMovement player_movement = player_object.GetComponent<OnlineMovement> ();
		OnlineShields player_shields = player_object.GetComponent<OnlineShields> ();
		player_shields.Current_Camera = Start_Camera;
		string player_color = player_info.player_color;
		GameObject flame_thrower = player_movement.flamethrower;
		int character_id = PhotonNetwork.AllocateViewID();
		int fire_id = PhotonNetwork.AllocateViewID ();
		PhotonView[] nViews = player_object.GetComponentsInChildren<PhotonView>();
		nViews[0].viewID = character_id;
		nViews [1].viewID = fire_id;
		flame_thrower.SetActive (false);
		player_movement.control_enabled = true;
		player_movement.AssignColor(player_color);
		flame_thrower.GetComponent<Flamethrower> ().playercolor = player_movement.playercolor;
		photonView.RPC ("SpawnCharacter", PhotonTargets.OthersBuffered, character_id, player_color, fire_id);
		AssignUI (player_info, player_shields);
	}

	void AssignUI(PlayerInfo player_info, OnlineShields player_shields){
	
		for(int i = 0; i < player_info.team1_list.Count; i++){
			team1_list[i].GetComponent<UILabel>().text = player_info.team1_list[i].playername;
			team1_list[i].GetComponent<UILabel>().color = UIColorCheck(player_info.team1_list[i].color); 
			if(player_shields.playercolor == UIColorCheck(player_info.team1_list[i].color)){
			player_shields.SetUpKillDeath(team1_list[i]);	
			}
		}

		for(int i = 0; i < player_info.team2_list.Count; i++){
			team2_list[i].GetComponent<UILabel>().text = player_info.team2_list[i].playername;
			team2_list[i].GetComponent<UILabel>().color = UIColorCheck(player_info.team2_list[i].color);
			if(player_shields.playercolor == UIColorCheck(player_info.team2_list[i].color)){
				player_shields.SetUpKillDeath(team2_list[i]);	
			}
		}
	}

	Color UIColorCheck(string color){
		Color player_color = Color.blue;
		switch (color) {
		    case "Blue": player_color = Color.blue; break;
			case "Cyan" : player_color = Color.cyan; break;
			case "Green": player_color = Color.green; break;
			case "Red" : player_color = Color.red;  break;
			case "Yellow": player_color = Color.yellow;  break;
			case "Magenta" : player_color = Color.magenta; break;
		}
		return player_color;
	}

	[RPC]
	void SpawnCharacter(int character_id, string player_color, int fire_id){
		GameObject player_object = (GameObject)Instantiate (Online_player, Vector3.zero, Quaternion.identity);
		PlayerInfo player_info = GameObject.FindGameObjectWithTag ("PlayerInfo").GetComponent<PlayerInfo> ();
		OnlineMovement player_movement = player_object.GetComponent<OnlineMovement> ();
		OnlineShields player_shields = player_object.GetComponent<OnlineShields> ();
		PhotonView[] nViews = player_object.GetComponentsInChildren<PhotonView>();
		GameObject flame_thrower = player_movement.flamethrower;
		nViews[0].viewID = character_id;
		nViews [1].viewID = fire_id;
		flame_thrower.SetActive (false);
		player_object.GetComponent<OnlineMovement> ().AssignColor(player_color);
		flame_thrower.GetComponent<Flamethrower> ().playercolor = player_movement.playercolor;
		AssignUI (player_info, player_shields);
	}

	void OnGUI()
	{
		int ping = PhotonNetwork.GetPing();
		GUILayout.Label(ping.ToString());
	}

}
