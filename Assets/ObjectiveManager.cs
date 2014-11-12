using UnityEngine;
using System.Collections;

public class ObjectiveManager : Photon.MonoBehaviour {

	public GameObject Team1_Tower;
	public GameObject Team2_Tower;
	public float Team1_Wins = 0;
	public float Team2_Wins = 0;
	public float Next_round_time;
	public float round_number;
	public UILabel Map_Info;
	private bool ready_reset;
	public UILabel Team1_Victory;
	public UILabel Team2_Victory;


	// Use this for initialization
	void Start () {
	
	}

	void InitiateRound(){

	}

	public void TowerDown(int team_type){
		Next_round_time = 5;
		ready_reset = true;
		if (team_type == 1) {
			Team1_Wins++;
			photonView.RPC("TowerWin", PhotonTargets.All, team_type, Team1_Wins);
		} else {
			Team2_Wins++;
			photonView.RPC("TowerWin", PhotonTargets.All, team_type, Team2_Wins);
		}
	}

	[RPC]
	void TowerWin(int team_type, float Team_Wins){
		if (team_type == 1) {
			Team1_Victory.text = "Team 1: " + Team_Wins;
		} else {
			Team2_Victory.text = "Team 2: " + Team_Wins;
		}
	}

	[RPC]
	void RoundReset(){
		round_number++;
		Map_Info.text = "Round: " + round_number;
		Team1_Tower.SetActive (true);
		Team2_Tower.SetActive (true);
		Team1_Tower.GetComponent<Tower> ().ResetTower ();
		Team2_Tower.GetComponent<Tower> ().ResetTower ();
	}

	// Update is called once per frame
	void Update () {
		if (ready_reset) {
			if (Next_round_time < 0) {
				photonView.RPC("RoundReset", PhotonTargets.All);
				ready_reset = false;
			} else {
				Next_round_time -= Time.deltaTime;
			}
		}
	}
}
