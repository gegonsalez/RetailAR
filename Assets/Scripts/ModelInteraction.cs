using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModelInteraction : MonoBehaviour {

	// Cloud Handler
	public GameObject CloudRecognition;
	public SimpleCloudHandler cloudHandler;
	
	// 3D Model Object
	public GameObject modelObject;

	// List of materials of the 
	// 3D Model to be editted
	private Material[] objectMaterials;

	// List of materials of the 
	// 3D Model to be restored
	private Material[] originalObjectMaterials;

	// Original scale
	private Vector3 originalScale;

	// Texture to be pre-loaded for changing
	// GOLD
	public Material goldFrontCase;
	public Material goldBackCase;
	public Material goldGloss;

	// Toggle to display extra detail
	private bool displayDetails = false;

	// Use this for initialization
	void Start () {
		objectMaterials = modelObject.GetComponent<Renderer> ().materials;
		originalObjectMaterials = modelObject.GetComponent<Renderer> ().materials;
		originalScale = transform.localScale;

		cloudHandler = CloudRecognition.GetComponent<SimpleCloudHandler> ();
	}

	// Update is called once per frame
	void Update () {
		int noOfTouch = Input.touchCount;

		// For rotation
		if (noOfTouch == 1) {
			// Get touch
			Touch touch = Input.GetTouch (0);

			// Touch 3D Model to display more information on screen
			if (touch.phase == TouchPhase.Ended) {
				Ray raycast = Camera.main.ScreenPointToRay (touch.position);
				RaycastHit raycasthit;
				if (Physics.Raycast (raycast, out raycasthit)) {
					if (raycasthit.collider.CompareTag ("Phone")) {
						displayDetails = !displayDetails;
					}
				}
			}

			// Apply Rotation on the 3D Model
			if (touch.phase == TouchPhase.Moved) {
				// Rotate along the Z-axis using left-right swipes
				transform.Rotate (0f, 0f, touch.deltaPosition.x, Space.Self);
				// Rotate along the X-axis using up-down swipes
				//transform.Rotate (touch.deltaPosition.y, 0f, 0f, Space.Self);
			}
		}

		// For scaling
		else if (noOfTouch == 2) {
			// Get touch
			Touch touch0 = Input.GetTouch (0);
			Touch touch1 = Input.GetTouch (1);

			float deltaMagnitudeDiff = 0f;

			if (touch1.phase == TouchPhase.Moved) {
				// Find the position in the previous frame of each touch.
				Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
				Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

				// Find the magnitude of the vector (the distance) between the touches in each frame.
				float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
				float touchDeltaMag = (touch0.position - touch1.position).magnitude;

				deltaMagnitudeDiff = -(prevTouchDeltaMag - touchDeltaMag) * 0.1f;

				transform.localScale = Vector3.Lerp (transform.localScale, transform.localScale * deltaMagnitudeDiff, Time.deltaTime);
			}
		}
	}

	// Additional interaction for 3D Model using GUI's
	void OnGUI() {
		// Default font size
		GUI.skin.button.fontSize = 20;
		GUI.skin.box.fontSize = 20;

		// Button to change color to gold(white)
		if (GUI.Button (new Rect (500, 950, 200, 100), "White")) {
			objectMaterials [0] = goldBackCase;
			objectMaterials [3] = goldFrontCase;
			objectMaterials [8] = goldGloss;
			modelObject.GetComponent<Renderer> ().materials = objectMaterials;
		}

		// Button to reset the color
		if (GUI.Button (new Rect (750, 950, 200, 100), "Reset color")) {
			modelObject.GetComponent<Renderer> ().materials = originalObjectMaterials;
		}

		// Button to reset scale to original
		if (GUI.Button (new Rect (1000, 950, 200, 100), "Reset scale")) {
			transform.localScale = originalScale;
		}

		if (displayDetails) {
			GUI.Box (new Rect (1500, 500, 200, 200), cloudHandler.mTargetMetadata);
		}
	}
}
