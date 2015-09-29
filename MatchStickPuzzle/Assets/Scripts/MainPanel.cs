using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainPanel : MonoBehaviour {

    public GameObject levelButton;

    private IList<string> levels;

	void Start () {
        CreateLevelButtons(GameManager.instance.GetLevelNames(), GameManager.instance.GetCompletedLevels());
	}

    public void CreateLevelButtons(IList<string> levels, IList<int> solvedLevels) {
        this.levels = levels;
        foreach (string level in levels) {
            GameObject button = Instantiate(levelButton) as GameObject;
            button.transform.SetParent(transform);
            button.GetComponent<LevelButton>().SetText(level);
            try
            {
                int m = Int32.Parse(level);
                button.GetComponent<LevelButton>().SetLevel(m);
				if (solvedLevels.Contains(m)) {
					button.GetComponent<LevelButton>().SetSolved();
				}
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }
            // Output: Input string was not in a correct format.
        }
    }
}
