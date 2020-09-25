using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector]
    public static PlayerManager instance = null;

    private List<Vector3> playerMoveVector = new List<Vector3>();
    public bool gamePhase = false;
    public float moveSpeed;
    private int moveVectorIndex = 0;
    private Rigidbody rb;
    //private float velocityToAdd = 0;
    private NavMeshAgent navMeshAgent;
    //private bool startedMoving = false;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = GetComponent<PlayerManager>();
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gamePhase)
        {
            //print("GamePhase true");
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Stationary)
                {
                    if(moveVectorIndex < (playerMoveVector.Count - 1))
                    {
                        float distance = Vector3.Distance(playerMoveVector[moveVectorIndex], transform.position);
                        
                        
                        if(distance > 0.5f)
                        {
                            //startedMoving = true;
                            Vector3 movement = transform.forward * Time.deltaTime * moveSpeed;
                            navMeshAgent.Move(movement);
                        }
                        else
                        {
                            ++moveVectorIndex;
                            transform.LookAt(playerMoveVector[moveVectorIndex]);
                        }
                        
                        //print("Iteration: " + moveVectorIndex);
                        //var speed = 10f;
                        //transform.LookAt(playerMoveVector[moveVectorIndex]);
                        //canMove = true;
                        /*
                        transform.position = Vector3.MoveTowards(transform.position, playerMoveVector[moveVectorIndex], Time.deltaTime * speed);

                        if (transform.position == playerMoveVector[moveVectorIndex])
                        {
                            //canMove = false;
                            ++moveVectorIndex;
                        }
                        */
                    }
                    
                }
                if(touch.phase == TouchPhase.Ended)
                {
                    //canMove = false;
                    //rb.velocity = Vector3.zero;
                }
                /*
                if (touch.phase == TouchPhase.Moved)
                {
                    if (moveVectorIndex < playerMoveVector.Count)
                    {
                        transform.position = playerMoveVector[moveVectorIndex++];
                    }
                }
                */
            }
        }
    }

    private void FixedUpdate()
    {
        /*
        if(canMove)
        {
            rb.velocity = Vector3.forward * 2f;
            if (transform.position == playerMoveVector[moveVectorIndex])
            {
                ++moveVectorIndex;
            }
        }
        */
        
        //rb.MovePosition(playerMoveVector[moveVectorIndex]);
        //++moveVectorIndex;
        //rb.AddForce(Vector3.forward * 2f, ForceMode.Force);
        //++moveVectorIndex;
        /*
        if(canMove)
        {
            transform.position = Vector3.forward * Time.deltaTime;
            if(transform.position == playerMoveVector[moveVectorIndex])
            {
                canMove = false;
                ++moveVectorIndex;
            }
            
            rb.AddForce(Vector3.forward * 2f, ForceMode.Force);
            ++moveVectorIndex;
            canMove = false;
            
        }
        */
    }

    public void StartGamePhase()
    {
        /*
        for(int i=0;i<DrawManager.instance.lineRenderer.positionCount; ++i)
        {
            playerMoveVector.Add(DrawManager.instance.lineRenderer.GetPosition(i));
        }
        */
        Vector3[] tempVector = new Vector3[DrawManager.instance.lineRenderer.positionCount];
        DrawManager.instance.lineRenderer.GetPositions(tempVector);
        playerMoveVector = new List<Vector3>(tempVector);
        transform.LookAt(playerMoveVector[moveVectorIndex]);
        //NavMeshPath path = new NavMeshPath();
        //path.
        //GetComponent<NavMeshAgent>().SetPath(tempVector);
        //playerMoveVector = DrawManager.instance.points;
        //playerMoveVector = new List<Vector3>(DrawManager.instance.lineRenderer.GetPosition);
        gamePhase = true;
        //print("Setting move vector. Count: " + playerMoveVector.Count);
    }
}
