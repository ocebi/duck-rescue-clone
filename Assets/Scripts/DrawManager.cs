using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    [HideInInspector]
    public LineRenderer lineRenderer;
    [HideInInspector]
    public List<Vector3> points = new List<Vector3>();
    [HideInInspector]
    public static DrawManager instance = null; //simple singleton

    public GameObject drawingPrefab;
    public GameObject activeDrawing;
    private bool canDraw = false;
    public GameObject finish;

    private void Awake()
    {
        instance = GetComponent<DrawManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0 && PlayerManager.instance.gamePhase == false)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (activeDrawing != null)
                {
                    Destroy(activeDrawing);
                }

                points.Clear();

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    var pointToAdd = hitInfo.point;
                    pointToAdd.y = 0;

                    if (Vector3.Distance(transform.position, pointToAdd) > 2)
                    {
                        canDraw = false;
                    }
                    else
                    {
                        activeDrawing = Instantiate(drawingPrefab);
                        lineRenderer = activeDrawing.GetComponent<LineRenderer>();
                        lineRenderer.startWidth = 0.2f;
                        lineRenderer.endWidth = 0.2f;
                        lineRenderer.numCornerVertices = 5;
                        //lineRenderer.numCapVertices = 5;
                        canDraw = true;
                    }
                }

            }

            if (touch.phase == TouchPhase.Moved)
            {
                if (canDraw)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitInfo;
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        //print("point: " + hitInfo.point);
                        var pointToAdd = hitInfo.point;
                        pointToAdd.y = 0;
                        
                        points.Add(pointToAdd);
                        lineRenderer.positionCount = points.Count;
                        lineRenderer.SetPositions(points.ToArray());
                        
                    }
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                canDraw = false;
                if(points.Count > 0)
                {
                    if (points[points.Count - 1].z >= finish.transform.position.z)
                    {
                        int pointsBefore = lineRenderer.positionCount;
                        lineRenderer.Simplify(0.05f);
                        Debug.Log("Line reduced from " + pointsBefore + " to " + lineRenderer.positionCount);
                        PlayerManager.instance.StartGamePhase();
                    }
                    else if (activeDrawing != null)
                    {
                        Destroy(activeDrawing);
                    }
                }
            }
        }
        
    }
    
}
