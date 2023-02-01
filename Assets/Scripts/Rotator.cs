using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {
	[SerializeField] float RotationSpeedX = 0f;
	[SerializeField] float RotationSpeedY = 0f;
	[SerializeField] float RotationSpeedZ = 0f;

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {
		var xRotation = RotationSpeedX * Time.deltaTime;
		var yRotation = RotationSpeedY * Time.deltaTime;
		var zRotation = RotationSpeedZ * Time.deltaTime;
		transform.Rotate(xRotation, yRotation, zRotation);
	}
}
