using UnityEngine;
using System.Collections;

public class MatchStick : MonoBehaviour {

	//public GameObject Stick;

    Point point = new Point();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Point GetPosition() {
        return this.point;
    }

    public void setPosition(float x, float y, float z) {
        transform.position = new Vector3(x, y, z);
        point.SetPoint(x, y);
        setRotation();
        Debug.Log("Position: " + point.GetX() + ", " + point.GetY());
    }

    // If the match stick is on a grid line, one of the axis will be at the mid point. If it is in the middle of the x
    // then it is on a horizontal line and the match stick should be oriented the same way.
    // If both axis are .5s/at the mid point, then the match stick is in the middle of the box and should be at a diagonal.
    void setRotation() {
        Vector3 currentPosition = transform.position;

        // If x is .5 and y is whole number, then we should orient horizontally.
        if (currentPosition.x != (int)currentPosition.x && (currentPosition.y == (int)currentPosition.y)) {
            //transform.Rotate(new Vector3(0, 0, 1), 90);
            transform.rotation = Quaternion.Euler(0, 0, 90);
        } else if (currentPosition.x == (int)currentPosition.x && (currentPosition.y != (int)currentPosition.y)) {
            // If x is whole number and y is half, orient vertically.
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        // TODO: add for middle
    }
    
    private void OnDestroy() {
        GameManager.instance.CheckLevel();
    }
}
