using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemFunctionalities : MonoBehaviour
{
    public GameObject playerObj;
    private player playerScript;

    public GameObject itemScreenObj;
    private ItemControl itemScreen;


    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.Find("playerSphere");
        playerScript = playerObj.GetComponent<player>();
        itemScreenObj = GameObject.Find("Item Screen");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Antique()
    {
        //Increase score by 50, 100, 150, 200 or 250
        playerScript.score.IncreaseDecrease((UnityEngine.Random.Range(1, 6) * 50));
        //Delete Item
        UnityEngine.Object.Destroy(this.gameObject);

    }

    public void Broom()
    {
        //Get Current Room
        GameObject currentRoom = playerScript.currentRoom;
        RoomControl currentRoomScript = currentRoom.GetComponent<RoomControl>();
        //Remove All Crumbs
        currentRoomScript.RemoveBreadcrumbs();
        //Delete Item
        UnityEngine.Object.Destroy(this.gameObject);
    }

    public void Decoy()
    {
        //Get Adj Rooms
        GameObject currentRoom = playerScript.currentRoom;
        GameObject prevRoom = playerScript.prevRoom;
        RoomControl prevRoomScript = prevRoom.GetComponent<RoomControl>();
        List<GameObject> adjRooms = prevRoomScript.adjRooms.ToList();
        //Remove current room from the list
        adjRooms.Remove(currentRoom);
        //Add breadcrumbs to all rooms in list
        for (int i = 0; i < adjRooms.Count; i++)
        {
            RoomControl adjRoomScript = adjRooms[i].GetComponent<RoomControl>();
            adjRoomScript.AddBreadcrumbs(150);
        }
        //Delete Item
        UnityEngine.Object.Destroy(this.gameObject);
    }

    public void InvisCloak()
    {
        //Set bool to true, will not end the turn
        playerScript.isWearingCloak = true;
        //Hide
        playerScript.Hide();
        //Delete Item
        UnityEngine.Object.Destroy(this.gameObject);
    }

    public void Key()
    {
        //Close item screen
        ItemControl itemScreen = itemScreenObj.GetComponent<ItemControl>();
        itemScreen.InventoryClick();


        //Get current room
        GameObject currentRoom = playerScript.currentRoom;
        RoomControl currentRoomScript = currentRoom.GetComponent<RoomControl>();
        if (currentRoomScript.hasExitDoor)
        {
            //Delete Item
            UnityEngine.Object.Destroy(this.gameObject);
            //Add Score
            playerScript.score.IncreaseDecrease(500);
            //Show Winning end screen
            //Set to escaped
            playerScript.isEscaped = true;
            playerScript.score.isEscaped = true;
            //Get Cops
            List<GameObject> myCopsObject = playerScript.myCopsObject.ToList();
            for (int i = 0; i < myCopsObject.Count; i++)
            {
                CopController myCopScript = myCopsObject[i].GetComponent<CopController>();
                //Set escaped
                myCopScript.isEscaped = true;
            }
            
            
        }
        
    }

    public void Shoes()
    {
        //Set bool to true, will not trigger cops to move for a turn
        playerScript.isWearingShoes = true;
        //Delete Item
        UnityEngine.Object.Destroy(this.gameObject);
    }

    public void Taser()
    {
        //Get Cops
        List<GameObject> myCopsObject = playerScript.myCopsObject.ToList();
        //Check if active and remove non-active
        for (int i = 0; i < myCopsObject.Count; i++)
        {
            if (!myCopsObject[i].activeSelf)
            {
                myCopsObject.RemoveAt(i);
            }
        }
        //Choose random from active
        int index = UnityEngine.Random.Range(0, myCopsObject.Count);
        //Tase the cop
        CopController myCopScript = myCopsObject[index].GetComponent<CopController>();
        myCopScript.Tase();
        
        //Delete Item
        UnityEngine.Object.Destroy(this.gameObject);
    }
}
