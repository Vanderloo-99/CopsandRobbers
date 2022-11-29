using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    public string map;

    public int Score = 100;
    public TMP_Text scoreText;

    public GameObject updater;
    public TMP_Text scoreUpdateText;
    Color32 Red = new Color32(254, 9, 0, 255);
    Color32 Yellow = new Color32(212, 175, 55, 255);

    public GameObject finalObject;
    public GameObject newHigh;
    public GameObject finalScoreObj;
    public TMP_Text finalScore;

    public GameObject caughtTextObj;
    public TMP_Text caughtText;

    public GameObject highScoreObj1;
    public TMP_Text highScore1;

    public GameObject highScoreObj2;
    public TMP_Text highScore2;

    public GameObject highScoreObj3;
    public TMP_Text highScore3;


    public bool isCaught = false;
    public bool isEscaped = false;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<TMP_Text>();
        finalScore = finalScoreObj.GetComponent<TMP_Text>();

        highScore1 = highScoreObj1.GetComponent<TMP_Text>();
        highScore2 = highScoreObj2.GetComponent<TMP_Text>();
        highScore3 = highScoreObj3.GetComponent<TMP_Text>();

        scoreText.text = "Score: " + Score;
    }


    float elapsed = 0f;
    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= 1f)
        {
            elapsed = elapsed % 1f;
            if (!isCaught && !isEscaped)
            {
                IncreaseDecrease(-1);
            }
            else
            {
                if(finalObject.activeSelf == false)
                {
                    FinalScore();
                }
                    
            }
            
            scoreText.text = "Score: " + Score;
        }
        
    }

    public void IncreaseDecrease(int points) 
    {
        Score = Score + points;
        if(points > 0)
        {
            //Create
            GameObject instantiatedObject = Instantiate(updater);
            instantiatedObject.transform.SetParent(this.transform);
            instantiatedObject.transform.localPosition = new Vector3(120, 0, 0);
            instantiatedObject.transform.localScale = new Vector3(0.7f, 0.7f, 1);
            scoreUpdateText = instantiatedObject.GetComponent<TMP_Text>();
            scoreUpdateText.color = Yellow;
            scoreUpdateText.text = "+" + points;

            //Fade
            StartCoroutine(FadeText(2f, scoreUpdateText));

            //Destroy
            UnityEngine.Object.Destroy(instantiatedObject, 2.0f);
        }
        else 
        {
            //Create
            GameObject instantiatedObject = Instantiate(updater);
            instantiatedObject.transform.SetParent(this.transform);
            instantiatedObject.transform.localPosition = new Vector3(120, 0, 0);
            instantiatedObject.transform.localScale = new Vector3(0.7f, 0.7f, 1);
            scoreUpdateText = instantiatedObject.GetComponent<TMP_Text>();
            scoreUpdateText.color = Red;
            scoreUpdateText.text = points.ToString();

            //Fade
            StartCoroutine(FadeText(2f, scoreUpdateText));

            //Destroy
            UnityEngine.Object.Destroy(instantiatedObject, 2.0f);
        }

    }


    public void FinalScore() 
    {
        //Change the score
        finalScore.text = "Score: " + Score;
        DateTime today = DateTime.Today;
        string todayString = today.ToString("d");
        //Check if new high score
        if (Score > PlayerPrefs.GetInt("HighScore" + map + "3", 0))
        {
            newHigh.SetActive(true);
            //New HighScore -- Find if it beats 2nd best 
            if (Score > PlayerPrefs.GetInt("HighScore" + map + "2", 0))
            {
                //New HighScore -- Find if it beats 1st
                if (Score > PlayerPrefs.GetInt("HighScore" + map + "1", 0))
                {
                    //New HighScore
                    //Shift all down
                    PlayerPrefs.SetInt("HighScore" + map + "3", PlayerPrefs.GetInt("HighScore" + map + "2", 0));
                    PlayerPrefs.SetString("HighScore" + map + "3 Date", PlayerPrefs.GetString("HighScore" + map + "2 Date", ""));
                    PlayerPrefs.SetInt("HighScore" + map + "2", PlayerPrefs.GetInt("HighScore" + map + "1", 0));
                    PlayerPrefs.SetString("HighScore" + map + "2 Date", PlayerPrefs.GetString("HighScore" + map + "1 Date", ""));
                    //Set new
                    PlayerPrefs.SetInt("HighScore" + map + "1", Score);
                    PlayerPrefs.SetString("HighScore" + map + "1 Date", todayString);

                }
                else
                {
                    //Shift Down
                    PlayerPrefs.SetInt("HighScore" + map + "3", PlayerPrefs.GetInt("HighScore" + map + "2", 0));
                    PlayerPrefs.SetString("HighScore" + map + "3 Date", PlayerPrefs.GetString("HighScore" + map + "2 Date", ""));
                    //Set new
                    PlayerPrefs.SetInt("HighScore" + map + "2", Score);
                    PlayerPrefs.SetString("HighScore" + map + "2 Date", todayString);
                }
            }
            else
            {
                //Set new
                PlayerPrefs.SetInt("HighScore" + map + "3", Score);
                PlayerPrefs.SetString("HighScore" + map + "3 Date", todayString);
            }
        }


        //Check if escaped or caught
        if (isEscaped)
        {
            //Change text
            caughtText = caughtTextObj.GetComponent<TMP_Text>();
            caughtText.text = "Congrats! You Escaped";

        }



        //Display high scores
        highScore1.text = "1.  " + PlayerPrefs.GetString("HighScore" + map + "1 Date", "") + "   " + PlayerPrefs.GetInt("HighScore" + map + "1", 0).ToString();
        highScore2.text = "2. " + PlayerPrefs.GetString("HighScore" + map + "2 Date", "") + "   " + PlayerPrefs.GetInt("HighScore" + map + "2", 0).ToString();
        highScore3.text = "3. " + PlayerPrefs.GetString("HighScore" + map + "3 Date", "") + "   " + PlayerPrefs.GetInt("HighScore" + map + "3", 0).ToString();


        //Show the end screen
        finalObject.SetActive(true);
    }



    public IEnumerator FadeText(float t, TMP_Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }


}
