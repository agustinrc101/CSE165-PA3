using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehavior : MonoBehaviour {
	private GameObject gm;
	private GameObject player;
	private Collider col;
	private bool isCur = false;

	void Awake () {
		gm = GameObject.FindGameObjectWithTag("GM");
		player = GameObject.FindGameObjectWithTag("Player");
		col = GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (player == null)
			player = GameObject.FindGameObjectWithTag("Player");
		else {
			if (isCur && Vector3.Distance(player.transform.position, transform.position) > 10.0f)
				transform.LookAt(player.transform);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("PlayerCollider")) {
			if (gm == null)
				gm = GameObject.FindGameObjectWithTag("GM");
			gm.GetComponent<Parser>().nextWaypoint();
		}
	}

	public void setCollider(bool b) {col.enabled = b; }

	public void setLineDestination(Vector3 endPos) {
		LineRenderer lr = GetComponent<LineRenderer>();
		lr.SetPosition(0, transform.position);
		lr.SetPosition(1, endPos);
	}

	public void setIsCurrent(bool b) {	isCur = b; }
}
