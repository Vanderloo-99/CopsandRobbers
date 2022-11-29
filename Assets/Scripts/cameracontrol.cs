using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameracontrol : MonoBehaviour
{
    public GameObject cam;
    public GameObject camFollowPos;
    public GameObject camOverheadPos;
    private Transform target;
    public bool follow = true;
    

    public GameObject myPlayerObject;
    private player myPlayer;
    public GameObject currentArrows;
   

    private void Start()
    {
        myPlayer = myPlayerObject.GetComponent<player>();
        
    }

    public void Switch()
    {
        if (myPlayer.isTurn)
        {
            follow = !follow;
        }
    }


    private void Update()
    {
        if (!myPlayer.isTurn) 
        {
            follow = true;
        }
        try
        {
            currentArrows = myPlayer.currentRoom.transform.Find("Arrows").gameObject;
            currentArrows.SetActive(!follow);
        }
        catch { }
        float duration = Time.deltaTime * 3;
        Vector3 pos = cam.transform.position;
        Quaternion rot = cam.transform.rotation;
        if (follow)
        {
            target = camFollowPos.transform;
        }
        else 
        { 
            target = camOverheadPos.transform;
        }
        cam.transform.position = Vector3.Lerp(pos, target.position, duration);
        cam.transform.rotation = Quaternion.Lerp(rot, target.rotation, duration);
    }
}
