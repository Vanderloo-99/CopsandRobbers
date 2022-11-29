using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CopController : MonoBehaviour
{
    public GameObject myPlayerObject;
    private player myPlayer;

    public GameObject currentRoom;
    public GameObject prevRoom;
    private RoomControl myRoom;

    public GameObject scoreObject;
    private ScoreScript score;

    public bool isCaught = false;
    public bool isEscaped = false;
    public bool isTurn = false;

    public int difficulty = 0;

    public bool isTased = false;
    public int taseTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        difficulty = StateController.difficulty;
        myPlayer = myPlayerObject.GetComponent<player>();
        myRoom = currentRoom.GetComponent<RoomControl>();
        score = scoreObject.GetComponent<ScoreScript>();

        Physics.IgnoreCollision(myPlayerObject.GetComponent<Collider>(), GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
        //Check if caught
        if (currentRoom == myPlayer.currentRoom && !myPlayer.isHidden)
        {
            isCaught = true;
            myPlayer.isCaught = true;
            score.isCaught = true;
        }

        //Move to correct room
        float duration = Time.deltaTime * 2;
        Vector3 pos = this.transform.position;
        Quaternion rot = this.transform.rotation;

        Transform target = currentRoom.transform;
        Vector3 targetpos = target.position;
        targetpos.y = targetpos.y + 0.5f;

        this.transform.position = Vector3.Lerp(pos, targetpos, duration);
        this.transform.rotation = Quaternion.Lerp(rot, target.rotation, duration);
    }

    public void StartTurn()
    {
        //Check if caught
        if (currentRoom == myPlayer.currentRoom && !myPlayer.isHidden)
        {
            isCaught = true;
            myPlayer.isCaught = true;
            score.isCaught = true;
        }
        if (!isCaught && !isEscaped)
        {
            StartCoroutine(CompleteTurn());
        } 
    }

    IEnumerator CompleteTurn()
    {
        yield return new WaitForSeconds(1f);
        Move();
        EndTurn();
    }

    public void EndTurn()
    {
        isTurn = false;
        myPlayer.StartTurn();
    }


    public void Move() 
    {
        myRoom = currentRoom.GetComponent<RoomControl>();
        GameObject chosenRoom = GetChoice();
        prevRoom = currentRoom;
        currentRoom = chosenRoom;
    }

    public GameObject GetChoice()
    {
        if(difficulty == 2) 
        {
            //Hard Algorithm
            // Check if player is next to cop
            int chosen = 0;
            List<GameObject> adjRooms = myRoom.adjRooms.ToList();
            for(int i = 0; i < adjRooms.Count; i++) 
            {
                if(myPlayer.currentRoom == adjRooms[i]) 
                {
                    return adjRooms[i];
                }
            }
            //Otherwise track with breadcrumbs
            int roll = UnityEngine.Random.Range(0, 100);
            if (roll > 90)
            {
                //10% of time Random Room
                int numOfAdjRooms = adjRooms.Count;
                chosen = Random.Range(0, numOfAdjRooms);
            }
            else
            {
                //90% of time Follow Breadcrumbs
                double breadcrumbs = 0;
                for (int i = 0; i < adjRooms.Count; i++)
                {
                    RoomControl room = adjRooms[i].GetComponent<RoomControl>();
                    if (room.GetBreadcrumbs() > breadcrumbs)
                    {
                        breadcrumbs = room.GetBreadcrumbs();
                        chosen = i;
                    }
                }
            }

            return adjRooms[chosen];

        }
        else if(difficulty == 1) 
        {
            //Medium Algorithm - Breadcrumbs
            List<GameObject> adjRooms = myRoom.adjRooms.ToList();
            int roll = UnityEngine.Random.Range(0, 100);
            int chosen = 0;
            if (roll > 65)
            {
                //35% of time Random Room
                int numOfAdjRooms = adjRooms.Count;
                chosen = Random.Range(0, numOfAdjRooms);
            }
            else
            {
                //64% of time Follow Breadcrumbs
                double breadcrumbs = 0;
                for (int i = 0; i < adjRooms.Count; i++)
                {
                    RoomControl room = adjRooms[i].GetComponent<RoomControl>();
                    if (room.GetBreadcrumbs() > breadcrumbs)
                    {
                        breadcrumbs = room.GetBreadcrumbs();
                        chosen = i;
                    }
                }
            }
            
            return adjRooms[chosen];
        }
        else
        {
            //Easy Algorithm - Semi Random     
            List<GameObject> adjRoomsNotPrev = myRoom.adjRooms.ToList();
            //If there is a dead end then go backwards
            if (adjRoomsNotPrev.Count == 1) 
            {
                return adjRoomsNotPrev[0];
            }
            adjRoomsNotPrev.Remove(prevRoom);
            int numOfAdjRoomsNotPrev = adjRoomsNotPrev.Count;
            //Choose a random room
            int chosen = Random.Range(0, numOfAdjRoomsNotPrev);
            return adjRoomsNotPrev[chosen];
        }
    }

    public void Tase() 
    {
        isTased = true;
        this.gameObject.SetActive(false);
    }

    public void Untase()
    {
        isTased = false;
        this.gameObject.SetActive(true);
    }

}
