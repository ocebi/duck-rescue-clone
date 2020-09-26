using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector]
    public static PlayerManager instance = null;

    private List<Vector3> playerMoveVector = new List<Vector3>();
    [HideInInspector]
    public bool gamePhase = false;
    public float moveSpeed;
    private int moveVectorIndex = 0;
    private Rigidbody rb;
    private NavMeshAgent navMeshAgent;
    //private bool isMoving = false;

    public GameObject followerPrefab;

    public List<GameObject> followerList;

    #region Unity Methods
    private void Awake()
    {
        instance = GetComponent<PlayerManager>();
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        followerList = new List<GameObject>();
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
                /*
                if(touch.phase == TouchPhase.Began)
                {

                }
                */
                if (touch.phase == TouchPhase.Stationary)
                {
                    //print("Velocity: " + navMeshAgent.velocity);
                    if(moveVectorIndex < playerMoveVector.Count)
                    {
                        float distance = Vector3.Distance(playerMoveVector[moveVectorIndex], transform.position);
                        //isMoving = true;
                        GetComponent<Animator>().SetBool("Run", true);
                        int i = 0;
                        foreach(GameObject go in followerList)
                        {
                            go.GetComponent<Animator>().SetBool("Run", true);
                            Vector3 vectorToLook;
                            if (i == 0)
                            {
                                vectorToLook = transform.position;
                            }
                            else
                            {
                                vectorToLook = followerList[i - 1].transform.position;
                            }
                            vectorToLook.y = go.transform.position.y; //y position of the looked object must be the same with the looker for lookAt function
                            go.transform.LookAt(vectorToLook);
                            ++i;
                        }
                        print("Distance: " + distance);
                        if (distance > 0.5f) //haven't reached next point
                        {
                            //isMoving = true;
                            Vector3 movement = transform.forward * Time.deltaTime * moveSpeed;
                            navMeshAgent.Move(movement);
                            i = 0;
                            foreach (GameObject go in followerList)
                            {
                                float distanceToPlayer;
                                if (i == 0) //if follower is the first
                                {
                                    distanceToPlayer = Vector3.Distance(go.transform.position, transform.position);
                                    if (distanceToPlayer > (1f))
                                    {
                                        go.transform.position += go.transform.forward * Time.deltaTime * moveSpeed;
                                    }
                                }
                                else //follower should follow the previous follower
                                {
                                    distanceToPlayer = Vector3.Distance(go.transform.position, followerList[i-1].transform.position);
                                    if (distanceToPlayer > (0.75f))
                                    {
                                        go.transform.position += go.transform.forward * Time.deltaTime * moveSpeed;
                                    }
                                }
                                ++i;
                            }
                            
                        }
                        else
                        {
                            print("Close to the point.");
                            ++moveVectorIndex;
                            Vector3 vectorToLook = playerMoveVector[moveVectorIndex];
                            vectorToLook.y = transform.position.y; //y position of the looked object must be the same with the looker for lookAt function
                            transform.LookAt(vectorToLook);
                            if(moveVectorIndex == playerMoveVector.Count - 1)
                            {
                                Vector3 movement = transform.forward * Time.deltaTime * (moveSpeed);
                                navMeshAgent.Move(movement);
                            }
                            /*
                            if(!isMoving)
                            {
                                print("here");
                                Vector3 movement = transform.forward * Time.deltaTime * (moveSpeed * 10);
                                navMeshAgent.Move(movement);
                            }
                            */
                            //isMoving = false;
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
                    //isMoving = false;
                    GetComponent<Animator>().SetBool("Run", false);
                    
                    foreach (GameObject go in followerList)
                    {
                        go.GetComponent<Animator>().SetBool("Run", false);
                    }
                    
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
    #endregion

    #region Public Methods
    public void StartGamePhase()
    {
        Vector3[] tempVector = new Vector3[DrawManager.instance.lineRenderer.positionCount];
        DrawManager.instance.lineRenderer.GetPositions(tempVector);
        playerMoveVector = new List<Vector3>(tempVector);
        transform.LookAt(playerMoveVector[moveVectorIndex]);
        gamePhase = true;
        /*
        for(int i=0;i<playerMoveVector.Count-2;++i)
        {
            Debug.DrawLine(playerMoveVector[i], playerMoveVector[i + 1]);
        }
        */
    }

    public void AddFollower()
    {
        Vector3 behindPosition = transform.position - transform.forward * (followerList.Count + 1);
        GameObject go = Instantiate(followerPrefab, behindPosition, transform.rotation);
        //go.transform.position = transform.position;
        followerList.Add(go);
    }
    #endregion
}
