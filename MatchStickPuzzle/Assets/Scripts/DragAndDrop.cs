using UnityEngine;
using System.Collections;

public class DragAndDrop : MonoBehaviour {
    Vector3 point;

    // Use this for initialization
    void Start() {
    
    }
    
    // Update is called once per frame
    void Update() {
    
    }

    void OnMouseDrag() {
        float currentZ = transform.position.z;

        point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        point.z = currentZ;
        transform.position = point;
    }

    void OnMouseUpAsButton() {
        Vector3 currentPosition = transform.position;

        GameObject stash = GameObject.FindGameObjectWithTag("Stash");
        // Check to see if we dropped on the stash. If so, then...
        if (stash.GetComponent<StickStash>().OnStash()) {
            // The stick's OnDestroy will call a check.
            // Do that instead of checking solution right here because
            // the stick won't actually get destroyed/removed until next frame.
            Destroy(gameObject);
            stash.GetComponent<StickStash>().AddToStash();
            return;
        }

        // Snap to .5
        float x = Mathf.Round(currentPosition.x * 2) / 2;
        float y = Mathf.Round(currentPosition.y * 2) / 2;
        float z = Mathf.Round(currentPosition.z);


        MatchStick matchStickScript = GetComponent<MatchStick>();
        if (matchStickScript != null) {
            matchStickScript.setPosition(x, y, z);
        }
        GameManager.instance.CheckLevel();
        /*
        transform.position = new Vector3(Mathf.Round(currentPosition.x),
                                     Mathf.Round(currentPosition.y),
                                     Mathf.Round(currentPosition.z));
        */
    }
}