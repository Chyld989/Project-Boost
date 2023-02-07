using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {
	[SerializeField] [Tooltip("Bubble size once fully scaled up")] [Min(0)] float BubbleSize = 20f;
	[SerializeField] GameObject TeleportPartner = null;
	[SerializeField] GameObject Rocket = null;
	[SerializeField] GameObject RocketLaunchPad = null;

	GameObject TeleportBubble;
	AudioSource AudioSource;
	Teleport TeleportPartnerScript;
	Movement RocketMovement;
	bool ActivateTheBubble = false;
	bool BubbleActivated = false;
	bool SendingPad = false;
	float StartTime = 0f;
	float StartingScaleX;
	float StartingScaleY;
	float StartingScaleZ;
	float TeleportX;
	float TeleportY;
	float TeleportZ;
	float RocketOffsetY;

	// Start is called before the first frame update
	void Start() {
		for (int c = 0; c < transform.childCount; c++) {
			if (transform.GetChild(c).gameObject.name.Contains("Teleport Bubble")) {
				TeleportBubble = transform.GetChild(c).gameObject;
			}
		}
		StartingScaleX = transform.localScale.x;
		StartingScaleY = transform.localScale.y;
		StartingScaleZ = transform.localScale.z;
		RocketOffsetY = Rocket.transform.position.y - RocketLaunchPad.transform.position.y;
		TeleportX = TeleportPartner.transform.position.x;
		TeleportY = RocketOffsetY + TeleportPartner.transform.position.y;
		TeleportZ = TeleportPartner.transform.position.z;
		TeleportPartnerScript = TeleportPartner.GetComponent<Teleport>();
		AudioSource = GetComponent<AudioSource>();
		RocketMovement = Rocket.GetComponent<Movement>();
	}

	// Update is called once per frame
	void Update() {
		if (ActivateTheBubble) {
			ActivateTheBubble = false;
			BubbleActivated = true;
			TeleportPartnerScript.SetSendingPad(false);
			TeleportPartnerScript.StartTeleportation();
			AudioSource.Play();
			StartTime = Time.time;
		}
		if (BubbleActivated) {
			float curBubbleScale = Mathf.Sin(Time.time - StartTime) * BubbleSize; // 0 to 1, 1 to -1, -1 to 0, repeat
			if (curBubbleScale < 0) {
				BubbleActivated = false;
			}
			TeleportBubble.transform.localScale = new Vector3(curBubbleScale * StartingScaleX, curBubbleScale * StartingScaleY, curBubbleScale * StartingScaleZ);
			if (curBubbleScale >= BubbleSize - 1) {
				if (SendingPad == false) {
					SendingPad = true;
					Rocket.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
					Rocket.transform.position = new Vector3(TeleportX, TeleportY, TeleportZ);
				}
			}
		}
	}

	public void StartTeleportation() {
		if (BubbleActivated == false && RocketMovement.IsPlayerActive()) {
			ActivateTheBubble = true;
			SetSendingPad(true);
		}
	}

	public void SetSendingPad(bool value) {
		SendingPad = value;
	}
}
