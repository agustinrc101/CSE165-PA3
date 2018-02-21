using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackSelection : MonoBehaviour {
	[Range(0.01f, 0.5f)]
	[SerializeField] private float _fillSpeed = 0.1f;
	[SerializeField] private Image _fillImage;

	private GameObject gm;
	private int trackNum = 0;

	// Use this for initialization
	void Awake () {
		gm = GameObject.FindGameObjectWithTag("GM");

		if (_fillImage == null) {
			_fillImage = transform.GetChild(0).GetComponent<Image>();

			if (_fillImage == null)
				Debug.LogError(name + " does not have a child with an image component");
		}

	}

	public void setName(string filename) {
		GetComponentInChildren<Text>().text = filename;
	}

	public void setNumber(int num) {
		trackNum = num;
	}

	public void setTrack() {
		if (trackNum == 0)
			return;

		gm.GetComponent<Parser>().beginParsing(trackNum);
		transform.parent.gameObject.SetActive(false);
	}

	public bool increaseFill(Color color) {
		_fillImage.color = color;
		_fillImage.fillAmount += (_fillSpeed * Time.deltaTime);

		if (_fillImage.fillAmount >= 1.0f) {
			beginGame();
			return true;
		}

		return false;
	}

	public void resetFill(){
		_fillImage.fillAmount = 0.0f;
	}

	private void beginGame() {
		if (gm == null)
			gm = GameObject.FindGameObjectWithTag("GM");

		gm.GetComponent<Parser>().beginParsing(trackNum);
	}

}
