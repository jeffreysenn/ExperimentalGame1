using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public ScoreManager scoreManager;
    public Timer timer;

    public GameObject[] playerObjs;
    private CharacterInfo[] characterInfos;
    private int currentSpriteIndex;
    private bool shouldStartGame = false;

    void SetCharacterInfos()
    {
        playerObjs = GameObject.FindGameObjectsWithTag("Player");
        characterInfos = new CharacterInfo[playerObjs.Length];
        for(int i = 0; i < playerObjs.Length; i++)
        {
            characterInfos[i] = playerObjs[i].GetComponent<CharacterInfo>();
        }
    }

    void ResetGame()
    {
        for (int i = 0; i < characterInfos.Length; i++)
        {
            characterInfos[i].IsActive = false;
            if(i == 0) { characterInfos[i].IsActive = true; }
            if (characterInfos[i].IsActive) { currentSpriteIndex = i; }
            characterInfos[i].gameObject.GetComponent<Renderer>().enabled = characterInfos[i].IsActive;
        }
        scoreManager.ResetScore();
        timer.ResetTimer();
        shouldStartGame = true;
    }

    void ClearScreen()
    {
        for (int i = 0; i < characterInfos.Length; i++)
        {
            characterInfos[i].IsActive = false;
            characterInfos[i].gameObject.GetComponent<Renderer>().enabled = characterInfos[i].IsActive;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SetCharacterInfos();
        ClearScreen();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            ResetGame();
        }

        if (shouldStartGame)
        {
            if (Input.GetButtonDown("Right"))
            {
                Vector2 NextNumAndRoll = characterInfos[currentSpriteIndex].NumAndRoll + new Vector2(1, 0);
                ShowNextSprite(NextNumAndRoll);
            }

            if (Input.GetButtonDown("Left"))
            {
                Vector2 NextNumAndRoll = characterInfos[currentSpriteIndex].NumAndRoll + new Vector2(-1, 0);
                ShowNextSprite(NextNumAndRoll);
            }

            if (Input.GetButtonDown("Up"))
            {
                if (LadderCheckAbove())
                {
                    Vector2 NextNumAndRoll = characterInfos[currentSpriteIndex].LadderAbove.GetComponent<CharacterInfo>().NumAndRoll;
                    ShowNextSprite(NextNumAndRoll);

                }
            }

            if (Input.GetButtonDown("Down"))
            {
                if (LadderCheckBelow())
                {
                    Vector2 NextNumAndRoll = characterInfos[currentSpriteIndex].LadderBelow.GetComponent<CharacterInfo>().NumAndRoll;
                    ShowNextSprite(NextNumAndRoll);

                }
            }
        }

    }

    private bool LadderCheckAbove()
    {
        if (characterInfos[currentSpriteIndex].LadderAbove == null) { return false; }
        return true;
    }

    private bool LadderCheckBelow()
    {
        if (characterInfos[currentSpriteIndex].LadderBelow == null) { return false; }
        return true;
    }

    private void ShowNextSprite(Vector2 NextNumAndRoll)
    {
        for (int i = 0; i < characterInfos.Length; i++)
        {
            if (characterInfos[i].NumAndRoll == NextNumAndRoll)
            {
                characterInfos[currentSpriteIndex].IsActive = false;
                characterInfos[i].IsActive = true;
                UpdateVisibility(i);
                currentSpriteIndex = i;
                break;
            }
        }
    }

    void UpdateVisibility(int newVisibleNum)
    {
        characterInfos[currentSpriteIndex].GetComponent<Renderer>().enabled = false;
        characterInfos[newVisibleNum].GetComponent<Renderer>().enabled = true;

    }

    public CharacterInfo GetCurrentSprite() { return characterInfos[currentSpriteIndex]; }
}
