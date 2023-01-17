using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour {
	[Range(0f, 5f)]
	[SerializeField] float SceneResetTimer = 3f;

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
					Debug.Log($@"You finished the level!");
					Movement.RemovePlayerControl();
					StartCoroutine(LoadNextSceneAfterDelay(SceneResetTimer));
				}
				break;
			default:
				if (collision.GetContact(0).thisCollider.transform.gameObject.name.Contains("Rocket Fin Stand") == false) {
					Debug.Log($@"{collision.gameObject.name} is an obstacle and you suck for hitting it.");
					Movement.RemovePlayerControl();
					StartCoroutine(ReloadSceneAfterDelay(SceneResetTimer));
				}
				break;
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Fuel")) {
			Debug.Log($@"{other.gameObject.name} is fuel to keep you going.");
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
}
