using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PillarManager : MonoBehaviour
{
    public Pillar[] m_pillars;

    public float m_activationFrequency;
    private float m_activationTime;

    void Start()
    {
        
    }

    void Update()
    {
        m_activationTime += Time.deltaTime;
        if (m_activationTime >= m_activationFrequency)
        {
            m_activationTime = 0;
            CrackRandomPillar();
        }
    }

    void CrackRandomPillar()
    {
        if (m_pillars.All(m_pillar => m_pillar.IsDestructing))
            return;


        List<Pillar> m_intactPillars = new List<Pillar>();

        foreach (Pillar m_pillar in m_pillars)
        {
            if (!m_pillar.IsDestructing)
            {
                m_intactPillars.Add(m_pillar);
            }
        }
        int m_random = Random.Range(0, m_intactPillars.Count);

        m_intactPillars[m_random].TriggerDestruction();
    }
}
