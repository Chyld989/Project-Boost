using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCollider : MonoBehaviour {
	[SerializeField] AudioClip CrashAudioClip = null;
	[SerializeField] ParticleSystem CrashParticles = null;
	[SerializeField] float ScaleDuration = 2.5f;

	bool Crashed = false;

	List<GameObject> AsteroidParts = new List<GameObject>();

	private void Start() {
		for (int c = 0; c < transform.childCount; c++) {
			if (transform.GetChild(c).gameObject.name.Contains("Asteroid Part")) {
				AsteroidParts.Add(transform.GetChild(c).gameObject);
			}
		}
	}

	private void OnCollisionEnter(Collision collision) {
		if (Crashed == false) {
			Explode();
		}
	}

	private void Explode() {
		Crashed = true;
		CrashParticles.Play();
		SpreadParts();
	}

	private void SpreadParts() {
		foreach (var part in AsteroidParts) {
			part.AddComponent<Rigidbody>();
			var newX = Random.Range(300, 2000);
			if (FlipCoin()) { newX *= -1; }
			var newY = Random.Range(300, 2000);
			if (FlipCoin()) { newY *= -1; }
			var newZ = Random.Range(300, 2000);
			if (FlipCoin()) { newZ *= -1; }
			part.GetComponent<Rigidbody>().AddForce(new Vector3(newX, newY, newZ));
			part.tag = "Annoyance";
			ScaleToTarget(part, new Vector3(0f, 0f, 0f), ScaleDuration);
		}
		Destroy(gameObject, ScaleDuration + 1f);
	}

	private void ScaleToTarget(GameObject gameObject, Vector3 targetScale, float duration) {
		// Taken from http://answers.unity.com/answers/1441623/view.html
		StartCoroutine(ScaleToTargetCoroutine(gameObject, targetScale, duration));
	}

	IEnumerator ScaleToTargetCoroutine(GameObject gameObject, Vector3 targetScale, float duration) {
		Vector3 startScale = gameObject.transform.localScale;
		float timer = 0f;

		while (timer < duration) {
			timer += Time.deltaTime;
			float t = timer / duration;
			// Smoother step algoritm
			t = t * t * t * (t * (6f * t - 15f) + 10f);
			gameObject.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
			yield return null;
		}

		yield return null;
	}

	private bool FlipCoin() {
		return Random.Range(0, 2) == 1;
	}
}
