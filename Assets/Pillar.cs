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
    public PillarStates m_state;

    public bool IsDestructing
    {
        get { return (m_state > 0); }
    }

    private float m_destructDuration = 2f;
    private float m_destructTime;

    void Start()
    {
        ResetDestructTime();
    }

    void Update()
    {
        if (IsDestructing && m_state != PillarStates.Destroyed)
        {
            m_destructTime -= Time.deltaTime;
            if (m_destructTime <= 0)
            {
                m_state = m_state + 1;
                Debug.Log(m_state);
                m_destructTime = m_destructDuration;
            }
        }

        if (GetComponent<SpriteRenderer>() != null)
            GetComponent<SpriteRenderer>().color = new Color(
                GetComponent<SpriteRenderer>().color.r,
                GetComponent<SpriteRenderer>().color.g,
                GetComponent<SpriteRenderer>().color.b,
                1 - ((float)m_state/((float)PillarStates.Count-1)));
        //Debug.Log(((1/5)));
    }

    public void TriggerDestruction()
    {
        m_state = PillarStates.Cracked;
    }

    public void Repair()
    {
        m_state = PillarStates.Intact;
    }

    public void ResetDestructTime()
    {
        m_destructTime = m_destructDuration;
    }
}
