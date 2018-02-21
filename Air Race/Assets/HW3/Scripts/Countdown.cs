using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown : MonoBehaviour {
	[SerializeField] private GameObject _three;
	[SerializeField] private GameObject _two;
	[SerializeField] private GameObject _one;
	[SerializeField] private GameObject _go;
	[SerializeField] private Transform _counterPosition;

	private bool isLeftThumbsUp = false;
	private bool isRightThumbsUp = false;

	private Vector3 counterPos;

	void Start() {
		counterPos = _counterPosition.position;
	}

	public void setLeftThumbsUp(bool b) { isLeftThumbsUp = b; }
	public void setRightThumbsUp(bool b) { isRightThumbsUp = b;	}
	public bool getIsReady() { return isLeftThumbsUp && isRightThumbsUp; }

	public void startCountdown() {
		StartCoroutine(countdown());
	}

	private IEnumerator countdown() {
		GameObject curCount;

		//3!
		curCount = Instantiate(_three,  _counterPosition);
		yield return new WaitForSeconds(1.0f);
		DestroyObject(curCount);

		//2!
		curCount = Instantiate(_two,  _counterPosition);
		yield return new WaitForSeconds(1.0f);
		DestroyObject(curCount);

		//1!
		curCount = Instantiate(_one, _counterPosition);
		yield return new WaitForSeconds(1.0f);
		DestroyObject(curCount);

		//GO!
		curCount = Instantiate(_go,  _counterPosition);
		GetComponent<PlayerMovement>().enabled = true;
		yield return new WaitForSeconds(1.0f);
		DestroyObject(curCount);

	}


}
