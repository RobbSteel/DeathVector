using UnityEngine;
using System.Collections;

public class NetworkLogib : MonoBehaviour {

	private const string roomName = "RoomName";
	public UILabel hostgame;
	public UILabel playername;
	public GameObject player_information;


	void Start()
	{
		PhotonNetwork.ConnectUsingSettings("0.1");
	}

	void Update(){
	
	}



	void OnGUI()
	{
				if (!PhotonNetwork.connected) {
						GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
				} else if (PhotonNetwork.room == null) {
						//{
						// Create Room
						//if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
						//PhotonNetwork.CreateRoom("Gooby", true, true, 5);
				}
		}

	public void CreateRoom(){
		if (hostgame.text != "" && playername.text != "" && hostgame.text != "ServerName" && playername.text != "Player Name" && PhotonNetwork.room == null) {
			player_information.GetComponentInChildren<PlayerInfo>().playername = playername.text;
			string room_selection = hostgame.text;
			PhotonNetwork.CreateRoom (room_selection, true, true, 5);

		}
	}
	

	void OnJoinedRoom()
	{
		PhotonNetwork.isMessageQueueRunning = false;
		Debug.Log("Connected to Room");
		Debug.Log (PhotonNetwork.playerList.Length);
		Debug.Log (PhotonNetwork.room.name);
		Application.LoadLevel("GameRoomTest");
	}
}
