using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector]
    public static PlayerManager instance = null;
    [HideInInspector]
    public List<GameObject> followerList;
    [HideInInspector]
    public bool gamePhase = false;
    public float moveSpeed;
    public GameObject followerPrefab;

    private int moveVectorIndex = 0;
    private NavMeshAgent navMeshAgent;
    private float touchTime;
    private List<Vector3> playerMoveVector;
    

    

    #region Unity Methods
    private void Awake()
    {
        instance = GetComponent<PlayerManager>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        followerList = new List<GameObject>();
        playerMoveVector = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gamePhase)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    touchTime = Time.time;
                }

                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                {
                    if(moveVectorIndex < playerMoveVector.Count)
                    {
                        float distance = Vector3.Distance(playerMoveVector[moveVectorIndex], transform.position);
                        string animationString;
                        if(Time.time - touchTime >= 2f)
                        {
                            animationString = "Run";
                        }
                        else
                        {
                            animationString = "Walk";
                        }

                        GetComponent<Animator>().SetBool(animationString, true);
                        int i = 0;
                        foreach(GameObject go in followerList)
                        {
                            go.GetComponent<Animator>().SetBool(animationString, true);
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
                        if (distance > 0.7f) //haven't reached next point
                        {
                            Vector3 movement;
                            if(animationString.Equals("Run"))
                            {
                                movement = transform.forward * Time.deltaTime * (moveSpeed + 3);
                            }
                            else
                            {
                                movement = transform.forward * Time.deltaTime * moveSpeed;
                            }
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
                                        go.transform.position += go.transform.forward * Time.deltaTime * (moveSpeed + 3);
                                    }
                                }
                                else //follower should follow the previous follower
                                {
                                    distanceToPlayer = Vector3.Distance(go.transform.position, followerList[i-1].transform.position);
                                    if (distanceToPlayer > (0.75f))
                                    {
                                        go.transform.position += go.transform.forward * Time.deltaTime * (moveSpeed + 3);
                                    }
                                }
                                ++i;
                            }
                            
                        }
                        else
                        {
                            ++moveVectorIndex;
                            Vector3 vectorToLook = playerMoveVector[moveVectorIndex];
                            vectorToLook.y = transform.position.y; //y position of the looked object must be the same with the looker for lookAt function
                            transform.LookAt(vectorToLook);
                            if(moveVectorIndex == playerMoveVector.Count - 1)
                            {
                                Vector3 movement = transform.forward * Time.deltaTime * (moveSpeed);
                                navMeshAgent.Move(movement);
                            }
                        }
                        
                    }
                    
                }
                if(touch.phase == TouchPhase.Ended)
                {
                    GetComponent<Animator>().SetBool("Run", false);
                    GetComponent<Animator>().SetBool("Walk", false);

                    foreach (GameObject go in followerList)
                    {
                        go.GetComponent<Animator>().SetBool("Run", false);
                        go.GetComponent<Animator>().SetBool("Walk", false);
                    }
                    
                }
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
    }

    public void AddFollower()
    {
        Vector3 behindPosition = transform.position - transform.forward * (followerList.Count + 1);
        GameObject go = Instantiate(followerPrefab, behindPosition, transform.rotation);
        followerList.Add(go);
    }
    #endregion
}
