using UnityEngine;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public GridManager gridScript;

    private GameObject congratsPanel;
    private GameObject mainMenu;
    private static int level = -1;
    private bool checkLevel;

    private IList<Point> solutionPoints = new List<Point>();

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
		gridScript = GetComponent<GridManager> ();
		instance.InitGame ();
	}

    // Update is called once per frame
    void Update () {

    }

	void InitGame() {
        Debug.Log("NextLevel: " + level);
        congratsPanel = GameObject.Find("Congrats Panel");
        congratsPanel.SetActive(false);
        mainMenu = GameObject.Find("MainMenu");
		gridScript.SetupScene ();
        if (level == -1) {

        } else {
            LoadLevelInit(level);
            mainMenu.SetActive(false);
        }
	}

    void Restart() {
        Application.LoadLevel(Application.loadedLevel);
    }

    private void LoadLevelInit(int level) {
        GameManager.level = level;
        LevelSetup(level);
    }

    public void LoadLevel(int level) {
        GameManager.level = level;
        Restart();
    }

    // Auto gets called when next level is loaded.
    void OnLevelWasLoaded(int index) {
        InitGame();
    }

    public IList<string> GetLevelNames() {
        IList<string> levels = new List<string>();
        string file = "Levels/LevelList";

        TextAsset levelList = (TextAsset)Resources.Load(file, typeof(TextAsset));
        // levelList.text contains whole file.
        // Read line by line
        StringReader reader = new StringReader(levelList.text);
        if (reader == null) {
            Debug.Log("LevelList not found or readable");
        } else {
            string line;
            while ((line = reader.ReadLine()) != null) {
                levels.Add(line);
            }
        }
//        try
//        {
//            string[] fullPaths = Directory.GetDirectories(path);
//            foreach (string s in fullPaths) {
//                levels.Add(s.Remove(0, path.Length));
//            }
//            return levels;
//        }
        // If anything broke in the try block, we throw an exception with information
        // on what didn't work
//        catch (Exception e)
//        {
//            Debug.LogError(e.Message);
//            Console.WriteLine("{0}\n", e.Message);
//            //return false;
//        }

        return levels;
    }

    public void CheckLevel() {
        CheckSolution();
    }

    // Add sticks to the grid and store solution for future checks.
    private void LevelSetup(int level) {        
        Debug.Log("Loading level: " + level);
        //this.level = level;
        
        TextAsset levelText = (TextAsset)Resources.Load("Levels/" + level + "/level", typeof(TextAsset));
        var json = JSONNode.Parse(levelText.text);
        // Array of strings of points separated by commas ex ["2, 2.5", "2.5, 2"]
        var levelList = json["level"];
        var solutionList = json["solution"];
        String title = json ["title"];
        String description = json ["description"];
        String startingStash = json ["startingStash"];

        for (int i = 0; i < levelList.Count; i++) {
            gridScript.AddStick(levelList[i]);
        }

        //for (int i = 0; i < solutionList.Count; i++) {
            // Bring below up here
       // }

        ((Text) GameObject.Find("Level Description").GetComponent<Text>()).text = description;
    }

    // TODO Move this up and create a new class called Point that has x/y/ direction
    private void CheckSolution() {
        TextAsset levelText = (TextAsset)Resources.Load("Levels/" + level + "/level", typeof(TextAsset));
        var json = JSONNode.Parse(levelText.text);

        var solutionList = json["solution"];

        // Handle any problems that might arise when reading the text
        try
        {
            GameObject[] sticks = GameObject.FindGameObjectsWithTag("Stick");
            IList<Point> stickPoints = new List<Point>();
            IList<Point> solutionPoints = new List<Point>();
            string line;
            StringReader theReader = new StringReader(levelText.text);
            using(theReader) {
                foreach (GameObject stick in sticks) {
                    stickPoints.Add(stick.GetComponent<MatchStick>().GetPosition());
                }
                // While there's lines left in the text file, do this:
                for (int i = 0; i < solutionList.Count; i++) {
                    line = theReader.ReadLine();
                    if (line != null) {
                        // Do whatever you need to do with the text line, it's a string now
                        // In this example, I split it into arguments based on comma
                        // deliniators, then send that array to DoStuff()
                        //Debug.Log(line);
                        
                        string[] point = ((String) solutionList[i]).Split(',');
                        
                        float x = -99;
                        float y = -99;
                        try {
                            x = float.Parse(point[0]);
                        } catch (Exception e) {
                            Debug.LogError("Message: " + e.Message + "\nx value for line: " + line);
                        }
                        try { 
                            y = float.Parse(point[1]);
                        } catch (Exception e) {
                            Debug.LogError("Message: " + e.Message + "\ny value for line: " + line);
                        }
                        if (x != -99 && y != -99) {
                            Point solutionPoint = new Point(x, y);

                            solutionPoints.Add(solutionPoint);
                            
                            // Check to see if a stick covers this point.
                            bool found = false;
                            foreach (Point stickPoint in stickPoints) {
                                if (stickPoint.IsEqual(solutionPoint)) {
                                    found = true;
                                    break;
                                }
                            }
                            
                            if (!found) {
                                Debug.Log("Wrong");
                                return;
                            }
                            //instance.GetComponent<MatchStick>().setPosition(x, y, 0);
                        }
                    }
                }

                // Check opposite way.
                // If any stick isn't part of the solution, then bad.
                foreach (Point point in stickPoints) {
                    bool found = false;
                    foreach (Point sPoint in solutionPoints) {
                        if (point.IsEqual(sPoint)) {
                            found = true;
                            break;
                        }
                    }
                    if (!found) {
                        Debug.Log("Wrong");
                        return;
                    }
                }
                
                // Done reading, close the reader and return true to broadcast success
                theReader.Close();
                
                Debug.Log("Correct");
                congratsPanel.SetActive(true);
            }
        }
        // If anything broke in the try block, we throw an exception with information
        // on what didn't work
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            Console.WriteLine("{0}\n", e.Message);
            //return false;
        }
    }
	
    public void NextLevel() {
        Debug.Log(level);
        level++;
        Debug.Log(level);
        Restart();
    }
}
