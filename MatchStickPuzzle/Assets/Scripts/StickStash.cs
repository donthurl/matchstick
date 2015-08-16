using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StickStash : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public GameObject stick;

    static bool pointerEntered = false;

    // The new instance that will be created on drag.
    GameObject instance;
    private RectTransform canvasRectTransform;
    private int stashNum = 0;
    private Text stashText;

	// Use this for initialization
	void Start () {
        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null) {
            canvasRectTransform = canvas.transform as RectTransform;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    #region IBeginDragHandler implementation
    public void OnBeginDrag(PointerEventData eventData) {
        if (stashNum <= 0) {
            return;
        }
        Debug.Log("Start Drag");
        Vector3 currentPosition = transform.position;
        instance = Instantiate (stick, currentPosition, Quaternion.identity) as GameObject;
        //instance.GetComponent<MatchStick>().setPosition(x, y, 0);
        
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    #endregion

    #region IDragHandler implementation

    public void OnDrag(PointerEventData eventData) {
        if (stashNum <= 0) {
            return;
        }
        Vector2 pointerPosition = ClampToWindow(eventData);

        float currentZ = transform.position.z;
        
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        point.z = currentZ;
        instance.transform.position = point;
    }

    #endregion

    #region IEndDragHandler implementation

    public void OnEndDrag(PointerEventData eventData) {
        if (stashNum <= 0) {
            return;
        }

        Vector3 currentPosition = instance.transform.position;
        
        // Snap to .5
        float x = Mathf.Round(currentPosition.x * 2) / 2;
        float y = Mathf.Round(currentPosition.y * 2) / 2;
        float z = Mathf.Round(currentPosition.z);
        
        instance.GetComponent<MatchStick>().setPosition(x, y, z);
        //MatchStick matchStickScript = GetComponent<MatchStick>();
        //if (matchStickScript != null) {
        //    matchStickScript.setPosition(x, y, z);
        //}
        GameManager.instance.CheckLevel();
        /*
        transform.position = new Vector3(Mathf.Round(currentPosition.x),
                                     Mathf.Round(currentPosition.y),
                                     Mathf.Round(currentPosition.z));
        */
        RemoveFromStash();
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    #endregion

    // Used for dropping GameObject matches on it
    public void OnPointerEnter(BaseEventData eventData) {
        pointerEntered = true;
    }

    public void OnPointerExit(BaseEventData eventData) {
        pointerEntered = false;
    }

    public bool OnStash() {
        return pointerEntered;
    }

    public void AddToStash() {
        stashNum++;
        Debug.Log("Stash: " + stashNum);
        SetStashText(stashNum);
    }

    public void RemoveFromStash() {
        stashNum--;
        Debug.Log("Stash: " + stashNum);
        SetStashText(stashNum);
    }

    public void SetStash(int stashNum) {
        this.stashNum = stashNum;
        if (stashText == null) {
            stashText = GameObject.Find("StashText").GetComponent<Text>();
        }
        SetStashText(stashNum);
    }

    Vector2 ClampToWindow(PointerEventData data) {
        Vector2 rawPointerPosition = data.position;

        Vector3[] canvasCorners = new Vector3[4];
        canvasRectTransform.GetWorldCorners(canvasCorners);

        float clampedX = Mathf.Clamp(rawPointerPosition.x, canvasCorners [0].x, canvasCorners [2].x);
        float clampedY = Mathf.Clamp(rawPointerPosition.y, canvasCorners [0].y, canvasCorners [2].y);

        Vector2 newPointerPosition = new Vector2(clampedX, clampedY);
        return newPointerPosition;
    }

    private void SetStashText(int stashNum) {
        if (stashText == null) {
            stashText = GameObject.Find("StashText").GetComponent<Text>();
        }
        stashText.text = stashNum + "";
    }
}
