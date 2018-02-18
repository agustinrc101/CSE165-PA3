using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Parser : MonoBehaviour {
	[SerializeField] private GameObject _checkpoint;

	private float inchToMeters = 0.0254f;
	public int totalFiles = 0;

	private List<GameObject> checkpoints;
	private int index = 0;

	void OnDrawGizmos() {
		DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/HW3/Tracks/");
		FileInfo[] info = dir.GetFiles();
		totalFiles = info.Length / 2;
	}


	// Use this for initialization
	void Start () {
		//
	}

	public void beginParsing(int fileIndex) {
		if (File.Exists(Application.dataPath + "/HW3/Tracks/track" + fileIndex)) {
			Debug.Log("File Exists!");
			//BinaryFormatter bf = new BinaryFormatter();
			//FileStream file = File.Open("../Tracks/" + _track, FileMode.Open);
		}
		else {
			Debug.Log(Application.dataPath + "/HW3/Tracks/track" + fileIndex);
			Debug.LogError("Could not find checkpoint track file of name: " + fileIndex);
		}
	}

	public void begin() {
		index = 0;
		checkpoints[index].SetActive(true);

		if (checkpoints.Count > 1)
			checkpoints[index + 1].SetActive(true);
	}

	public void nextWaypoint() {
		checkpoints[index].SetActive(false);
		index++;

		if (checkpoints.Count > (index + 1))
			checkpoints[index + 1].SetActive(true);

		if (checkpoints.Count <= index)
			finishRace();

	}

	private void finishRace() {
		//TODO
		Debug.Log("Finished Race!");
		//TODO
	}

}
