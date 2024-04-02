using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Draw : MonoBehaviour
{
    public Camera m_camera;
    public GameObject brush;    


    LineRenderer currentLineRenderer;
    Vector2 lastPos;
    Queue dataQueue;
    string output;
    bool stringInit;

    void Start()
    {
        dataQueue = new Queue();
        stringInit = false;
        
    }


    // Update is called once per frame
    void Update()
    {
        Drawing();

        

            
        if (Input.GetKeyDown("space"))
        {
            print("appending char");
            var sb = new StringBuilder();
            stringInit = true;
            foreach (Vector2 point in dataQueue)
            {   
                float xPos = point.x;
                float yPos = point.y;
                sb.Append('\n').Append(xPos.ToString()).Append(',').Append(yPos.ToString());
                //print("("+xPos+","+yPos+")");
                
            }

            sb.Append('\n').Append("-9999").Append(',').Append("-9999");
            output += sb.ToString();
            
            dataQueue.Clear();

            //erase writing
            GameObject[] clones = GameObject.FindGameObjectsWithTag("clone");
            foreach (GameObject clone in clones)
            {
                Destroy(clone);
            }

        }
        if (Input.GetKeyDown("left"))
        {
            print("exporting data");
            SaveToFile(output);
        }




    }
    void Drawing()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CreateBrush();
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            PointToMousePos();
        }
        else
        {
            currentLineRenderer = null;
        }
    }



    void CreateBrush()
    {
        GameObject brushInstance = Instantiate(brush);
        brushInstance.tag = "clone";
        currentLineRenderer = brushInstance.GetComponent <LineRenderer>();

        //because you gotta have 2 points to start a line renderer, 
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);

        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);

        
    }

    void AddAPoint(Vector2 pointPos)
    {
        currentLineRenderer.positionCount++;
        int positionIndex = currentLineRenderer.positionCount - 1;
        currentLineRenderer.SetPosition(positionIndex, pointPos);
    }

    void PointToMousePos()
    {
        Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
        if (lastPos != mousePos)
        {
            AddAPoint(mousePos);
            lastPos = mousePos;
        }

        
        dataQueue.Enqueue(mousePos);
        
    }

    public void SaveToFile(string sb)
    {
        // Use the CSV generation from before
        var content = sb;

        // The target file path e.g.


        var folder = Application.dataPath;


        var filePath = Path.Combine(folder, "export.csv");

        using (var writer = new StreamWriter(filePath, false))
        {
            writer.Write(content);
        }

        // Or just
        //File.WriteAllText(content);

        Debug.Log($"CSV file written to \"{filePath}\"");


    }
}

