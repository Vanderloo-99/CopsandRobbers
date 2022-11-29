using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControl : MonoBehaviour
{


    public GameObject[] adjRooms;
    public double breadcrumbs = 0;
    public bool isRummaged = false;
    public int rummageTimer = 0;

    public bool hasExitDoor;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateBreadcrumbs()
    {
        // Update the breadcrumbs
        double evapRate = 0.3;
        breadcrumbs = (1 - evapRate) * breadcrumbs;
        if(breadcrumbs < 10)
        {
            breadcrumbs = 0;
        }
        //Update rummage
        if (isRummaged)
        {
            rummageTimer += 1;
            if (rummageTimer == 4)
            {
                rummageTimer = 0;
                isRummaged = false;
            }
        }

    }

    // Add breadcrumbs
    public void AddBreadcrumbs(double num)
    {
        breadcrumbs += num;
    }

    public double GetBreadcrumbs()
    {
        return breadcrumbs;
    }

    //Remove breadcrumbs
    public void RemoveBreadcrumbs()
    {
        breadcrumbs = 0;
    }
}
