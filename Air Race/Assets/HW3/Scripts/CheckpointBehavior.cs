using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehavior : MonoBehaviour {

	private GameObject gm;
	private GameObject player;
	private Collider col;


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
			if(Vector3.Distance(player.transform.position, transform.position) > 10.0f)
				transform.LookAt(player.transform);

		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag("PlayerCollider"))
			StartCoroutine(reachedWaypoint());
	}

	public void setCollider(bool b) {col.enabled = b; }

	private IEnumerator reachedWaypoint() {
		yield return new WaitForSeconds(0.5f);
		if (gm == null)
			gm = GameObject.FindGameObjectWithTag("GM");
		gm.GetComponent<Parser>().nextWaypoint();
	}
}
