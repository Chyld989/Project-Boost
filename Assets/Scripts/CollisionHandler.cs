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
	bool IsCollisionDisabled = false;

	Movement Movement;
	AudioSource MainAudioSource;
	AudioSource SecondaryAudioSource;

	private void Start() {
		SetGravity();
		Movement = GetComponent<Movement>();
		foreach (var audioSource in GetComponents<AudioSource>()) {
			if (MainAudioSource == null) {
				MainAudioSource = audioSource;
			} else {
				SecondaryAudioSource = audioSource;
			}
		}
	}

	private static void SetGravity() {
		if (SceneManager.GetActiveScene().name == "100 Fight against wind") {
			Physics.gravity = new Vector3(1f, -9.81f, 0);
		} else if (SceneManager.GetActiveScene().name == "110 Fight against a strong wind") {
			Physics.gravity = new Vector3(2f, -9.81f, 0);
		} else if (SceneManager.GetActiveScene().name == "120 Fight against a downward wind") {
			Physics.gravity = new Vector3(0f, -13f, 0);
		} else {
			Physics.gravity = new Vector3(0, -9.81f, 0);
		}
	}

	public void LoadNextScene() {
		StartCoroutine(LoadNextSceneAfterDelay(0));
	}

	public void LoadPreviousScene() {
		StartCoroutine(LoadPreviousSceneAfterDelay(0));
	}

	public void ReloadScene() {
		StartCoroutine(ReloadSceneAfterDelay(0));
	}

	public void ToggleCollision() {
		IsCollisionDisabled = !IsCollisionDisabled;
	}

	private void OnCollisionEnter(Collision collision) {
		if (IsCollisionDisabled) { return; }
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
			case "Annoyance":
				// Sole purpose is to screw up flight patterns when hit, not fail the level
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

	IEnumerator LoadPreviousSceneAfterDelay(float delay) {
		yield return new WaitForSeconds(delay);

		var previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
		if (previousSceneIndex < 0) {
			previousSceneIndex = SceneManager.sceneCountInBuildSettings - 1;
		}
		SceneManager.LoadScene(previousSceneIndex);
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
