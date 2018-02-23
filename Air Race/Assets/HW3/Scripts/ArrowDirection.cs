using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDirection : MonoBehaviour {

	private Vector3 lookAtPos;

	void Start () {
		lookAtPos = transform.position + Vector3.forward;
	}
	
	void Update () {
		transform.LookAt(lookAtPos, Vector3.right);
	}

	public void setArrowLookAtPosition(Vector3 pos) {
		lookAtPos = pos;
	}
}
