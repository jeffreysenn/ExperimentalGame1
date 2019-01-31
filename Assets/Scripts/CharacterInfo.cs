using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public Vector2 numAndRoll;
    public bool isActive;
    public Pillar pillar;
    public CharacterInfo ladderAbove, ladderBelow;

    private void OnEnable()
    {
        FixPillar.OnRepaired += FixAnimation;
    }

    private void OnDisable()
    {
        FixPillar.OnRepaired -= FixAnimation;
    }

    public void FixAnimation(Pillar pillar)
    {
        GetComponent<Animator>().SetTrigger("Fix");
    }

}
