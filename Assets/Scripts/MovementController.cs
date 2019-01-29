using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public delegate void MovementHandler();
    public static event MovementHandler OnMoved;

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
        bool hasActive = false;
        for (int i = 0; i < characterInfos.Length; i++)
        {
            //characterInfos[i].isActive = false;
            //if(i == 0) { characterInfos[i].isActive = true; }
            if (characterInfos[i].isActive) { currentSpriteIndex = i; hasActive = true; }
            characterInfos[i].gameObject.GetComponent<Renderer>().enabled = characterInfos[i].isActive;
        }
        if (!hasActive)
        {
            for (int i = 0; i < characterInfos.Length; i++)
            {
                if (i == 0) { characterInfos[i].isActive = true; }
                characterInfos[i].gameObject.GetComponent<Renderer>().enabled = characterInfos[i].isActive;
            }
        }
        //scoreManager.ResetScore();
        //timer.ResetTimer();
        shouldStartGame = true;
    }

    void ClearScreen()
    {
        for (int i = 0; i < characterInfos.Length; i++)
        {
            characterInfos[i].gameObject.GetComponent<Renderer>().enabled = false;
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
        if (Input.GetButtonDown("Start") && !shouldStartGame)
        {
            ResetGame();
        }

        if (shouldStartGame)
        {

            if (Input.GetButtonDown("Right"))
            {
                Vector2 NextNumAndRoll = characterInfos[currentSpriteIndex].numAndRoll + new Vector2(1, 0);
                ShowNextSprite(NextNumAndRoll);
            }

            if (Input.GetButtonDown("Left"))
            {
                Vector2 NextNumAndRoll = characterInfos[currentSpriteIndex].numAndRoll + new Vector2(-1, 0);
                ShowNextSprite(NextNumAndRoll);
            }

            if (Input.GetButtonDown("Up"))
            {
                if (LadderCheckAbove())
                {
                    Vector2 NextNumAndRoll = characterInfos[currentSpriteIndex].ladderAbove.GetComponent<CharacterInfo>().numAndRoll;
                    ShowNextSprite(NextNumAndRoll);

                }
            }

            if (Input.GetButtonDown("Down"))
            {
                if (LadderCheckBelow())
                {
                    Vector2 NextNumAndRoll = characterInfos[currentSpriteIndex].ladderBelow.GetComponent<CharacterInfo>().numAndRoll;
                    ShowNextSprite(NextNumAndRoll);
                }
            }
        }

    }

    private bool LadderCheckAbove()
    {
        if (characterInfos[currentSpriteIndex].ladderAbove == null) { return false; }
        return true;
    }

    private bool LadderCheckBelow()
    {
        if (characterInfos[currentSpriteIndex].ladderBelow == null) { return false; }
        return true;
    }

    private void ShowNextSprite(Vector2 NextNumAndRoll)
    {
        for (int i = 0; i < characterInfos.Length; i++)
        {
            if (characterInfos[i].numAndRoll == NextNumAndRoll)
            {
                characterInfos[currentSpriteIndex].isActive = false;
                characterInfos[i].isActive = true;
                UpdateVisibility(i);
                currentSpriteIndex = i;
                if (OnMoved!=null)
                {
                    OnMoved();
                }
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
