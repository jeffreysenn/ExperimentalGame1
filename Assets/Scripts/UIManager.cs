using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public Text scoreDisplay;
    public Text timeDisplay;
    public Transform m_playerLives;

    private void OnEnable()
    {
        Pillar.OnDestroyed += UpdateLives;
        ScoreManager.OnAddedLife += UpdateLives;
        FireController.OnFireHit += UpdateLives;

    }

    private void OnDisable()
    {
        Pillar.OnDestroyed -= UpdateLives;
        ScoreManager.OnAddedLife -= UpdateLives;
        FireController.OnFireHit -= UpdateLives;


    }

    public int GetPlayerLife()
    {
        return GameStateManager.m_maxDestroyedPillar - PillarManager.pillarsDestroyed;
    }

    private void UpdateLives()
    {
        UpdateLives(GetPlayerLife());
    }

    public void UpdateScore(int score)
    {
        scoreDisplay.text = "Score: " + score.ToString();
    }

    public void UpdateSystemTime()
    {
        scoreDisplay.text = System.DateTime.Now.ToShortTimeString();
    }

    public void UpdateTime(int time)
    {
        timeDisplay.text = "Time: " + time.ToString();
    }

    public void UpdateLives(int p_lives)
    {
        if (m_playerLives == null) { return; }

        Image[] m_images = m_playerLives.GetComponentsInChildren<Image>();

        for (int i = 0; i < m_images.Length; i++)
            m_images[i].color = Color.clear;
        for (int i = 0; i < p_lives; i++)
            m_images[i].color = Color.white;
    }
}
