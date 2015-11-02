using UnityEngine;
using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public GridManager gridScript;
	public AudioClip placementSound;

	private AudioSource source;
	private static GameObject resetMenu;
    private static GameObject congratsPanel;
    // For some reason this variable isn't staying set so made this static.
    private static GameObject mainMenu;
	private static GameObject resetProgressButton;
	private static GameObject topMenu;
	private GameObject grid;
    private static int level = -1;
	private static IList<int> completedLevels = new List<int>();
    private bool checkLevel;
    private GameObject canvas;

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
		source = GetComponent<AudioSource>();

#if UNITY_WEBPLAYER
        Debug.Log("Web Player");
#endif
#if UNITY_IOS
		// Forces a different code path in the BinaryFormatter that doesn't rely on run-time code generation (which would break on iOS).
		Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER","yes");
        Debug.Log("IOS");
#endif
#if !UNITY_WEBPLAYER
        Load();
#endif
        instance.InitGame ();
	}

    // Update is called once per frame
    void Update () {

    }

	void InitGame() {
        canvas = GameObject.Find("Canvas");
        congratsPanel = GameObject.Find("Congrats Panel");
        congratsPanel.SetActive(false);
        mainMenu = GameObject.Find("MainMenu");
		resetMenu = GameObject.Find("ConfirmReset Panel");
		resetProgressButton = GameObject.Find("ResetProgressionButton");
		resetProgressButton.GetComponent<ResetButton>().SetResetMenu(resetMenu);
		resetMenu.SetActive(false);
		topMenu = GameObject.Find ("Top Menu");
		topMenu.SetActive(false);
		gridScript.SetupScene ();
        if (level == -1) {

        } else {
            LoadLevelInit(level);
            mainMenu.SetActive(false);
			resetProgressButton.SetActive(false);
        }
	}

    public void DisplayMainMenu(Boolean display) {
		if (grid == null) {
			grid = GameObject.Find("Grid");
		}
		grid.SetActive(false);
		congratsPanel.SetActive(false);
		topMenu.SetActive(false);
        mainMenu.SetActive(display);
		resetProgressButton.SetActive(display);
    }

    public void RestartLevel() {
        Application.LoadLevel(Application.loadedLevel);
    }

    private void LoadLevelInit(int level) {
        GameManager.level = level;
        LevelSetup(level);
    }

    public void LoadLevel(int level) {
        GameManager.level = level;
        RestartLevel();
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
			if (Debug.isDebugBuild)  {
                Debug.Log("LevelList not found or readable");
			}
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

	public IList<int> GetCompletedLevels() {
		return completedLevels;
	}

    public void CheckLevel() {
        CheckSolution();
    }

    // Add sticks to the grid and store solution for future checks.
    private void LevelSetup(int level) {
        if (Debug.isDebugBuild) {
		    Debug.Log ("Loading level: " + level);
		}
		topMenu.SetActive(true);
		if (grid == null) {
			grid = GameObject.Find("Grid");
		}
		source.PlayOneShot(placementSound, 1);
		grid.SetActive(true);
        StickStash stash = GameObject.FindGameObjectWithTag("Stash").GetComponent<StickStash>();
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

        if (startingStash == null) {
            stash.SetStash(0);
        } else {
            stash.SetStash(Int32.Parse(startingStash));
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

        // Array of Array possible solutions
        var solutionLists = json["solution"];

        // Handle any problems that might arise when reading the text
        try
        {
            GameObject[] sticks = GameObject.FindGameObjectsWithTag("Stick");
            IList<Point> stickPoints = new List<Point>();
            IList<Point> solutionPoints = new List<Point>();

            foreach (GameObject stick in sticks) {
                stickPoints.Add(stick.GetComponent<MatchStick>().GetPosition());
            }
            bool foundPossibleSolution = false;
            // While there's lines left in the text file, do this:
            for (int j = 0; j < solutionLists.Count; j++) {
                if (foundPossibleSolution) {
                    break;
                }
                var solutionList = solutionLists[j];
                solutionPoints.Clear();

                for (int i = 0; i < solutionList.Count; i++) {
                    
                    string[] point = ((String) solutionList[i]).Split(',');
                    
                    float x = -99;
                    float y = -99;
                    float r = -99;
                    try {
                        x = float.Parse(point[0]);
                    } catch (Exception e) {
						if (Debug.isDebugBuild)  {
                            Debug.LogError("Message: " + e.Message + "\nx value for line: " + point);
						}
					}
                    try { 
                        y = float.Parse(point[1]);
                    } catch (Exception e) {
						if (Debug.isDebugBuild)  {
                           Debug.LogError("Message: " + e.Message + "\ny value for line: " + point);
						}
                    }
                    try {
                        r = float.Parse(point[2]);
                    } catch (Exception e)  {
                        if (Debug.isDebugBuild) { 
                            Debug.LogError ("Message: " + e.Message + "\nr value for point: " + point);
                        }
                    }
                    if (x != -99 && y != -99 &&  r != -99) {
                        Point solutionPoint = new Point(x, y, r);

                        solutionPoints.Add(solutionPoint);
                        
                        // Check to see if a stick covers this point.
                        bool found = false;
                        foreach (Point stickPoint in stickPoints) {
                            if (stickPoint.IsEqual(solutionPoint)) {
                                found = true;
                                foundPossibleSolution = true;
                                break;
                            }
                        }
                        
                        if (!found) {
                            foundPossibleSolution = false;
							if (Debug.isDebugBuild)  {
                                Debug.Log("Wrong");
							}
                            // Don't need to search this solution list anymore, check next list
                            break;
                        }
                        //instance.GetComponent<MatchStick>().setPosition(x, y, 0);
                    }
                }
            }
            // If it didn't match any of the lists, don't need to do any more checks.
            if (!foundPossibleSolution) {
                return;
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
					if (Debug.isDebugBuild)  {
                        Debug.Log("Wrong");
					}
                    return;
                }
            }
            if (Debug.isDebugBuild) {
                Debug.Log("Correct");
		    }
			if (!completedLevels.Contains(level)) {
				completedLevels.Add(level);
#if !UNITY_WEBPLAYER
                    Save();
#endif
				
			}

            congratsPanel.SetActive(true);
            
        }
        // If anything broke in the try block, we throw an exception with information
        // on what didn't work
        catch (Exception e)
        {
            if (Debug.isDebugBuild) {
			    Debug.LogError(e.Message);
		    }
            Console.WriteLine("{0}\n", e.Message);
            //return false;
        }
    }
	
    public void NextLevel() {
        level++;
		if (Debug.isDebugBuild) {
			Debug.Log (level);
		}
        RestartLevel();
    }

	public void Save() {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerData.dat");

		PlayerData data = new PlayerData ();
		data.beatenLevels = completedLevels;

		bf.Serialize (file, data);
		file.Close ();
	}

	public void Load() {
		if (File.Exists(Application.persistentDataPath + "/playerData.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);
			PlayerData data = (PlayerData) bf.Deserialize(file);
			file.Close();

			completedLevels = data.beatenLevels;

			if (Debug.isDebugBuild) {
				Debug.Log ("Loading player data: " + Application.persistentDataPath + "/playerData.dat");
			}
		}
	}

	public void Delete() {
		if (File.Exists(Application.persistentDataPath + "/playerData.dat")) {
			File.Delete(Application.persistentDataPath + "/playerData.dat");
		}
		completedLevels.Clear();
	}

	[Serializable]
	class PlayerData {
		public IList<int>  beatenLevels;
	}
}
