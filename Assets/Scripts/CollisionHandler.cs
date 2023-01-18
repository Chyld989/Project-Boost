using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour {
	[Range(0f, 5f)]
	[SerializeField] float SceneResetTimer = 3f;
	[SerializeField] AudioClip CrashAudioClip = null;
	[SerializeField] AudioClip SuccessAudioClip = null;
	[SerializeField] ParticleSystem CrashParticles = null;
	[SerializeField] ParticleSystem SuccessParticles = null;

	float SceneCompleteTimer = 5f;

	Movement Movement;
	AudioSource MainAudioSource;
	AudioSource SecondaryAudioSource;

	private void Start() {
		Movement = GetComponent<Movement>();
		foreach (var audioSource in GetComponents<AudioSource>()) {
			if (MainAudioSource == null) {
				MainAudioSource = audioSource;
			} else {
				SecondaryAudioSource = audioSource;
			}
		}
	}

	private void OnCollisionEnter(Collision collision) {
		switch (collision.gameObject.tag) {
			case "Friendly":
				if (collision.GetContact(0).thisCollider.transform.gameObject.name.Contains("Rocket Fin Stand") == false) {
					// If anything other than the fins touches it's a crash
					StartCrashSequence();
				}
				break;
			case "Finish":
				if (collision.GetContact(0).thisCollider.transform.gameObject.name.Contains("Rocket Fin Stand")) {
					StartLevelCompleteSequence();
				} else {
					// If anything other than the fins touches it's a crash
					StartCrashSequence();
				}
				break;
			default:
				if (collision.GetContact(0).thisCollider.transform.gameObject.name.Contains("Rocket Fin Stand") == false) {
					// If anything other than the fins touches it's a crash
					StartCrashSequence();
				}
				break;
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Fuel")) {
			Destroy(other.gameObject);
		}
	}

	IEnumerator LoadNextSceneAfterDelay(float delay) {
		yield return new WaitForSeconds(delay);

		var nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
		if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings) {
			nextSceneIndex = 0;
		}
		SceneManager.LoadScene(nextSceneIndex);
	}

	IEnumerator ReloadSceneAfterDelay(float delay) {
		yield return new WaitForSeconds(delay);

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	void StartCrashSequence() {
		Movement.RemovePlayerControl();
		if (Movement.IsPlayerActive()) {
			Movement.DeactivatePlayer();
			PlaySoundEffect(CrashAudioClip);
			CrashParticles.Play();
			StartCoroutine(ReloadSceneAfterDelay(SceneResetTimer));
		}
	}

	private void StartLevelCompleteSequence() {
		Movement.RemovePlayerControl();
		if (Movement.IsPlayerActive()) {
			Movement.DeactivatePlayer();
			PlaySoundEffect(SuccessAudioClip);
			SuccessParticles.Play();
			StartCoroutine(LoadNextSceneAfterDelay(SceneCompleteTimer));
		}
	}

	private void PlaySoundEffect(AudioClip audioClip) {
		// Mute secondary audio source in case it's being used
		SecondaryAudioSource.volume = 0f;
		MainAudioSource.clip = audioClip;
		MainAudioSource.loop = false;
		MainAudioSource.volume = 1f;
		MainAudioSource.Play();
	}
}
