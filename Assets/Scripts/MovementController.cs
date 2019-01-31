using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public delegate void MovementHandler(int i);
    public static event MovementHandler OnMoved;

    public ScoreManager scoreManager;
    public Timer timer;
    public PillarManager pillarManager;

    [SerializeField] bool canPillarBlockMovement = true;

    public GameObject[] playerObjs;
    private CharacterInfo[] characterInfos;
    private int currentSpriteIndex;
    private bool shouldStartGame = false;
    public bool isFrozen = false;
    private float frozenTimer;

    public void FreezeForSeconds(float time)
    {
        isFrozen = true;
        frozenTimer = time;
    }

    public Sprite m_gameOverSprite;

    void SetCharacterInfos()
    {
        playerObjs = GameObject.FindGameObjectsWithTag("Player");
        characterInfos = new CharacterInfo[playerObjs.Length];
        for(int i = 0; i < playerObjs.Length; i++)
        {
            characterInfos[i] = playerObjs[i].GetComponent<CharacterInfo>();
        }
    }

    void ResetGame(int gameIndex)
    {
        bool hasActive = false;
        for (int i = 0; i < characterInfos.Length; i++)
        {
            //characterInfos[i].isActive = false;
            //if(i == 0) { characterInfos[i].isActive = true; }
            if (characterInfos[i].isActive) { currentSpriteIndex = i; hasActive = true; }
            characterInfos[i].gameObject.GetComponent<Renderer>().enabled = characterInfos[i].isActive;
            SetLadder(i);
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
    void OnEnable()
    {
        SetCharacterInfos();
        ClearScreen();
        ResetGame(0);

        //GameStateManager.OnStartGame += ResetGame;
        GameStateManager.OnGameOver += ChangeIntoLosingSprite;
    }

    private void OnDisable()
    {
        //GameStateManager.OnStartGame -= ResetGame;
        GameStateManager.OnGameOver -= ChangeIntoLosingSprite;

    }

    void SetLadder(int characterIndex)
    {
        if (characterInfos[characterIndex].ladderAbove)
        {
            characterInfos[characterIndex].ladderAbove.ladderBelow = characterInfos[characterIndex];
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.gameState == GameState.Failed)
            return;

        if (isFrozen)
        {
            frozenTimer -= Time.deltaTime;
            if(frozenTimer < 0)
            {
                isFrozen = false;
            }
            return;
        }
        if (shouldStartGame)
        {

            if (Input.GetButtonDown("Right"))
            {
                Vector2 nextNumAndRoll = characterInfos[currentSpriteIndex].numAndRoll + new Vector2(1, 0);
                int nextSpriteIndex;
                if(DoesNextSpriteExist(nextNumAndRoll, out nextSpriteIndex))
                {
                    if (CanMoveToNextSprite(nextSpriteIndex))
                    {
                        ShowNextSprite(nextSpriteIndex);

                    }
                }
            }

            if (Input.GetButtonDown("Left"))
            {
                Vector2 nextNumAndRoll = characterInfos[currentSpriteIndex].numAndRoll + new Vector2(-1, 0);
                int nextSpriteIndex;
                if (DoesNextSpriteExist(nextNumAndRoll, out nextSpriteIndex))
                {
                    if (CanMoveToNextSprite(nextSpriteIndex))
                    {
                    ShowNextSprite(nextSpriteIndex);

                    }
                }
            }

            if (Input.GetButtonDown("Up"))
            {
                if (LadderCheckAbove())
                {
                    Vector2 nextNumAndRoll = characterInfos[currentSpriteIndex].ladderAbove.GetComponent<CharacterInfo>().numAndRoll;
                    int nextSpriteIndex;
                    if (DoesNextSpriteExist(nextNumAndRoll, out nextSpriteIndex))
                    {
                        ShowNextSprite(nextSpriteIndex);
                    }

                }
            }

            if (Input.GetButtonDown("Down"))
            {
                if (LadderCheckBelow())
                {
                    Vector2 nextNumAndRoll = characterInfos[currentSpriteIndex].ladderBelow.GetComponent<CharacterInfo>().numAndRoll;
                    int nextSpriteIndex;
                    if (DoesNextSpriteExist(nextNumAndRoll, out nextSpriteIndex))
                    {
                        ShowNextSprite(nextSpriteIndex);
                    }
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

    private bool DoesNextSpriteExist(Vector2 nextNumAndRoll, out int nextSpriteIndex)
    {
        for (int i = 0; i < characterInfos.Length; i++)
        {
            if (characterInfos[i].numAndRoll == nextNumAndRoll)
            {
                nextSpriteIndex = i;
                return true;
            }
        }
        nextSpriteIndex = new int();
        return false;
    }

    private bool CanMoveToNextSprite(int nextSpriteIndex)
    {
        if (!canPillarBlockMovement) { return true; }
        if(!characterInfos[currentSpriteIndex].pillar && !characterInfos[nextSpriteIndex].pillar) { return true; }
        else
        {
            int pillarRow = (int)characterInfos[currentSpriteIndex].numAndRoll.y;
            foreach (Pillar pillar in pillarManager.pillarsOfRows[pillarRow])
            {
                if (pillar.m_state != PillarStates.Destroyed)
                {
                    if ((pillar.transform.position.x - characterInfos[currentSpriteIndex].transform.position.x) * (pillar.transform.position.x - characterInfos[nextSpriteIndex].transform.position.x) < 0)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    private void ShowNextSprite(int nextSpriteIndex)
    {
        characterInfos[currentSpriteIndex].isActive = false;
        characterInfos[nextSpriteIndex].isActive = true;
        UpdateVisibility(nextSpriteIndex);
        currentSpriteIndex = nextSpriteIndex;
        if (OnMoved != null)
        {
            OnMoved(0);
        }
    }

    void UpdateVisibility(int newVisibleNum)
    {
        characterInfos[currentSpriteIndex].GetComponent<Renderer>().enabled = false;
        characterInfos[newVisibleNum].GetComponent<Renderer>().enabled = true;

    }

    private void ChangeIntoLosingSprite()
    {
        GetCurrentSprite().Lost();
    }

    public CharacterInfo GetCurrentSprite() { return characterInfos[currentSpriteIndex]; }

    public CharacterInfo[] GetCharacterInfos() { return characterInfos; }
}
