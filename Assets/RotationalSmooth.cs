using UnityEngine;
using System.Collections;

public class RotationalSmooth : Photon.MonoBehaviour {
		
		private Vector3 correctPlayerPos;
		private Quaternion correctPlayerRot;
		private Vector2 correctPlayerVel;
		
		// Update is called once per frame
		void Update()
		{
			if (!photonView.isMine)
			{
				//UpdatePositions();
				

				transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 20);
			}
		}
		
		
		
		void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			
			if (stream.isWriting)
			{
				// We own this player: send the others our data
				stream.SendNext(transform.rotation);
				//stream.SendNext(rigidbody2D.velocity);
				
			}
			else
			{

				this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
		
			}
		}
	}

