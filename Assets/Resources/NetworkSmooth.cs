using UnityEngine;
using System.Collections;

public class NetworkSmooth : Photon.MonoBehaviour {
	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;

	private Vector3 correctPlayerPos;
	private Quaternion correctPlayerRot;
		
		// Update is called once per frame
	void Update()
	{
		if (!photonView.isMine)
		{
			transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 10);
			transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 10);
		}
	}
	

		
		void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			
		Vector3 syncPosition = Vector3.zero;
		Vector3 syncVelocity = Vector3.zero;
		if (stream.isWriting)
			{
				// We own this player: send the others our data
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
				
			}
			else
			{
			this.correctPlayerPos = (Vector3)stream.ReceiveNext();
			this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
			syncDelay = Time.deltaTime - lastSynchronizationTime;
			lastSynchronizationTime = Time.deltaTime;
			
			syncEndPosition = syncPosition + syncVelocity * syncDelay;
			syncStartPosition = rigidbody2D.position;
			}
		}

	}