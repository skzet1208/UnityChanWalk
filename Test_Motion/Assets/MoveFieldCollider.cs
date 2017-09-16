using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFieldCollider : MonoBehaviour
{
	public GameObject onFieldPlayer;

	void OnCollisionEnter (Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts) {
			if (contact.otherCollider.gameObject.tag == "Player") {
				onFieldPlayer.transform.parent = transform.parent;
			}
		}
	}

	void OnCollisionExit (Collision collision)
	{
		onFieldPlayer.transform.parent = null;
	}
}
