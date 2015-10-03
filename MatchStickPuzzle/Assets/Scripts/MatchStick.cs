using UnityEngine;
using System.Collections;

public class MatchStick : MonoBehaviour {

    //public GameObject Stick;
    public AudioClip placementSound;

    Point point = new Point();
    private AudioSource source;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake () {
        source = GetComponent<AudioSource>();
    }

    public Point GetPosition() {
        return this.point;
    }

    /* Calls setPosition and plays audio sound */
    public void dropAt(float x, float y, float z) {
        setPosition(x, y, z);
        source.PlayOneShot(placementSound, 1);
    }
    public void setPosition(float x, float y, float z) {
        transform.position = new Vector3(x, y, 0);
		Rotate(z);
        point.SetPoint(x, y, z);
    }

	/* Rotate clockwise by 45 degrees */
	public void Rotate() {
		// Rotate clockwise.
		transform.Rotate (0, 0, -45);
		point.Rotate(-45);
        if (Debug.isDebugBuild) {
			Debug.Log ("Rotation: " + transform.eulerAngles.z +  "  Point rotation: " +  point.GetRotation());
		}
	}

	public void Rotate(float rotation)  {
		transform.rotation = Quaternion.Euler(0, 0, rotation);
		point.SetRotation(rotation);
	}
    
    private void OnDestroy() {
        GameManager.instance.CheckLevel();
    }
}
