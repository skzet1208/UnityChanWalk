using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleAnimation : MonoBehaviour
{
	// Public
	public float speed;
	public float jumpLateStartTime;

	// Private
	private float speedMultiple = 1.0f;
	private bool jumpFlg = false;

	// Animator Component
	private Animator animator;
	private Rigidbody rb;

	// Motion flag
	private const string key_isWalk = "isWalk";
	private const string key_isRun = "isRun";
	private const string key_isJump = "isJump";

	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody> ();

		// Get Animator
		animator = GetComponent<Animator> ();

	}
	
	// Update is called once per frame
	void Update ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal"); // x:左右
		float moveVertical = Input.GetAxis ("Vertical"); // z:前後

		// キャラクター操作
		characterControl ();

		// Run Speed
		speedMultiple = (Input.GetAxis ("Run") != 0) ? 2.2f : 1.0f;

		// 自身の向きベクトル取得(ラジアン角)
		if (moveHorizontal != 0) {
			float turnDirection = moveHorizontal > 0 ? 1.0f : -1.0f;
			transform.Rotate (new Vector3 (0, (1.2f * turnDirection * speedMultiple), 0));
		}
		float angleDir = transform.eulerAngles.y * (Mathf.PI / 180.0f);

		Vector3 movement = new Vector3 (Mathf.Sin (angleDir), 0.0f, Mathf.Cos (angleDir));

		if (moveVertical > 0) {
			// Time.deltaTimeで動作にムラがないように
//			transform.position += transform.forward * 0.01f * speed * Time.deltaTime * speedMultiple;

			rb.AddForce (movement * speed * speedMultiple);
		}
	}

	void OnCollisionEnter (Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts) {
			if (contact.otherCollider.gameObject.tag == "Field" && jumpFlg) {
				jumpFlg = false;
			}
		}
	}

	IEnumerator LateStart (float time)
	{
		yield return new WaitForSeconds (time);
		rb.useGravity = true;
	}

	void characterControl ()
	{
		float moveVertical = Input.GetAxis ("Vertical"); // z:前後

		// Check Push Shift Key
		bool isShift = Input.GetAxis ("Run") > 0;

		// Walk & Run
		if (moveVertical > 0.0f && isShift) {
			animator.SetBool (key_isRun, true);
			animator.SetBool (key_isWalk, false);
		} else if (moveVertical > 0.0f && !isShift) {
			animator.SetBool (key_isWalk, true);
			animator.SetBool (key_isRun, false);
		} else {
			animator.SetBool (key_isRun, false);
			animator.SetBool (key_isWalk, false);
		}

		// Jump
		if (Input.GetKey (KeyCode.Space)) {
			animator.SetBool (key_isJump, true);
			rb.useGravity = false;
			jumpFlg = true;

			// 重力フラグ判定
			StartCoroutine (LateStart (jumpLateStartTime));
		} else {
			animator.SetBool (key_isJump, false);
		}
	}
}
