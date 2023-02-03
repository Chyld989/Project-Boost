using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {
	[SerializeField] [Min(0)] float LaserLightIntensity = 10f;
	[SerializeField] [Min(0)] float LaserLightMaxRange = 50f;
	[SerializeField] [Tooltip("Delay before initial firing sequence starts")] [Min(0)] float InitialDelayInSeconds = 0f;
	[SerializeField] [Tooltip("Overrides Initial Delay In Seconds value")] bool RandomInitialDelay = false;
	[SerializeField] [Tooltip("Delay between firings")] [Min(0)] float LaserDelayInSeconds = 1f;
	[SerializeField] [Min(0.1f)] float LaserBeamDuration = 0.5f;
	[SerializeField] [Range(0, 1)] float LaserSoundVolume = 0.1f;

	float LaserTimer = 0f;
	float LightBuildTime = 3.5f;
	float FireLaserAtTime = 4.5f;
	bool SoundStarted = false;
	bool LightBuilding = true;

	AudioSource AudioSource;
	GameObject LaserLens;
	Light LaserLight;
	GameObject LaserBeam;

	// Start is called before the first frame update
	void Start() {
		AudioSource = GetComponent<AudioSource>();
		for (int c = 0; c < transform.childCount; c++) {
			if (transform.GetChild(c).gameObject.name.Contains("Laser Lens")) {
				LaserLens = transform.GetChild(c).gameObject;
			}
			if (transform.GetChild(c).gameObject.name.Contains("Laser Beam")) {
				LaserBeam = transform.GetChild(c).gameObject;
			}
		}
		LaserLight = LaserLens.GetComponent<Light>();
		LaserLight.intensity = LaserLightIntensity;
		LaserBeam.SetActive(false);
		if (RandomInitialDelay) {
			InitialDelayInSeconds = Random.Range(0, 4.5f);
		}
		LaserTimer = (InitialDelayInSeconds * -1) + LaserDelayInSeconds;
	}

	// Update is called once per frame
	void Update() {
		LaserTimer += Time.deltaTime;
		AudioSource.volume = LaserSoundVolume;
		if (LaserTimer > LaserBeamDuration && LaserBeam.activeInHierarchy) {
			LaserBeam.SetActive(false);
		}
		if (LaserTimer > LaserDelayInSeconds) {
			if (SoundStarted == false) {
				AudioSource.Stop();
				AudioSource.Play();
				SoundStarted = true;
				LightBuilding = true;
			}
			if (LightBuilding) {
				LaserLight.range += (LaserLightMaxRange / LightBuildTime) * Time.deltaTime;
			}
		}
		if (LaserTimer > LightBuildTime + LaserDelayInSeconds) {
			LightBuilding = false;
		}
		if (LaserTimer > FireLaserAtTime + LaserDelayInSeconds) {
			LaserLight.range = 0;
			LaserBeam.SetActive(true);
			LaserTimer = 0;
			SoundStarted = false;
		}
	}
}
