using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    public int difficulty = 0;
    public GameObject info;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayGame(int map)
    {
        StateController.difficulty = difficulty;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + map);
    }

    public void HandleDropDown(int val)
    {
        difficulty = val;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }

    public void InfoButton() 
    {
        if (info.activeSelf)
        {
            info.SetActive(false);
        }
        else
        {
            info.SetActive(true);
        }
    }

}
