using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour {
	[Range(0f, 5f)]
	[SerializeField] float SceneResetTimer = 3f;
	[SerializeField] AudioClip CrashExplosion = null;
	[SerializeField] AudioClip SuccessFireworks = null;

	Movement Movement;

	private void Start() {
		Movement = GetComponent<Movement>();
	}

	private void OnCollisionEnter(Collision collision) {
		switch (collision.gameObject.tag) {
			case "Friendly":
				Debug.Log($@"{collision.gameObject.name} is friendly.");
				break;
			case "Finish":
				if (collision.GetContact(0).thisCollider.transform.gameObject.name.Contains("Rocket Fin Stand")) {
					StartLevelCompleteSequence();
				}
				break;
			default:
				if (collision.GetContact(0).thisCollider.transform.gameObject.name.Contains("Rocket Fin Stand") == false) {
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
		//Movement.enabled = false; // Rick's way of doing it
		// TODO: Add SFX on crash
		// TODO: Add VFX on crash
		StartCoroutine(ReloadSceneAfterDelay(SceneResetTimer));
	}

	private void StartLevelCompleteSequence() {
		Movement.RemovePlayerControl();
		//Movement.enabled = false; // Rick's way of doing it
		StartCoroutine(LoadNextSceneAfterDelay(SceneResetTimer));
	}
}
