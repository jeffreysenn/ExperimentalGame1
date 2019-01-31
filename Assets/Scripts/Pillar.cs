using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PillarStates
{
    Intact,
    Cracked,
    Fractured,
    Wrecked,
    Destroyed,
    Count
}

public class Pillar : MonoBehaviour
{
    public int point = 1;
    [HideInInspector]
    public PillarStates m_state;

    public SpriteRenderer m_spriteRenderer;
    public Sprite[] m_sprites;

    public bool IsDestructing { get { return (m_state > 0); } }

    public int row;

    public PillarManager pillarManager;

    private float m_destructDuration = 2.5f;
    private float m_destructTime;

    public AudioSource[] m_audioSources;

    public delegate void PillarHandler();
    public static event PillarHandler OnDestroyed;
    public static event PillarHandler OnRebuilt;

    private bool isFrozen = false;

    void OnEnable()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        ResetDestructTime();

    }

    public void SetPillarFrozen(bool setFrozen)
    {
        isFrozen = setFrozen;
    }

    void Update()
    {
        if (isFrozen) { return; }
        if (GameStateManager.gameState == GameState.Failed)
            return;
        if (IsDestructing && m_state != PillarStates.Destroyed)
        {
            m_destructTime -= Time.deltaTime;
            if (m_destructTime <= 0)
            {
                NextState();

                m_destructTime = m_destructDuration;
            }
        }

        /*
        if (m_spriteRenderer != null)
            m_spriteRenderer.color = new Color(
                m_spriteRenderer.color.r,
                m_spriteRenderer.color.g,
                m_spriteRenderer.color.b,
                1 - ((float)m_state/((float)PillarStates.Count-1)));
        */
        //Debug.Log(((1/5)));
    }

    public void TriggerDestruction()
    {
        NextState();
    }

    private void NextState()
    {
        m_state = m_state + 1;

        if (m_state != PillarStates.Destroyed)
            m_spriteRenderer.sprite = m_sprites[(int)m_state];
        else
        {
            m_spriteRenderer.color = Color.clear;
            PillarManager.pillarsDestroyed++;
            pillarManager.ReportPillarDestruction(row);
            if(OnDestroyed != null)
            {
                OnDestroyed();
            }
        }

        if (m_audioSources.Length > (int)m_state - 1 && m_audioSources[(int)m_state - 1] != null)
            m_audioSources[(int)m_state - 1].Play();
    }

    public void Repair()
    {
        if (m_state == PillarStates.Destroyed)
            OnRebuilt();
        m_state = PillarStates.Intact;
        m_spriteRenderer.sprite = m_sprites[0];
        m_spriteRenderer.color = Color.white;
    }

    public void ResetDestructTime()
    {
        m_destructTime = m_destructDuration;
    }
}
