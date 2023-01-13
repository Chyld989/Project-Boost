using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
	[SerializeField] float MainThrust = 750f;
	[SerializeField] float RotationThrust = 100f;

	Rigidbody Rigidbody;
	RigidbodyConstraints RigidbodyConstraints;

	// Start is called before the first frame update
	void Start() {
		Rigidbody = GetComponent<Rigidbody>();
		RigidbodyConstraints = Rigidbody.constraints;
	}

	// Update is called once per frame
	void Update() {
		ProcessInput();
	}

	void ProcessInput() {
		ProcessThrust();
		ProcessRotation();
	}

	private void ProcessThrust() {
		if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
			Rigidbody.AddRelativeForce(Vector3.up * MainThrust * Time.deltaTime);
		}
	}

	private void ProcessRotation() {
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
			ApplyRotation(RotationThrust);
		} else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
			ApplyRotation(RotationThrust * -1);
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
