using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour {
	[Range(0.0f, 0.1f)]
	[SerializeField] private float _minAlpha = 0.0f;
	[Range(0.1f, 1.0f)]
	[SerializeField] private float _maxAlpha = 0.3f;
	[Range(0.001f, 0.01f)]
	[SerializeField] private float _alphaStep = 0.05f;

	private Material mat;
	private float curAlpha;
	private bool increment = true;

	// Use this for initialization
	void Start () {
		mat = GetComponent<MeshRenderer>().materials[0];
		curAlpha = _minAlpha;
	}

	// Update is called once per frame
	void Update() {
		if (curAlpha < _minAlpha)
			increment = true;
		else if (curAlpha > _maxAlpha)
			increment = false;

		if (increment)
			curAlpha += _alphaStep;
		else 
			curAlpha -= _alphaStep;

		mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, curAlpha);
	}
}
