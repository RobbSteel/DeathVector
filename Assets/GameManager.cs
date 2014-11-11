using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	private float gametime = 60f;
	public UILabel Gametime;
	public UILabel WarningMessages;
	private float topkdratio;
	private GameObject winningplayer;
	private VectorGrid m_VectorGrid;
	private bool gamefinished = false;
	private bool tiegame = false;
	private bool suddendeath = false;
	public float suddendeathplayers;
	public float messagetime = 0f;
	private List<GameObject> tieplayers = new List<GameObject>();
	private List<GameObject> safeplayers = new List<GameObject>();
	private GameObject[] players;

	// Use this for initialization
	void Start () {
		m_VectorGrid = GameObject.Find("VectorGrid").GetComponent<VectorGrid>();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (suddendeath) {
			if(suddendeathplayers == 1){
				WarningMessages.text = "Victory!";
				WarningMessages.enabled = true;
				WarningMessages.alpha = 1;
				messagetime = 3f;
				suddendeath = false;
			}
		}

		if (messagetime <= 0) {
			WarningMessages.enabled = false;
		} else {
			WarningMessages.alpha -= .005f;
			messagetime -= Time.deltaTime;
		}

		if (gametime <= 0 && !gamefinished) {

			players = GameObject.FindGameObjectsWithTag("Player");
			for(int i = 0; i < players.Length; i++){
				float kills = players[i].GetComponent<PlayerShields>().kills;
				float deaths = players[i].GetComponent<PlayerShields>().deaths;
				float kdratio = kills - deaths;
				if(i == 0){
					topkdratio = kdratio;
					winningplayer = players[i];
				} else {
					if(topkdratio < kdratio){
						topkdratio = kdratio;
						winningplayer = players[i];
						if(tiegame){
							tieplayers.Clear();
							tiegame = false;
						}
					} else if(topkdratio == kdratio && !tiegame){
						tieplayers.Add(winningplayer);
						tieplayers.Add(players[i]);
						tiegame = true;
					} else if(topkdratio == kdratio && tiegame){
						tieplayers.Add (players[i]);
					}
				}
			}

			for(int j = 0; j < players.Length; j++){
				if(!tiegame){
					if(players[j] != winningplayer){
						players[j].GetComponent<PlayerShields>().Death();
						Destroy (players[j]);
					}
				} else {
					for(int l = 0; l < tieplayers.Count; l++){
						if(players[j] == tieplayers[l]){
							players[j].GetComponent<PlayerShields>().suddendeath = true;
							players[j] = null;
						}
					}
				}
			}

			if(tiegame){
				for(int t = 0; t < players.Length; t++){
					if(players[t] != null){
						players[t].GetComponent<PlayerShields>().Death();
						Destroy(players[t]);
					}

				}
				suddendeathplayers = tieplayers.Count;
				suddendeath = true;
				messagetime = 3;
				WarningMessages.enabled = true;
				WarningMessages.alpha = 1;
			} else {
				WarningMessages.text = "Victory!";
				WarningMessages.enabled = true;
				WarningMessages.alpha = 1;
				messagetime = 3f;
			}
			gamefinished = true;
			Gametime.enabled = false;

		


		} else if(!gamefinished){
			gametime -= Time.deltaTime;
			string minutes = Mathf.Floor(gametime / 60).ToString();
			string seconds = Mathf.Floor(gametime % 60).ToString("00");
			Gametime.text = ""+ minutes +":" + seconds;
		}
	}
}
