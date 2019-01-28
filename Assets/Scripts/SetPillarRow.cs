using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPillarRow : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        if (GetComponent<CharacterInfo>().pillar)
        {
            GetComponent<CharacterInfo>().pillar.row = (int)GetComponent<CharacterInfo>().numAndRoll.y;
        }
    }
}
