using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemControl : MonoBehaviour
{
    public GameObject itemScreen;

    public GameObject itemPrefab;
    public GameObject itemGrid;

    public GameObject playerObj;
    private player playerScript;

    public GameObject[] prefabList;
    public GameObject scoreItemPrefab;
    public GameObject keyPrefab;

    public bool hasKey = false;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = playerObj.GetComponent<player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InventoryClick()
    {
        playerScript = playerObj.GetComponent<player>();
        
        if (itemScreen.activeSelf)
        {
            itemScreen.SetActive(false);
        }
        else
        {
            itemScreen.SetActive(true);
        }
        
    }

    public void Rummage()
    {
        playerScript = playerObj.GetComponent<player>();
        RoomControl currentRoom = playerScript.currentRoom.GetComponent<RoomControl>();
        if (playerScript.isTurn && !currentRoom.isRummaged)
        {
            //Add Object
            //65% Antique 35% Utility
            GameObject instantiatedObject;
            int roll = UnityEngine.Random.Range(1, 101);
            if(roll > 60) 
            {
                if (roll > 80 && !hasKey)
                {
                    instantiatedObject = Instantiate(keyPrefab);
                    hasKey = true;
                }
                else {
                    int prefabIndex = UnityEngine.Random.Range(0, prefabList.Length);
                    instantiatedObject = Instantiate(prefabList[prefabIndex]);
                }
            }
            else 
            {
                instantiatedObject = Instantiate(scoreItemPrefab);
            }

            
            instantiatedObject.transform.SetParent(itemGrid.transform);
            instantiatedObject.transform.localScale = new Vector3(1, 1, 1);
            //Increase Score
            playerScript.score.IncreaseDecrease(20);
            //End Turn
            currentRoom.isRummaged = true;
        }
    }
}
