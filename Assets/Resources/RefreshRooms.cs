using UnityEngine;
using System.Collections;

public class RefreshRooms : MonoBehaviour {

	private RoomInfo[] roomsList;
	public GameObject server_info;

	void OnReceivedRoomListUpdate()
	{
		roomsList = PhotonNetwork.GetRoomList();
	}

	public void RefreshRoom(){
		if (roomsList != null) {
			for(int i = 0; i < roomsList.Length; i++){
				GameObject room_label = (GameObject)Instantiate(server_info, new Vector3(100, 250 + (110 * i), 0), gameObject.transform.rotation);
				room_label.transform.localScale = new Vector3(6,6,6);
				room_label.GetComponentInChildren<UILabel>().text = roomsList[i].name + " " + roomsList[i].playerCount + " / " + roomsList[i].maxPlayers;
				room_label.GetComponentInChildren<JoinRoom>().room_name = roomsList[i].name;
			}
		}
	}

}
