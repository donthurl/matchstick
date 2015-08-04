using UnityEngine;
using System;
using System.Collections.Generic;//Allows us to use Lists.;
using System.Text;
using System.IO;

public class GridManager : MonoBehaviour {

    public int columns = 8;
    public int rows = 8;
    public GameObject stick;

    int level;

    // Put all grid items in a grid dir/folder to keep things organized.
    private Transform gridHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseList() {
        gridPositions.Clear();

        for(int x = 1; x < columns - 1; x++) {
            for(int y = 1; y < rows - 1; y++) {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void GridSetup() {
        gridHolder = new GameObject("Grid").transform;

        return;

        for(int x = -1; x < columns + 1; x++) {
            for(int y = -1; y < rows + 1; y++) {
                GameObject toInstantiate = stick;

                //GameObject instance = Instantiate (toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                //instance.transform.SetParent(gridHolder);
            }
        }

        Color c1 = Color.white;
        Color c2 = Color.green;
        Vector3 startPoint;
        Vector3 endPpoint;
        //Material whiteDiffuseMat = new Material(Shader.Find("Unlit/Texture"));
        LineRenderer lineRenderer;

        GameObject lineObj;

        for(int x = 0; x < columns + 1; x++) {
            for(int y = 0; y < rows + 1; y++) {

                startPoint = new Vector3(x, y, 0f);
                endPpoint = new Vector3(x + 1, y, 0f);

                lineObj = new GameObject();

                lineRenderer = lineObj.AddComponent<LineRenderer>();
                lineRenderer.SetColors(c1, c2);
                //lineRenderer.material = whiteDiffuseMat;
                lineRenderer.SetWidth(0.1F, 0.1F);
                lineRenderer.SetPosition(0, startPoint);
                lineRenderer.SetPosition(1, endPpoint);

                startPoint = new Vector3(x, y, 0f);
                endPpoint = new Vector3(x, y + 1, 0f);

                lineObj = new GameObject();
                
                lineRenderer = lineObj.AddComponent<LineRenderer>();
                lineRenderer.SetColors(c1, c2);
                //lineRenderer.material = whiteDiffuseMat;
                lineRenderer.SetWidth(0.1F, 0.1F);
                lineRenderer.SetPosition(0, startPoint);
                lineRenderer.SetPosition(1, endPpoint);
            }
        }
    }

    public void SetupScene() {
        GridSetup();
        InitialiseList();
    }

    public void LoadLevel(int level) {
        // Handle any problems that might arise when reading the text
        
        Debug.Log("Loading level: " + level);
        this.level = level;
        TextAsset levelText = (TextAsset)Resources.Load("Levels/" + level + "/level", typeof(TextAsset));
        try
        {            
            string line;
            StringReader theReader = new StringReader(levelText.text);

            //using(theReader) {
                // While there's lines left in the text file, do this:
                do {
                    line = theReader.ReadLine();
                    if (line != null) {
                        // Do whatever you need to do with the text line, it's a string now
                        // In this example, I split it into arguments based on comma
                        // deliniators, then send that array to DoStuff()
                        //Debug.Log(line);

                        string[] point = line.Split(',');

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
                            GameObject instance = Instantiate (stick, new Vector3(0, 0, 0f), Quaternion.identity) as GameObject;
                            instance.GetComponent<MatchStick>().setPosition(x, y, 0);
                            instance.transform.SetParent(gridHolder);
                        }
                    }
                } while (line != null);
                
                // Done reading, close the reader and return true to broadcast success
               // theReader.Close();
            //}
        }
        // If anything broke in the try block, we throw an exception with information
        // on what didn't work
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            Debug.LogError(e.StackTrace);
            Console.WriteLine("{0}\n", e.Message);
            //return false;
        }
    }

    // Use this for initialization
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

}
