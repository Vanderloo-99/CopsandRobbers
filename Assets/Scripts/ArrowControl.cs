using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowControl : MonoBehaviour
{ 
    public GameObject currentRoom;
    public GameObject nextRoom;
    public GameObject myPlayerObject;
    public GameObject arrow;
    private player myPlayer;



    // Start is called before the first frame update
    void Start()
    {
        myPlayer = myPlayerObject.GetComponent<player>();
    }


    // Update is called once per frame
    void Update()
    {
        //Check for mouse click 
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, 1000f))
            {
                if (raycastHit.transform != null)
                {
                    CurrentClickedGameObject(raycastHit.transform.gameObject);
                }
            }
        }
    }

    public void CurrentClickedGameObject(GameObject gameObjectClicked)
    {
        if (gameObjectClicked == arrow)
        {
            myPlayer.MoveRoom(nextRoom);
        }
    }

}
