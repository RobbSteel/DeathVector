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


	// Use this for initialization
	void Start () {
	
	}

	void InitiateRound(){

	}

	public void TowerDown(){
		Next_round_time = 5;
		ready_reset = true;
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
