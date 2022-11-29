using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{

    Rigidbody rb;
    public GameObject playerObj;
    public GameObject currentRoom;
    public GameObject prevRoom;
    private RoomControl currentRoomScript;

    public GameObject[] allRooms;
    private RoomControl[] allRoomsScript;

    public Material matNorm;
    public Material matHidden;

    public GameObject moveButton;
    private cameracontrol cameracontroller;


    public GameObject[] myCopsObject;
    private CopController[] myCops;

    public GameObject scoreObject;
    public ScoreScript score;

    public GameObject particles;

    public bool isCaught = false;
    public bool isEscaped = false;
    public bool isHidden = false;
    private int hideTimer = 0;
    public bool isTurn = true;

    public bool isWearingShoes = false;
    public bool isWearingCloak = false;


    // Start is called before the first frame update
    void Start()
    {
        cameracontroller = moveButton.GetComponent<cameracontrol>();
        score = scoreObject.GetComponent<ScoreScript>();
        myCops = new CopController[myCopsObject.Length];
        for(int i = 0; i < myCopsObject.Length; i++)
        {
            myCops[i] = myCopsObject[i].GetComponent<CopController>();
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        float duration = Time.deltaTime * 2;
        Vector3 pos = playerObj.transform.position;
        Quaternion rot = playerObj.transform.rotation;

        Transform target = currentRoom.transform;
        Vector3 targetpos = target.position;
        targetpos.y = targetpos.y + 0.5f;

        playerObj.transform.position = Vector3.Lerp(pos, targetpos, duration);
        playerObj.transform.rotation = Quaternion.Lerp(rot, target.rotation, duration);
    }

    public void StartTurn()
    {
        //Check if all cops have ended
        int copsEnded = 0;
        for (int i = 0; i < myCops.Length; i++)
        {
            if (myCops[i].isTurn == false)
            {
                copsEnded += 1;
            }
        }
        if (copsEnded == myCops.Length) 
        {
            isTurn = true;
        }
        //Start Turn
        isWearingShoes = false;
        if (isTurn)
        {
            if (isHidden)
            {
                if (hideTimer == 1)
                {
                    Hide();

                }
                else
                {
                    hideTimer += 1;
                    if (!isWearingCloak)
                    {
                        EndTurn();
                    }
                }
            }
        }
    }

    public void EndTurn()
    {
        //Update Breadcrumbs
        allRoomsScript = new RoomControl[allRooms.Length];
        for (int i = 0; i < allRooms.Length; i++)
        {
            allRoomsScript[i] = allRooms[i].GetComponent<RoomControl>();
            allRoomsScript[i].UpdateBreadcrumbs();
        }

        if (!isWearingShoes)
        {
            for (int i = 0; i < myCops.Length; i++)
            {
                if (myCopsObject[i].activeSelf)
                {
                    isTurn = false;
                    myCops[i].isTurn = true;
                    myCops[i].StartTurn();
                }
                else
                {
                    if (myCops[i].taseTimer == 2)
                    {
                        myCopsObject[i].SetActive(true);
                    }
                    else
                    {
                        myCops[i].taseTimer += 1;
                    }
                }
            }
            if (isTurn) 
            {
                StartTurn();
            }
        }
        else 
        {
            StartTurn();
        }
    }

    public void MoveRoom(GameObject nextRoom)
    {
        if (!isCaught)
        {
            //Add Breadcrumbs to prev room
            currentRoomScript = currentRoom.GetComponent<RoomControl>();
            currentRoomScript.AddBreadcrumbs(50);
            //Turn off arrows and switch room
            cameracontroller.currentArrows.SetActive(false);
            prevRoom = currentRoom;
            currentRoom = nextRoom;
            //Add Breadcrumbs to new room
            currentRoomScript = currentRoom.GetComponent<RoomControl>();
            currentRoomScript.AddBreadcrumbs(20);
            //Change camera angle and add points
            cameracontroller.follow = !cameracontroller.follow;
            score.IncreaseDecrease(10);
            //End turn
            EndTurn();
        }

    }

    public void Hide()
    {
        if (isTurn)
        {
            if (isHidden)
            {
                StartCoroutine(UnHide());
            }
            else
            {
                if (isWearingCloak)
                {
                    particles.SetActive(true);
                    playerObj.GetComponent<Renderer>().material = matHidden;
                    isHidden = true;
                    hideTimer = 0;
                }
                else
                {
                    if (score.Score > 25)
                    {
                        particles.SetActive(true);
                        playerObj.GetComponent<Renderer>().material = matHidden;
                        isHidden = true;
                        hideTimer = 0;
                        score.IncreaseDecrease(-25);
                        EndTurn();
                    }
                }

            }
        }
    }

    IEnumerator UnHide()
    {
        yield return new WaitForSeconds(1f);
        particles.SetActive(false);
        playerObj.GetComponent<Renderer>().material = matNorm;
        isHidden = false;
        isWearingCloak = false;
        hideTimer = 0;
    }




 }
