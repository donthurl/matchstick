using UnityEngine;
using System.Collections;

public class ResetButton : MonoBehaviour {

	private GameObject confirmPanel;

	// Use this for initialization
	void Start () {
//		confirmPanel = GameObject.Find("ConfirmReset Panel");
//		if (confirmPanel != null) {
//			confirmPanel.SetActive (false);
//		}
	}
	
	public void SetResetMenu(GameObject confirmPanel) {
		this.confirmPanel = confirmPanel;
	}

	public void ShowResetConfirmMenu() {
		confirmPanel.SetActive(true);
	}
}
