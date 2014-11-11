using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public struct Player{
	public string color;
	public string playername;
}


public class BeginMatch : Photon.MonoBehaviour {

	private float countdown = 5;
	private bool count_started = false;
	public UILabel countdown_label;
	public UILabel button_label;
	public List<Player> team1_list = new List<Player>();
	public List<Player> team2_list = new List<Player>();
	public GameObject[] team1;
	public GameObject[] team2;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (count_started) {
			if(countdown < 0){
				if(PhotonNetwork.isMasterClient){
					photonView.RPC("StartMatch", PhotonTargets.All);
				}
			} else {
				countdown -= Time.deltaTime;
				string seconds = Mathf.Ceil(countdown % 60).ToString("00");
				countdown_label.text = seconds;
			}
		}
	}
	

	void ActivePlayers(){



		for(int i = 0; i < team1.Length; i++){
			if(team1[i].GetComponent<UILabel>().text != "Open"){
				Player player;
				player.color = team1[i].GetComponent<PlayerSlot>().color;
				player.playername = team1[i].GetComponent<UILabel>().text;
				team1_list.Add(player);
			}
		}

		for(int j = 0; j < team1.Length; j++){
			if(team2[j].GetComponent<UILabel>().text != "Open"){
				Player player;
				player.color = team2[j].GetComponent<PlayerSlot>().color;
				player.playername = team2[j].GetComponent<UILabel>().text;
				print (player.color);
				team2_list.Add(player);
			}
		}

		GameObject player_info = GameObject.FindGameObjectWithTag ("PlayerInfo");
		player_info.GetComponent<PlayerInfo> ().team1_list = team1_list;
		player_info.GetComponent<PlayerInfo> ().team2_list = team2_list;
	}

	[RPC]
	void StartMatch(){
		ActivePlayers();
		PhotonNetwork.isMessageQueueRunning = false;
		Application.LoadLevel("OnlineTest");
	}

	[RPC]
	void StopMatch(){
		countdown_label.enabled = false;
		countdown = 5;
		count_started = false;
		button_label.text = "Start Game";
	}

	[RPC]
	void CountStart(){
		countdown_label.enabled = true;
		count_started = true;
		button_label.text = "Cancel";
	}


	public void StartGame(){
		if (!count_started) {
			photonView.RPC ("CountStart", PhotonTargets.All);

		} else if (count_started) {
			photonView.RPC("StopMatch", PhotonTargets.All);
		}
	}
}
