using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public Text scoreDisplay;
    public Text timeDisplay;

    public void UpdateScore(int score)
    {
        scoreDisplay.text = "Score: " + score.ToString();
    }

    public void UpdateTime(int time)
    {
        timeDisplay.text = "Time: " + time.ToString();
    }
}
