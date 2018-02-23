using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {
	[SerializeField] private GameObject _leftHand;
	[SerializeField] private GameObject _rightHand;
	[Range(1, 500)]
	[SerializeField] private int _speed = 100;
	[SerializeField] private Text _distanceText;

	private bool isFlying = false;
	private Parser _gm;
	private Vector3 curDirection;

	private bool visibleHands;
	private bool didNotCollide = true;

	// Use this for initialization
	void Start() {
		_gm = GameObject.FindGameObjectWithTag("GM").GetComponent<Parser>();
		isFlying = false;
		curDirection = Vector3.zero;
		visibleHands = false;
	}

	
	void FixedUpdate() {
		visibleHands = _leftHand.activeInHierarchy && _rightHand.activeInHierarchy;

		if (transform.position.y <= 0)
			GetComponent<Countdown>().hitObstacle();
	}

	public void fly(Vector3 direction) {
		if (isFlying && visibleHands && didNotCollide) {
			transform.Translate(direction * _speed * Time.deltaTime);
			GameObject cp = _gm.getCurrentcheckpoint(_gm.getIndex());
			if(cp != null)
				_distanceText.text = Vector3.Distance(transform.position, cp.transform.position).ToString("F1");
		}
	}

	public void setIsFlying(bool b) { isFlying = b; }
	public void setDidNotCollide(bool b) { didNotCollide = b; }
}