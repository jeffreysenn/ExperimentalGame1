using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpriteInfo
{
    public GameObject Sprite;
    public Vector2 NumAndRoll;
    public bool IsActive;
    public Vector2 PillarNumRoll;
}

public class MovementController : MonoBehaviour
{
    public SpriteInfo[] sprites;
    private int currentSpriteIndex;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].IsActive) { currentSpriteIndex = i; }
            sprites[i].Sprite.GetComponent<Renderer>().enabled = sprites[i].IsActive;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Right"))
        {
            Vector2 NextNumAndRoll = sprites[currentSpriteIndex].NumAndRoll + new Vector2(1, 0);
            ShowNextSprite(NextNumAndRoll);
        }

        if (Input.GetButtonDown("Left"))
        {
            Vector2 NextNumAndRoll = sprites[currentSpriteIndex].NumAndRoll + new Vector2(-1, 0);
            ShowNextSprite(NextNumAndRoll);
        }

        if (Input.GetButtonDown("Up"))
        {
            Vector2 NextNumAndRoll = sprites[currentSpriteIndex].NumAndRoll + new Vector2(0, 1);
            ShowNextSprite(NextNumAndRoll);
        }

        if (Input.GetButtonDown("Down"))
        {
            Vector2 NextNumAndRoll = sprites[currentSpriteIndex].NumAndRoll + new Vector2(0, -1);
            ShowNextSprite(NextNumAndRoll);
        }

    }

    private void ShowNextSprite(Vector2 NextNumAndRoll)
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            if (sprites[i].NumAndRoll == NextNumAndRoll)
            {
                sprites[currentSpriteIndex].IsActive = false;
                sprites[i].IsActive = true;
                UpdateVisibility(i);
                currentSpriteIndex = i;
                break;
            }
        }
    }

    void UpdateVisibility(int newVisibleNum)
    {
        sprites[currentSpriteIndex].Sprite.GetComponent<Renderer>().enabled = false;
        sprites[newVisibleNum].Sprite.GetComponent<Renderer>().enabled = true;

    }

    public SpriteInfo GetCurrentSprite() { return sprites[currentSpriteIndex]; }
}
