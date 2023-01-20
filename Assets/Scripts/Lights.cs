using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour {
	[SerializeField] List<Light> PadLights = null;
	[SerializeField] float LightChangeSpeedInSeconds = 0.2f;

	int lightLit = 0;
	float currentTime = 0f;

	// Start is called before the first frame update
	void Start() {
		foreach (var light in PadLights) {
			light.enabled = false;
		}
		for (int c = 0; c < 4; c++) {
			PadLights[lightLit + (4 * c)].enabled = true;
		}
	}

	// Update is called once per frame
	void Update() {
		currentTime += Time.deltaTime;

		if (currentTime >= LightChangeSpeedInSeconds) {
			currentTime = 0f;
			lightLit++;
			if (lightLit >= 4) { lightLit = 0; }

			foreach (var light in PadLights) {
				light.enabled = false;
			}
			for (int c = 0; c < 4; c++) {
				PadLights[lightLit + (4 * c)].enabled = true;
			}
		}
	}
}
