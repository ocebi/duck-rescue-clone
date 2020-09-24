using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public GameObject drawingPrefab;
    public GameObject activeDrawing;
    public Vector3 touchPosition;
    private bool canDraw = false;
    //private bool canDraw = true;

    private List<Vector3> points = new List<Vector3>();

    void Update()
    {
        /*
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
        {
            activeDrawing = (GameObject)Instantiate(drawingPrefab,
                transform.position,
                Quaternion.identity);
            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if(objPlane.Raycast(mRay, out rayDistance))
            {
                startPos = mRay.GetPoint(rayDistance);
            }
        }
        else if((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0))
        {
            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (objPlane.Raycast(mRay, out rayDistance))
            {
                startPos = mRay.GetPoint(rayDistance);
            }
        }
        else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) || Input.GetMouseButtonUp(0))
        {
            if(Vector3.Distance(activeDrawing.transform.position, startPos) < 0.1f)
            {
                Destroy(activeDrawing);
            }
        }
        */

        //if (Input.GetMouseButtonDown(0))
        if (Input.touchCount > 0)
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
                        canDraw = true;
                    }
                }

            }

            if (touch.phase == TouchPhase.Moved)
            {
                //FreeDraw();
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
            }
        }

        
        
        /*
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if(activeDrawing != null)
                {
                    Destroy(activeDrawing);
                }
                activeDrawing = Instantiate(drawingPrefab, transform.position, drawingPrefab.transform.rotation);
                lineRenderer = activeDrawing.GetComponent<LineRenderer>();
                lineRenderer.useWorldSpace = false;
                //lineRenderer.alignment = LineAlignment.;
                lineRenderer.transform.parent = transform;

                touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 26f));
                print("Touch pos: " + touchPosition);
                var distance = Vector3.Distance(touchPosition, transform.position);
                print("Distance: " + distance);
                if (Vector3.Distance(touchPosition, transform.position) < 10f) //if touch is not close to the gameobject, dont draw
                {
                    canDraw = true;
                }
            }

            if (touch.phase == TouchPhase.Moved)
            {
                if(canDraw)
                {
                    FreeDraw();
                }
            }

            if(touch.phase == TouchPhase.Ended)
            {
                canDraw = false;
                //canDraw = true;
            }
        }
        */
    }

    void FreeDraw()
    {
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 26f);
        //Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        //float rayDistance;
        //RaycastHit hit;
        //if (objPlane.Raycast(mRay, out rayDistance))
        //if (Physics.Raycast(ray, out hit))
        //{
            //startPos = mRay.GetPoint(rayDistance);
        //    startPos = hit.point;
        //}

        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, Camera.main.ScreenToWorldPoint(mousePos));
        //lineRenderer.SetPosition(lineRenderer.positionCount - 1, Camera.main.ScreenToWorldPoint(startPos));



    }
}
