using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GestureTest : MonoBehaviour 
{
    public List<GameObject> objlist = new List<GameObject>();


	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void SetPointToShow(Vector3 pos)
    {
        GameObject tmp = GameObject.CreatePrimitive(PrimitiveType.Cube);

        tmp.transform.position = pos;

        tmp.transform.localScale = new Vector3(5,5,5);

        
        objlist.Add(tmp);
    }

    public void ClearPoints()
    {
        foreach (GameObject tmp in objlist)
        {
            Object.DestroyImmediate(tmp);
        }
        objlist.Clear();
    
    }


    public void SaveJson(string txt)
    {
        string path=Application.dataPath+"//Resource//";

        path = "./Assets/Resources/TXT/Edit_NewGesture.txt";
       
        string file = path;
         
        StreamWriter sw;

        FileInfo t = new FileInfo(file);

      
        if(!t.Exists)
        {
           // Debug.LogError("SaveJson1: " + path);

            sw = t.CreateText();
        }
        else
        {
            //Debug.LogError("SaveJson2: " + path);

            sw = t.AppendText();
        }
        sw.WriteLine(txt);
        sw.Close();
        sw.Dispose();
        
    }

    void Clear()
    {
#if UNITY_EDITOR
        string path = Application.dataPath + "//Resource//";

        path = "./Assets/Resources/TXT/Edit_NewGesture.txt";

        string file = path;

        StreamWriter sw;

        FileInfo t = new FileInfo(file);


        if (t.Exists)
        {
            Debug.LogError(" delet@£¡");
            t.Delete();
        }
        sw = t.CreateText();
        sw.Close();
        sw.Dispose();
#endif
    }
    void RealSave()
    {
       DCGesture gesture =  Object.FindObjectOfType(typeof(DCGesture)) as DCGesture;

       List<Point2D> tmplist = gesture.GetPoint2DList();

        SaveJson(" public List<Point2D> getGestureLighting4()");

        SaveJson("{");

        SaveJson("  List<Point2D> path = new List<Point2D>();");

        for (int i = 0; i < tmplist.Count; i++)
        {
            SaveJson(" path.Add(new Point2D( " + tmplist[i].x + "," + tmplist[i].y + "));");

            SetPointToShow(tmplist[i].ToVector3());
        }
        SaveJson(" return path;");

        SaveJson("}");
    }


    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 50, 60), "Save"))
        {
            RealSave();
        }
        if (GUI.Button(new Rect(10, 70, 50, 60), "Clear"))
        {
            Clear();
        }
    }
}
