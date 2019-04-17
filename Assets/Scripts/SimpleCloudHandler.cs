using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class SimpleCloudHandler : MonoBehaviour, ICloudRecoEventHandler {

	public ImageTargetBehaviour ImageTargetTemplate;

	private CloudRecoBehaviour mCloudRecoBehaviour;
	private bool mIsScanning = false;
	public string mTargetMetadata = "";

	// For flash toggle
	public GUISkin guiSkin;
	private bool flashToggle = false;

	// Use this for initialization
	void Start () {
		CameraDevice.Instance.SetFocusMode (CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);

		mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();

		if (mCloudRecoBehaviour) {
			mCloudRecoBehaviour.RegisterEventHandler(this);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnInitialized() {
		Debug.Log ("Cloud Reco initialized");
	}

	public void OnInitError(TargetFinder.InitState initError) {
		Debug.Log ("Cloud Reco init error " + initError.ToString());
	}

	public void OnUpdateError(TargetFinder.UpdateState updateError) {
		Debug.Log ("Cloud Reco update error " + updateError.ToString());
	}

	// This action lets you know whether Vuforia is scanning the cloud
	public void OnStateChanged(bool scanning) {
		mIsScanning = scanning;

		if (scanning) {
			// Clear all known trackables
			var tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

			tracker.TargetFinder.ClearTrackables(false);
		}
	}

	// Here we handle a cloud target recognition event
	public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult) {
		// Do something with the target metadata
		mTargetMetadata = targetSearchResult.MetaData;

		// Stop the target finder (i.e. stop scanning the cloud)
		mCloudRecoBehaviour.CloudRecoEnabled = false;

		// Build augmentation based on target
		if (ImageTargetTemplate) {
			// Enable the new result with the same ImageTargetBehaviour:
			ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

			ImageTargetBehaviour imageTargetBehaviour =
				(ImageTargetBehaviour)tracker.TargetFinder.EnableTracking(
					targetSearchResult, ImageTargetTemplate.gameObject);
		}
	}

	void OnGUI() {
		// Set a default font size
		GUI.skin.box.fontSize = 20;

		// Display current 'scanning' status
		GUI.Box (new Rect(50, 50, 400, 100), mIsScanning ? "Scanning" : "Not scanning");

		GUI.skin = guiSkin;

		flashToggle = GUI.Toggle (new Rect (1500, 50, 100, 100), flashToggle, "Flash", "button");

		if (flashToggle) {
			CameraDevice.Instance.SetFlashTorchMode (true);
		} else {
			CameraDevice.Instance.SetFlashTorchMode (false);
		}

	}
}
