using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public delegate void ScoreHandler();
    public static event ScoreHandler OnAddedLife;
    public GameObject UIManagerObj;
    private int score = 0;
    private UIManager UIManager;
    [SerializeField] int addLifeScore = 10;
    private int lastScoreWhenAddedLife = 0;

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
    
    void AddScore(Pillar pillar)
    {
        if(GameStateManager.gameState != GameState.Clear)
        score+= pillar.point;
        UIManager.UpdateScore(score);
        if(score - lastScoreWhenAddedLife >= addLifeScore) { AddLife(); }
    }

    public void AddLife()
    {
        if(OnAddedLife != null) { OnAddedLife(); }
        PillarManager.pillarsDestroyed = 0;
        lastScoreWhenAddedLife += addLifeScore;
    }

    public void ResetScore()
    {
        score = 0;
        UIManager.UpdateScore(score);

    }
}
