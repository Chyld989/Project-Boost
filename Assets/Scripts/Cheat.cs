using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour {
	[SerializeField] bool IsCheatEnabled = true;

	CollisionHandler CollisionHandler;

	// Start is called before the first frame update
	void Start() {
		CollisionHandler = GetComponent<CollisionHandler>();
	}

	// Update is called once per frame
	void Update() {
		if (IsCheatEnabled == false) { return; }
		if (Input.GetKeyDown(KeyCode.C)) {
			CollisionHandler.ToggleCollision();
		} else if (Input.GetKeyDown(KeyCode.P)) {
			CollisionHandler.LoadPreviousScene();
		} else if (Input.GetKeyDown(KeyCode.R)) {
			CollisionHandler.ReloadScene();
		} else if (Input.GetKeyDown(KeyCode.N)) {
			CollisionHandler.LoadNextScene();
		}
	}
}
