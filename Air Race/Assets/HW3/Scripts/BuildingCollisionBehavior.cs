using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCollisionBehavior : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("PlayerCollider"))
			GameObject.FindGameObjectWithTag("Player").GetComponent<Countdown>().hitObstacle();
	}
}

