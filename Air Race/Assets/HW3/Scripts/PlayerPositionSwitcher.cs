using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionSwitcher : MonoBehaviour {
	[SerializeField] private Transform[] _playerPositions;
	[SerializeField] private Transform _cameraTransform;
	[SerializeField] private GameObject _ship;
	[SerializeField] private Collider _playerCollider;
	[SerializeField] private float _delay = 2.0f;
	private int curIndex;
	private float time;

	// Use this for initialization
	void Start () {
		time = 0.0f;
		curIndex = 0;
		if (_playerPositions.Length < 1)
			Debug.LogError(name + " does not have any spawn positions");
	}

	public void switchPosition() {
		time += Time.deltaTime;
		if (time > _delay) {
			time = 0.0f;
			curIndex++;
			if (curIndex >= _playerPositions.Length)
				curIndex = 0;

			_ship.SetActive(curIndex != 0);
			_cameraTransform.position = _playerPositions[curIndex].position;

			_playerCollider.enabled = (curIndex == 0);

			if (curIndex == 1) {
				//_ship.transform.parent = this.transform;
				
			}
			else if (curIndex == 2) {
				//_ship.transform.parent = _cameraTransform;
				
			}
		}
	}

	void FixedUpdate() {
		if (curIndex == 1 && _ship.activeInHierarchy) {
			Parser p = GameObject.FindGameObjectWithTag("GM").GetComponent<Parser>();
			GameObject cp = p.getCurrentcheckpoint(p.getIndex());

			if (cp != null) {
				Vector3 target = cp.transform.position;
				_ship.transform.LookAt(new Vector3(target.x, _ship.transform.position.y, target.z));
			}

		}
	}
}
