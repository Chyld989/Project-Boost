using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
	[Header("Rocket Control Speed")]
	[SerializeField] float MainThrust = 750f;
	[SerializeField] float RotationThrust = 100f;

	[Header("Sound Effects")]
	[SerializeField] AudioClip MainThruster = null;
	[SerializeField] AudioClip AuxiliaryThruster = null;

	[Header("Thruster Particle Effects")]
	[SerializeField] ParticleSystem MainThrusterParticles = null;
	[SerializeField] ParticleSystem TurnRightThruster1Particles = null;
	[SerializeField] ParticleSystem TurnRightThruster2Particles = null;
	[SerializeField] ParticleSystem TurnLeftThruster1Particles = null;
	[SerializeField] ParticleSystem TurnLeftThruster2Particles = null;

	[Header("Thruster Lights")]
	[SerializeField] Light MainThrusterLight = null;
	[SerializeField] Light TurnRightThruster1Light = null;
	[SerializeField] Light TurnRightThruster2Light = null;
	[SerializeField] Light TurnLeftThruster1Light = null;
	[SerializeField] Light TurnLeftThruster2Light = null;

	[SerializeField] GameObject DirectionalCone = null;
	[SerializeField] GameObject LandingPad = null;

	bool PlayerHasControl = true;
	bool IsActive = true;

	Rigidbody Rigidbody;
	RigidbodyConstraints RigidbodyConstraints;
	AudioSource AudioSourceMainThruster;
	AudioSource AudioSourceAuxiliaryThruster;
	MeshRenderer DirectionalConeMeshRenderer;
	MeshRenderer LandingPadMeshRenderer;

	// Start is called before the first frame update
	void Start() {
		Rigidbody = GetComponent<Rigidbody>();
		RigidbodyConstraints = Rigidbody.constraints;

		DirectionalConeMeshRenderer = DirectionalCone.GetComponent<MeshRenderer>();
		LandingPadMeshRenderer = LandingPad.GetComponent<MeshRenderer>();
		
		foreach (var audioSource in GetComponents<AudioSource>()) {
			if (AudioSourceMainThruster == null) {
				AudioSourceMainThruster = audioSource;
				AudioSourceMainThruster.clip = MainThruster;
			} else {
				AudioSourceAuxiliaryThruster = audioSource;
				AudioSourceAuxiliaryThruster.clip = AuxiliaryThruster;
			}
		}
	}

	// Update is called once per frame
	void Update() {
		UpdateDirectionalCone();
		if (PlayerHasControl) {
			ProcessInput();
		} else if (IsActive) {
			if (AudioSourceMainThruster.isPlaying) {
				AudioSourceMainThruster.volume = 0f;
			}
			if (AudioSourceAuxiliaryThruster.isPlaying) {
				AudioSourceAuxiliaryThruster.volume = 0f;
			}
			MainThrusterParticles.Stop();
			TurnRightThruster1Particles.Stop();
			TurnRightThruster2Particles.Stop();
			TurnLeftThruster1Particles.Stop();
			TurnLeftThruster2Particles.Stop();
		}
	}

	void ProcessInput() {
		ProcessThrust();
		ProcessRotation();
	}

	private void ProcessThrust() {
		if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
			StartThrusting();
		} else {
			StopThrusting();
		}
	}

	private void ProcessRotation() {
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
			RotateLeft();
		} else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
			RotateRight();
		} else {
			StopRotating();
		}
	}

	private void StartThrusting() {
		Rigidbody.AddRelativeForce(Vector3.up * MainThrust * Time.deltaTime);
		AudioSourceMainThruster.volume = 1f;
		if (AudioSourceMainThruster.isPlaying == false) {
			AudioSourceMainThruster.Play();
		}
		if (MainThrusterParticles.isPlaying == false) {
			MainThrusterParticles.Play();
		}
		MainThrusterLight.enabled = true;
	}

	private void StopThrusting() {
		// Used instead of .Stop() on audio to avoid clicking sound at the end
		AudioSourceMainThruster.volume = 0f;
		MainThrusterParticles.Stop();
		MainThrusterLight.enabled = false;
	}

	private void RotateLeft() {
		ApplyRotation(RotationThrust);
		AudioSourceAuxiliaryThruster.volume = 1f;
		if (AudioSourceAuxiliaryThruster.isPlaying == false) {
			AudioSourceAuxiliaryThruster.Play();
		}
		if (TurnLeftThruster1Particles.isPlaying == false) {
			TurnLeftThruster1Particles.Play();
		}
		TurnLeftThruster1Light.enabled = true;
		if (TurnLeftThruster2Particles.isPlaying == false) {
			TurnLeftThruster2Particles.Play();
		}
		TurnLeftThruster2Light.enabled = true;
	}

	private void RotateRight() {
		ApplyRotation(RotationThrust * -1);
		AudioSourceAuxiliaryThruster.volume = 1f;
		if (AudioSourceAuxiliaryThruster.isPlaying == false) {
			AudioSourceAuxiliaryThruster.Play();
		}
		if (TurnRightThruster1Particles.isPlaying == false) {
			TurnRightThruster1Particles.Play();
		}
		TurnRightThruster1Light.enabled = true;
		if (TurnRightThruster2Particles.isPlaying == false) {
			TurnRightThruster2Particles.Play();
		}
		TurnRightThruster2Light.enabled = true;
	}

	private void StopRotating() {
		// Used instead of .Stop() on audio to avoid clicking sound at the end
		AudioSourceAuxiliaryThruster.volume = 0f;
		TurnRightThruster1Particles.Stop();
		TurnRightThruster1Light.enabled = false;
		TurnRightThruster2Particles.Stop();
		TurnRightThruster2Light.enabled = false;
		TurnLeftThruster1Particles.Stop();
		TurnLeftThruster1Light.enabled = false;
		TurnLeftThruster2Particles.Stop();
		TurnLeftThruster2Light.enabled = false;
	}

	private void ApplyRotation(float rotationThrust) {
		// Freeze physics based rotation
		Rigidbody.freezeRotation = true;
		transform.Rotate(Vector3.forward * rotationThrust * Time.deltaTime);
		// Re-enable physics based rotation
		Rigidbody.constraints = RigidbodyConstraints;
	}

	private void UpdateDirectionalCone() {
		DirectionalConeMeshRenderer.enabled = !MeshIsVisibleToCamera(Camera.main, LandingPadMeshRenderer);
		DirectionalCone.transform.LookAt(LandingPad.transform);
	}

	public void RemovePlayerControl() {
		PlayerHasControl = false;
	}

	public bool IsPlayerActive() {
		return IsActive;
	}

	public void DeactivatePlayer() {
		IsActive = false;
		StopThrusting();
		StopRotating();
	}

	private bool MeshIsVisibleToCamera(Camera camera, Renderer renderer) {
		// Taken from https://community.gamedev.tv/t/meshrenderer-isvisible-issues/219395/5 by bixarrio
		// If the renderer's pivot is in view we should be okay
		var viewPos = camera.WorldToViewportPoint(renderer.transform.position);
		if (viewPos.x >= 0 && viewPos.y >= 0 && viewPos.x <= 1 && viewPos.y <= 1) {
			return true;
		}

		// If pivot is not in view the mesh may still have some bits that are visible
		var bounds = renderer.bounds;

		// Check each of the points and see if any is visible
		// Helper constants
		const int LEFT = -1;
		const int RIGHT = 1;
		const int TOP = 1;
		const int BOTTOM = -1;
		const int FRONT = -1;
		const int BACK = 1;

		// Left bottom front
		if (PointIsVisible(LEFT, BOTTOM, FRONT, camera, bounds)) { return true; }
		// Left top front
		if (PointIsVisible(LEFT, TOP, FRONT, camera, bounds)) { return true; }
		// Left bottom back
		if (PointIsVisible(LEFT, BOTTOM, BACK, camera, bounds)) { return true; }
		// Left top back
		if (PointIsVisible(LEFT, TOP, BACK, camera, bounds)) { return true; }
		// Right bottom front
		if (PointIsVisible(RIGHT, BOTTOM, FRONT, camera, bounds)) { return true; }
		// Right top front
		if (PointIsVisible(RIGHT, TOP, FRONT, camera, bounds)) { return true; }
		// Right bottom back
		if (PointIsVisible(RIGHT, BOTTOM, BACK, camera, bounds)) { return true; }
		// Right top back
		if (PointIsVisible(RIGHT, TOP, BACK, camera, bounds)) { return true; }

		// If none of those are visible, we're likely not visible
		return false;
	}

	// All the duplicated code in one place
	bool PointIsVisible(int xDir, int yDir, int zDir, Camera camera, Bounds bounds) {
		var point = GetPoint(xDir, yDir, zDir, bounds);
		var localViewPos = camera.WorldToViewportPoint(point);
		return (localViewPos.x >= 0 && localViewPos.y >= 0 && localViewPos.x <= 1 && localViewPos.y <= 1);
	}

	// Just a simple helper to get the points I want
	Vector3 GetPoint(int xDir, int yDir, int zDir, Bounds bounds) => new Vector3(
		bounds.center.x + (bounds.extents.x * xDir),
		bounds.center.y + (bounds.extents.y * yDir),
		bounds.center.z + (bounds.extents.z * zDir));
}
