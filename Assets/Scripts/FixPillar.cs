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
        pillar = GetComponent<CharacterInfo>().pillar;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pillar) { return; }
        if (Input.GetButtonDown("Action1"))
        {
            pillar.Repair();
            if (OnRepaired != null)
            {
                OnRepaired();
            }
        }
    }
}
