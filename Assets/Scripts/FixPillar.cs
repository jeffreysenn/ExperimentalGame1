using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPillar : MonoBehaviour
{

    public delegate void ScoreHandler();
    public static event ScoreHandler OnRepaired;

    public GameObject pillarManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(pillarManager == null) { return; }
        if (Input.GetButtonDown("Action1"))
        {
            Pillar[] allPillars = pillarManager.GetComponent<PillarManager>().m_pillars;
            foreach(Pillar pillar in allPillars)
            {
                if(pillar.pillarNumRow == GetComponent<MovementController>().GetCurrentSprite().PillarNumRoll)
                {
                    pillar.Repair();
                    if(OnRepaired != null)
                    {
                        OnRepaired();
                    }
                }
            }
        }
    }
}
