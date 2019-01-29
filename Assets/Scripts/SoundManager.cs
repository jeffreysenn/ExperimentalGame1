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

    }

    private void OnDisable()
    {
        MovementController.OnMoved -= PlayMovementSFX;
    }
    
    private void PlaySFX(int i)
    {
        if (m_audioSources[i] != null)
            m_audioSources[i].Play();
    }
    void PlayMovementSFX()
    {
        PlaySFX(0);
    }

    void PlayRepairSFX()
    {
        PlaySFX(1);
    }

    void PlayLoosingSFX()
    {
        PlaySFX(2);
    }

    void PlayCrackedSFX()
    {
        PlaySFX(3);
    }

    void PlayFracturedSFX()
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
