using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public GameObject UIManagerObj;
    private int score = 0;
    private UIManager UIManager;

    void OnEnable()
    {
        FixPillar.OnRepaired += AddScore;
        UIManager = UIManagerObj.GetComponent<UIManager>();
        if(UIManager == null) { return; }
        UIManager.UpdateScore(score);
    }

    void OnDisable()
    {
        FixPillar.OnRepaired -= AddScore;
    }
    
    void AddScore()
    {
        score++;
        UIManager.UpdateScore(score);
    }

    public void ResetScore()
    {
        score = 0;
        UIManager.UpdateScore(score);

    }
}
