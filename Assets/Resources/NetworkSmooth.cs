using UnityEngine;
using System.Collections;

public class NetworkSmooth : Photon.MonoBehaviour {
	private double lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;

	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
	private Vector2 correctPlayerVel;
		
		// Update is called once per frame
	void Update()
	{
		if (!photonView.isMine)
		{
			//UpdatePositions();

			transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 10);
			transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 10);
		}
	}
	

		
		void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			
		if (stream.isWriting)
			{
				// We own this player: send the others our data
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			//stream.SendNext(rigidbody2D.velocity);
				
			}
			else
			{
			this.correctPlayerPos = (Vector3)stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
			//this.correctPlayerVel = (Vector2)stream.ReceiveNext();
			//syncDelay = Time.deltaTime - lastSynchronizationTime;
			//lastSynchronizationTime = Time.deltaTime;
			
			lastSynchronizationTime = info.timestamp;
			}
		}
	/*
	void UpdatePositions(){
		float total_ping = (float)PhotonNetwork.GetPing () * 0.001f;
		float last_time = (float)(PhotonNetwork.time - lastSynchronizationTime);
		float total_time = total_ping + last_time;

		Vector3 exterpolation_position = correctPlayerPos + transform.forward * total_time;
		Vector3 newPosition = Vector3.MoveTowards (transform.position, exterpolation_position, Time.deltaTime);

		if (Vector3.Distance (transform.position, exterpolation_position) > 2f) {
			newPosition = exterpolation_position;
		}

		transform.position = newPosition;
	}
*/
	}