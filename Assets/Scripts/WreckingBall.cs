using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingBall : MonoBehaviour {
	[SerializeField] float Speed = 2f;
	[SerializeField] float MaxRotationX = 45f;
	[SerializeField] float MaxRotationY = 0f;
	[SerializeField] float MaxRotationZ = 0f;

	// Update is called once per frame
	void Update() {
		float xRotation = MaxRotationX * Mathf.Sin(Time.time * Speed);
		float yRotation = MaxRotationY * Mathf.Sin(Time.time * Speed);
		float zRotation = MaxRotationZ * Mathf.Sin(Time.time * Speed);
		transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);
	}
}
