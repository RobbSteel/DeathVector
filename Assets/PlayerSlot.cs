using UnityEngine;
using System.Collections;

public class PlayerSlot : Photon.MonoBehaviour {

	public UILabel player_slot;
	private PlayerInfo player_information;
	public string color;
	public int team_numb;

	// Use this for initialization
	void Start () {
		player_information = GameObject.FindGameObjectWithTag ("PlayerInfo").GetComponent<PlayerInfo>();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void UnSlotPlayer(){
		photonView.RPC ("PlayerUnSlotting", PhotonTargets.AllBuffered);
	}
		             
	[RPC]
	public void PlayerUnSlotting(){
		gameObject.GetComponent<UILabel>().text = "Open";
	}

	public void SlotPlayer(){
		if (player_slot.text == "Open") {
			if(player_information.current_slot != null){
				player_information.current_slot.GetComponent<PlayerSlot>().UnSlotPlayer();
			}
			string playername = player_information.playername;
			player_slot.text = playername;
			player_information.current_slot = gameObject;
			player_information.player_color = color;
			player_information.team_numb = team_numb;
			if(gameObject.tag == "Team1"){
				player_information.team = "Team1";
			} else {
				player_information.team = "Team2";
			}
			photonView.RPC ("PlayerSlotting", PhotonTargets.OthersBuffered, playername);
		}
	}

	[RPC]
	void PlayerSlotting(string player_name){
		this.gameObject.GetComponent<UILabel>().text = player_name;
		print ("herrrrro");
	}


}
