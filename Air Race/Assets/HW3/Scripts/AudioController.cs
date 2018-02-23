using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {
	[SerializeField] private AudioClip _trackSelected;
	[SerializeField] private AudioClip _countdownBeep1;
	[SerializeField] private AudioClip _countdownBeep2;
	[SerializeField] private AudioClip _checkpointReached;
	[SerializeField] private AudioClip _finishedRace;
	[SerializeField] private AudioClip _hitABuilding;

	private AudioSource audioSource;

	void Start() {
		audioSource = GetComponent<AudioSource>();
	}

	public void playTrackSelected() {
		playAudio(_trackSelected);
	}

	public void playCountdownBeep1() {
		playAudio(_countdownBeep1);
	}

	public void playCountdownBeep2() {
		playAudio(_countdownBeep2);
	}

	public void playCheckPointReached() {
		playAudio(_checkpointReached);
	}

	public void playFinishedRace() {
		audioSource.volume = 0.1f;
		playAudio(_finishedRace);
	}

	public void playHitABuilding() {
		playAudio(_hitABuilding);
	}

	private void playAudio(AudioClip clip) {
		audioSource.clip = clip;
		audioSource.Play();
	}

}
