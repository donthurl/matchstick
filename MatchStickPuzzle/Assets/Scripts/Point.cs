using UnityEngine;
using System.Collections;

public class Point {

    float x;
    float y;
	// Transform angles to be either 0, 45, 90, 135
    int rotation;

    public Point() {
        SetPoint(-99, -99, 0);
    }

    public Point(float x, float y) {
        SetPoint(x, y);
    }

	public Point(float x, float y, int rotation) { 
		SetPoint(x, y, rotation);
	}

	public Point(float x, float y, float r) {
		SetPoint(x, y, r);
	}

    public void SetPoint(float x, float y) {
        SetPoint(x, y, 0);
    }

	public void SetPoint(float x, float y, float r) {
		this.x = x;
		this.y = y;
		SetRotation(r);
	}

    public float GetX() {
        return this.x;
    }

    public float GetY() {
        return this.y;
    }

    public int GetRotation() {
        return this.rotation;
    }

	// |=0 or 180 /=135 or 315 --=90 or 270 \=225 or 45
	public void SetRotation(float rotation) {
		int r = Mathf.RoundToInt (rotation);
		if (r > 136) { 
			this.rotation = r - 180;
		} else if (r < 0) {
			this.rotation = r + 180;
		} else {
			this.rotation = r;
		}
	}

	public void Rotate(int rotate)  {
		SetRotation(rotation + rotate);
	}

    public bool IsEqual(Point rhs) {
        if (rhs.GetX() == this.x && rhs.GetY() == this.y && rhs.GetRotation() == this.rotation) {
            return true;
        }

        return false;
    }

	public string Print() {
		return x + ", " + y + ", " + rotation;
	}
}
