using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPillar : MonoBehaviour
{

    public delegate void ScoreHandler(Pillar pillar);
    public static event ScoreHandler OnRepaired;

    private Pillar pillar;
    public float fixFrozenTime = .5f;
    private float frozenTimer;
    private bool shouldStartTimer = false;

    private void Start()
    {
        frozenTimer = fixFrozenTime;
    }
    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.gameState == GameState.Failed)
            return;
        if (Input.GetButtonDown("Action1"))
        {
            pillar = GetComponent<MovementController>().GetCurrentSprite().pillar;
            if (!pillar) { return; }
            if(pillar.m_state == PillarStates.Intact || pillar.m_state == PillarStates.Destroyed) { return; }
            pillar.SetPillarFrozen(true);
            shouldStartTimer = true;
            pillar.m_state = PillarStates.Intact;
            if (OnRepaired != null)
            {
                OnRepaired(pillar);
            }
            GetComponent<MovementController>().FreezeForSeconds(fixFrozenTime);
        }

        if (shouldStartTimer) { frozenTimer -= Time.deltaTime; }
        if (frozenTimer < 0)
        {
            pillar.Repair();
            pillar.SetPillarFrozen(false);

            shouldStartTimer = false;
            frozenTimer = fixFrozenTime;
        }
    }
}
