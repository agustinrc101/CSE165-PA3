using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Parser : MonoBehaviour {
	[SerializeField] private GameObject _checkpoint;
	[SerializeField] private Transform _trackSelectionCanvas;
	[SerializeField] private GameObject _trackSelectionBox;

	private float inchToMeters = 0.0254f;
	public int totalFiles = 0;

	private List<GameObject> checkpoints;
	private int currentCheckpoint = 0;

	void OnDrawGizmos() {
		DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/HW3/Tracks/");
		FileInfo[] info = dir.GetFiles();
		totalFiles = info.Length / 2;
	}

	// Use this for initialization
	void Start () {
		checkpoints = new List<GameObject>();

		DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/HW3/Tracks/");
		FileInfo[] info = dir.GetFiles();

		float curYLocation = 6.0f;

		for (int i = 0; i < info.Length; i++) {
			string temp = info[i].Name;
			int size = temp.Length;

			if (temp[size - 1] != 'a') {
				GameObject curBox = Instantiate(_trackSelectionBox, new Vector3(0, 0, 0), Quaternion.identity, _trackSelectionCanvas);
				curBox.GetComponent<RectTransform>().position = new Vector3(0, curYLocation, 10);
				curBox.GetComponent<TrackSelection>().setName("Track " + temp[size - 1]);
				curBox.GetComponent<TrackSelection>().setNumber(int.Parse("" + temp[size - 1]));

				curYLocation -= 1.2f;
			}
				
		}
	}

	//When a textbox is selected
	public void beginParsing(int fileIndex) {
		if (File.Exists(Application.dataPath + "/HW3/Tracks/track" + fileIndex)) {
			StreamReader sr = new StreamReader(Application.dataPath + "/HW3/Tracks/track" + fileIndex);

			while (!sr.EndOfStream) {
				string s = sr.ReadLine();
				if (s == null)
					break;

				Vector3 cPosition = Vector3.zero;

				//Parses X position
				int ind = 0;
				char curChar = s[ind];
				string s1 = "";
				
				while (curChar != ' ') {
					s1 += curChar;
					ind++;
					curChar = s[ind];
				}

				cPosition.x = float.Parse(s1) * inchToMeters;

				//Parses Y position
				s1 = "";
				ind++;
				curChar = s[ind];

				while (curChar != ' ') {
					s1 += curChar;
					ind++;
					curChar = s[ind];
				}

				cPosition.y = float.Parse(s1) * inchToMeters;

				//Parses Z position
				s1 = "";
				ind++;
				curChar = s[ind];

				while (curChar != '\n' && ind < s.Length) {
					s1 += curChar;
					ind++;
					if(ind < s.Length)
						curChar = s[ind];
				}

				cPosition.z = float.Parse(s1) * inchToMeters;

				GameObject curCheckpoint = Instantiate(_checkpoint, cPosition, Quaternion.identity, this.transform);
				checkpoints.Add(curCheckpoint);
				curCheckpoint.SetActive(false);
			}

			begin();

			if (checkpoints.Count > 0) {
				GameObject.FindGameObjectWithTag("Player").transform.position = checkpoints[0].transform.position;
				if (checkpoints.Count > 1)
					GameObject.FindGameObjectWithTag("Player").transform.LookAt(checkpoints[1].transform, Vector3.up);
			}


		}
		else {
			Debug.LogError("Could not find checkpoint track file of name: " + Application.dataPath + "/HW3/Tracks/track" + fileIndex);
		}
	}

	public void begin() {
		if (checkpoints.Count < 2) {
			finishRace();
			return;
		}

		currentCheckpoint = 1;
		checkpoints[currentCheckpoint].SetActive(true);
		checkpoints[currentCheckpoint].GetComponent<CheckpointBehavior>().setCollider(true);

		if (checkpoints.Count > 1) {
			checkpoints[currentCheckpoint + 1].SetActive(true);
			checkpoints[currentCheckpoint + 1].GetComponent<CheckpointBehavior>().setCollider(false);
		}
	}

	public void nextWaypoint() {
		checkpoints[currentCheckpoint].SetActive(false);
		currentCheckpoint++;

		checkpoints[currentCheckpoint].GetComponent<CheckpointBehavior>().setCollider(true);

		if (checkpoints.Count > (currentCheckpoint + 1)) {
			checkpoints[currentCheckpoint + 1].SetActive(true);
			checkpoints[currentCheckpoint + 1].GetComponent<CheckpointBehavior>().setCollider(false);
		}

		if (checkpoints.Count <= currentCheckpoint)
			finishRace();

	}

	private void finishRace() {
		//TODO
		Debug.Log("Finished Race!");
		//TODO
	}

	public int getIndex() {
		return currentCheckpoint;
	}

	public GameObject getCurrentcheckpoint(int index) {
		if (index < checkpoints.Count)
			return checkpoints[index];
		else
			return null;
	}

}
