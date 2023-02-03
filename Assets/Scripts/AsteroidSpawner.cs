using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour {
	[SerializeField] float MinSpawnX = -30f;
	[SerializeField] float MaxSpawnX = 30f;
	[SerializeField] float MinSpawnY = 49f;
	[SerializeField] float MaxSpawnY = 51f;
	[SerializeField] float MinLaunchForceX = -200f;
	[SerializeField] float MaxLaunchForceX = -1000f;
	[SerializeField] float MinLaunchForceY = -200f;
	[SerializeField] float MaxLaunchForceY = -1000f;
	[SerializeField] [Tooltip("Number of seconds to wait before spawning a new asteroid (repeats)")] float SpawnTimerInSeconds = 4f;
	[SerializeField] [Tooltip("GameObject that should be spawned")] GameObject AsteroidToSpawn = null;

	float SpawnTimer = 0f;
	bool KeepSpawning = true;

	// Start is called before the first frame update
	void Start() {
		SpawnTimer = SpawnTimerInSeconds;
	}

	// Update is called once per frame
	void Update() {
		if (KeepSpawning) {
			SpawnTimer += Time.deltaTime;
			if (SpawnTimer >= SpawnTimerInSeconds) {
				var xSpawn = Random.Range(MinSpawnX, MaxSpawnX);
				var ySpawn = Random.Range(MinSpawnY, MaxSpawnY);
				var xRotation = Random.Range(0f, 359f);
				var yRotation = Random.Range(0f, 359f);
				var zRotation = Random.Range(0f, 359f);
				var newGameObject = Instantiate(AsteroidToSpawn, new Vector3(xSpawn, ySpawn, 0f), transform.rotation * Quaternion.Euler(xRotation, yRotation, zRotation));
				var launchForceX = Random.Range(MinLaunchForceX, MaxLaunchForceX);
				var launchForceY = Random.Range(MinLaunchForceY, MaxLaunchForceY);
				newGameObject.GetComponent<AsteroidCollider>().AddForce(launchForceX, launchForceY, 0);
				SpawnTimer = 0f;
			}
		}
	}

	public void StopSpawning() {
		KeepSpawning = false;
	}
}
