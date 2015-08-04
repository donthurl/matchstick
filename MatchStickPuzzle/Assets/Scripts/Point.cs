using UnityEngine;
using System.Collections;

public class Point {

    float x;
    float y;
    int rotation;

    public Point() {
        SetPoint(-99, -99, 0);
    }

    public Point(float x, float y) {
        SetPoint(x, y);
    }

    public void SetPoint(float x, float y) {
        SetPoint(x, y, 0);
    }

    public void SetPoint(float x, float y, int rotation) {
        this.x = x;
        this.y = y;
        this.rotation = rotation;
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

    public bool IsEqual(Point rhs) {
        if (rhs.GetX() == this.x && rhs.GetY() == this.y && rhs.GetRotation() == this.rotation) {
            return true;
        }

        return false;
    }
}
