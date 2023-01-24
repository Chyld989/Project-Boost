using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour {
	[SerializeField] Vector3 MovementVector;
	[SerializeField] float Period = 5f;
	[SerializeField] [Range(0, 1)] float StartingMovementFactor = 0f;

	float MovementFactor;
	Vector3 StartingPosition;

	// Start is called before the first frame update
	void Start() {
		StartingPosition = transform.position;
	}

	// Update is called once per frame
	void Update() {
		if (Period <= Mathf.Epsilon) { return; }
		float cycles = (Time.time / Period) + StartingMovementFactor; // Continually growing over time

		const float tau = Mathf.PI * 2; // Constant value of 6.282...
		float rawSinWave = Mathf.Sin(cycles * tau); // Going from -1 to 1

		MovementFactor = (rawSinWave + 1f) / 2; // Recalculated to go from 0 to 1

		Vector3 movementOffset = MovementVector * MovementFactor;
		transform.position = StartingPosition + movementOffset;
	}
}
