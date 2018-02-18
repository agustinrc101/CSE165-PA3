using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackSelection : MonoBehaviour {
	private GameObject gm;
	private Text text;

	// Use this for initialization
	void Start () {
		text = GetComponentInChildren<Text>();
		gm = GameObject.FindGameObjectWithTag("GM");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setName(string filename) {
		text.text = filename;
	}

}
