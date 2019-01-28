using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPillar : MonoBehaviour
{

    public delegate void ScoreHandler();
    public static event ScoreHandler OnRepaired;

    private Pillar pillar;
    // Start is called before the first frame update
    void Start()
    {
        pillar = GetComponent<MovementController>().GetCurrentSprite().pillar;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Action1"))
        {
            pillar = GetComponent<MovementController>().GetCurrentSprite().pillar;
            if (!pillar) { return; }
            pillar.Repair();
            if (OnRepaired != null)
            {
                OnRepaired();
            }
        }
    }
}
