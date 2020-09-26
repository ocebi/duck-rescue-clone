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
    public static DrawManager instance = null; //singleton

    public GameObject drawingPrefab;
    public GameObject activeDrawing;
    private bool canDraw = false;
    public GameObject finish;
    public GameObject obstaclesParent;
    public Material obstacleMaterial_Transparent;
    public Material obstacleMaterial_Opaque;

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
                //int mask = ~(1 << 8);
                //if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, mask))
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
                        lineRenderer.startWidth = 0.5f;
                        lineRenderer.endWidth = 0.5f;
                        lineRenderer.numCornerVertices = 5;
                        lineRenderer.numCapVertices = 5;
                        canDraw = true;
                    }
                }
                //GameManager.instance.ToggleInfoText(false);
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
                        pointToAdd.y = -0.1f;
                        
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
                    float distanceToFinish = Vector3.Distance(points[points.Count - 1], finish.transform.position);
                    print(distanceToFinish);
                    if (distanceToFinish < 3.5f)
                    {
                        GameManager.instance.ToggleInfoText(false);
                        lineRenderer.Simplify(0.1f); //reduces the number of points in the path
                        PlayerManager.instance.StartGamePhase();

                        for (int i = 0; i < obstaclesParent.transform.childCount; ++i) //change layer mask and materials of the obstacles
                        {
                            obstaclesParent.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Obstacles");
                            obstaclesParent.transform.GetChild(i).GetComponent<MeshRenderer>().material = obstacleMaterial_Opaque;
                        }

                    }
                    else
                    {
                        if (activeDrawing != null)
                        {
                            Destroy(activeDrawing);
                        }
                    }
                }
            }
        }
        
    }
    
}
