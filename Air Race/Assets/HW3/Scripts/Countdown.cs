using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour {
	[SerializeField] private GameObject _three;
	[SerializeField] private GameObject _two;
	[SerializeField] private GameObject _one;
	[SerializeField] private GameObject _go;
	[SerializeField] private Transform _counterPosition;
	[SerializeField] private Text _timerText;

	private bool isLeftThumbsUp = false;
	private bool isRightThumbsUp = false;
	private bool isRacing = false;

	private Vector3 counterPos;
	private float curTime = 0.0f;

	void Start() {
		counterPos = _counterPosition.position;
	}

	void FixedUpdate() {
		if (isRacing) {
			curTime += Time.deltaTime;
			_timerText.text = curTime.ToString("F2");
		}
	}

	public void setLeftThumbsUp(bool b) { isLeftThumbsUp = b; }
	public void setRightThumbsUp(bool b) { isRightThumbsUp = b;	}
	public bool getIsReady() { return isLeftThumbsUp && isRightThumbsUp; }

	public void startCountdown() {
		StartCoroutine(countdown(true));
	}

	private IEnumerator countdown(bool go) {
		GameObject curCount;

		//3!
		curCount = Instantiate(_three,  _counterPosition);
		if (go)
			GetComponent<AudioController>().playCountdownBeep1();
		yield return new WaitForSeconds(1.0f);
		DestroyObject(curCount);

		//2!
		curCount = Instantiate(_two,  _counterPosition);
		if (go)
			GetComponent<AudioController>().playCountdownBeep1();
		yield return new WaitForSeconds(1.0f);
		DestroyObject(curCount);

		//1!
		curCount = Instantiate(_one, _counterPosition);
		if (go)
			GetComponent<AudioController>().playCountdownBeep2();
		yield return new WaitForSeconds(1.0f);
		DestroyObject(curCount);

		//GO!
		GetComponent<PlayerMovement>().enabled = true;
		GetComponent<PlayerMovement>().setDidNotCollide(true);

		if (go) {
			isRacing = true;
			curCount = Instantiate(_go, _counterPosition);
			yield return new WaitForSeconds(1.0f);
			DestroyObject(curCount);
		}
	}

	public void hitObstacle() {
		GetComponent<PlayerMovement>().setDidNotCollide(false);
		Parser gm = GameObject.FindGameObjectWithTag("GM").GetComponent<Parser>();
		Vector3 prevCheckpoint = gm.getCurrentcheckpoint(gm.getIndex() - 1).transform.position;
		transform.position = prevCheckpoint;

		GetComponent<AudioController>().playHitABuilding();

		StartCoroutine(countdown(false));
	}

	public void setIsRacing(bool b) {isRacing = b; }
}
