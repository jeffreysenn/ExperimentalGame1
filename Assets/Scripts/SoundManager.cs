using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource[] m_audioSources;

    private void OnEnable()
    {
        MovementController.OnMoved += PlayMovementSFX;
        FixPillar.OnRepaired += PlayRepairSFX;
        PillarManager.OnGameOver += PlayLoosingSFX;
        Pillar.OnDestroyed += PlayCrackedSFX;
        Pillar.OnRebuilt += PlayRebuiltSFX;
    }

    private void OnDisable()
    {
        MovementController.OnMoved -= PlayMovementSFX;
        FixPillar.OnRepaired -= PlayRepairSFX;
        PillarManager.OnGameOver -= PlayLoosingSFX;
        Pillar.OnRebuilt -= PlayRebuiltSFX;
    }

    private void PlaySFX(int i)
    {
        if(i > m_audioSources.Length) { return; }
        if (m_audioSources[i] != null)
        {
            if (i == 3 && m_audioSources[3] != null)
                Debug.Log("HENLO");
            m_audioSources[i].Play();
        }
    }
    void PlayMovementSFX(int i)
    {
        PlaySFX(i);
    }

    void PlayRepairSFX(Pillar pillar)
    {
        PlaySFX(1);
    }

    void PlayLoosingSFX()
    {
        PlaySFX(2);
    }

    void PlayCrackedSFX()
    {
        //PlaySFX(3);
    }

    void PlayRebuiltSFX()
    {
        PlaySFX(4);
    }

    void PlayWreckedSFX()
    {
        PlaySFX(5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
