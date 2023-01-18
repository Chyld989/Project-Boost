﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
	[Header("Rocket Control Speed")]
	[SerializeField] float MainThrust = 750f;
	[SerializeField] float RotationThrust = 100f;

	[Header("Sound Effects")]
	[SerializeField] AudioClip MainThruster = null;
	[SerializeField] AudioClip AuxiliaryThruster = null;

	bool PlayerHasControl = true;
	bool IsAlive = true;

	Rigidbody Rigidbody;
	RigidbodyConstraints RigidbodyConstraints;
	AudioSource AudioSourceMainThruster;
	AudioSource AudioSourceAuxiliaryThruster;

	// Start is called before the first frame update
	void Start() {
		Rigidbody = GetComponent<Rigidbody>();
		RigidbodyConstraints = Rigidbody.constraints;

		foreach (var audioSource in GetComponents<AudioSource>()) {
			if (AudioSourceMainThruster == null) {
				AudioSourceMainThruster = audioSource;
				AudioSourceMainThruster.clip = MainThruster;
			} else {
				AudioSourceAuxiliaryThruster = audioSource;
				AudioSourceAuxiliaryThruster.clip = AuxiliaryThruster;
			}
		}
	}

	// Update is called once per frame
	void Update() {
		if (PlayerHasControl) {
			ProcessInput();
		} else if (IsAlive) {
			if (AudioSourceMainThruster.isPlaying) {
				AudioSourceMainThruster.volume = 0f;
			}
			if (AudioSourceAuxiliaryThruster.isPlaying) {
				AudioSourceAuxiliaryThruster.volume = 0f;
			}
		}
	}

	public void RemovePlayerControl() {
		PlayerHasControl = false;
	}

	public bool IsPlayerAlive() {
		return IsAlive;
	}

	public void KillPlayer() {
		IsAlive = false;
	}

	void ProcessInput() {
		ProcessThrust();
		ProcessRotation();
	}

	private void ProcessThrust() {
		if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
			Rigidbody.AddRelativeForce(Vector3.up * MainThrust * Time.deltaTime);
			AudioSourceMainThruster.volume = 1f;
			if (AudioSourceMainThruster.isPlaying == false) {
				AudioSourceMainThruster.Play();
			}
		} else {
			// Used instead of .Stop() to avoid clicking sound at the end
			AudioSourceMainThruster.volume = 0f;
		}
	}

	private void ProcessRotation() {
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
			ApplyRotation(RotationThrust);
			AudioSourceAuxiliaryThruster.volume = 1f;
			if (AudioSourceAuxiliaryThruster.isPlaying == false) {
				AudioSourceAuxiliaryThruster.Play();
			}
		} else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
			ApplyRotation(RotationThrust * -1);
			AudioSourceAuxiliaryThruster.volume = 1f;
			if (AudioSourceAuxiliaryThruster.isPlaying == false) {
				AudioSourceAuxiliaryThruster.Play();
			}
		} else {
			// Used instead of .Stop() to avoid clicking sound at the end
			AudioSourceAuxiliaryThruster.volume = 0f;
		}
	}

	private void ApplyRotation(float rotationThrust) {
		// Freeze physics based rotation
		Rigidbody.freezeRotation = true;
		transform.Rotate(Vector3.forward * rotationThrust * Time.deltaTime);
		// Re-enable physics based rotation
		Rigidbody.constraints = RigidbodyConstraints;
	}
}
