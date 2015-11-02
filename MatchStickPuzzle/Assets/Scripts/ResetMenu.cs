using UnityEngine;
using System.Collections;

public class ResetMenu : MonoBehaviour {

	private GameObject confirmPanel;
	
	// Use this for initialization
	void Start () {
		confirmPanel = GameObject.Find("ConfirmReset Panel");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetResetMenu(GameObject confirmPanel) {
		this.confirmPanel = confirmPanel;
	}

	public void ResetProgressionData() {
		GameManager.instance.Delete();
		confirmPanel.SetActive (false);
	}

	public void Cancel() {
		confirmPanel.SetActive (false);
	}
}
