using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelButton : MonoBehaviour {

	public Sprite winSprite;
    private int level;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetText(string text) {
        gameObject.GetComponentInChildren<Text>().text = text;
    }

    public void SetLevel(int level) {
        this.level = level;
    }

    public void LoadLevel() {
        GameManager.instance.LoadLevel(level);
    }

	public void SetSolved() {
		Button button = gameObject.GetComponent<Button>();
		button.image.sprite = winSprite;
	}
}
