using UnityEngine;
using SimpleJSON;
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

    public void AddStick(String pointString) {
        string[] point = pointString.Split(',');
        
        float x = -99;
        float y = -99;
        try {
            x = float.Parse(point[0]);
        } catch (Exception e) {
            Debug.LogError("Message: " + e.Message + "\nx value for point: " + pointString);
        }
        try { 
            y = float.Parse(point[1]);
        } catch (Exception e) {
            Debug.LogError("Message: " + e.Message + "\ny value for point: " + pointString);
        }
        if (x != -99 && y != -99) {
            GameObject instance = Instantiate (stick, new Vector3(0, 0, 0f), Quaternion.identity) as GameObject;
            instance.GetComponent<MatchStick>().setPosition(x, y, 0);
            instance.transform.SetParent(gridHolder);
        }

    }

    // Use this for initialization
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

}
