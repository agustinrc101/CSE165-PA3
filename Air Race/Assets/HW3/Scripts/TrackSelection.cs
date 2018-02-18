using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackSelection : MonoBehaviour {
	private GameObject gm;
	private int trackNum = 0;

	// Use this for initialization
	void Awake () {
		gm = GameObject.FindGameObjectWithTag("GM");
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

}
