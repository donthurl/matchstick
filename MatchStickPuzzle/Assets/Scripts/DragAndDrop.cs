using UnityEngine;
using System.Collections;

public class DragAndDrop : MonoBehaviour {
    Vector3 point;
	private Vector3 screenPoint;
	private Vector3 offset;
	private float lastClick = 0;
	private float doubleClickDelay = 0.3f;
	private bool doubleClick = false;

    // Use this for initialization
    void Start() {
    
    }
    
    // Update is called once per frame
    void Update() {
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));

	}
	
	void OnMouseDown(){
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

		// Checking for double click
		if (Time.time - lastClick < doubleClickDelay) {
			Debug.Log ("DoubleClicked");
			doubleClick = true;
			MatchStick matchStickScript = GetComponent<MatchStick> ();
			if (matchStickScript != null) {
				matchStickScript.Rotate();
			}
			GameManager.instance.CheckLevel();
		} else {
			doubleClick = false;
		}

		lastClick = Time.time;
	}
	
	void OnMouseDrag(){
		if (!doubleClick) {
			Vector3 cursorPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (cursorPoint) + offset;
			cursorPosition.z = transform.position.z;
			transform.position = cursorPosition;
		}
	}
	
	void OnMouseUp() {
		if (!doubleClick) {
			Vector3 currentPosition = transform.position;

			GameObject stash = GameObject.FindGameObjectWithTag ("Stash");
			// Check to see if we dropped on the stash. If so, then...
			if (stash.GetComponent<StickStash> ().OnStash ()) {
				// The stick's OnDestroy will call a check.
				// Do that instead of checking solution right here because
				// the stick won't actually get destroyed/removed until next frame.
				Destroy (gameObject);
				stash.GetComponent<StickStash> ().AddToStash ();
				return;
			}

			// Snap to .5
			float x = Mathf.Round (currentPosition.x * 2) / 2;
			float y = Mathf.Round (currentPosition.y * 2) / 2;
			float z = Mathf.Round (transform.eulerAngles.z);


			MatchStick matchStickScript = GetComponent<MatchStick> ();
			if (matchStickScript != null) {
				matchStickScript.dropAt (x, y, z);
			}
			GameManager.instance.CheckLevel ();
			/*
        transform.position = new Vector3(Mathf.Round(currentPosition.x),
                                     Mathf.Round(currentPosition.y),
                                     Mathf.Round(currentPosition.z));
        */
		}
    }
}