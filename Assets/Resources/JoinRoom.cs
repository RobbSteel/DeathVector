using UnityEngine;
using System.Collections;

public class JoinRoom : MonoBehaviour {

	public string room_name;
	private GameObject player_name;
	private PlayerInfo player_info;

		void Start(){
			player_name = GameObject.FindGameObjectWithTag ("PlayerName");
			player_info = GameObject.FindGameObjectWithTag ("PlayerInfo").GetComponent<PlayerInfo>();
		}

	public void Join_Room(){
		string player_tag = player_name.GetComponent<UILabel> ().text; 
		if (player_tag != "" && player_tag != "Player Name" && PhotonNetwork.room == null) {
			player_info.playername = player_tag; 
			PhotonNetwork.JoinRoom (room_name);
		}

	}
}
