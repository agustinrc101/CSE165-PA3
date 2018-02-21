using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandRaycaster : MonoBehaviour {
	[SerializeField] private LayerMask _layerMask;

	private GameObject lastHit;
	private LineRenderer lineRenderer;

	// Use this for initialization
	void Start() {
		lastHit = null;
	}

	//Return true if the selection is complete
	public bool shootRaycast(Vector3 position, Vector3 direction, bool shootRay, Color color) {
		bool isDone = false;

		lineRenderer = GetComponent<LineRenderer>();

		//If the ray should be not be shot
		if (!shootRay) {
			lineRenderer.material.color = color;
			lineRenderer.SetPosition(0, position);
			lineRenderer.SetPosition(1, position);
			return isDone;
		}

		//Shoot ray
		Ray ray = new Ray(position, direction);
		RaycastHit hitInfo;

		Vector3 rayEndPosition = position;
		bool isUI = false;

		if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity)) {
			rayEndPosition = hitInfo.point;

			if (hitInfo.collider.gameObject.CompareTag("TrackSelectionBox"))
				isUI = true;

			if (isUI) {
				if (lastHit == null)
					lastHit = hitInfo.collider.gameObject;
				else if (hitInfo.collider.gameObject != lastHit) {
					lastHit.GetComponent<TrackSelection>().resetFill();
					lastHit = hitInfo.collider.gameObject;
				}

				isDone = hitInfo.collider.gameObject.GetComponent<TrackSelection>().increaseFill(color);
			}
		}
		else {
			if(lastHit != null)
				lastHit.GetComponent<TrackSelection>().resetFill();

			rayEndPosition = position + (100 * direction);
		}

		lineRenderer.material.color = color;
		lineRenderer.SetPosition(0, position);
		lineRenderer.SetPosition(1, rayEndPosition);

		return isDone;
	}
}
