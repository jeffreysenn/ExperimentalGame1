using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Clear,
    Failed
}

public class GameStateManager : MonoBehaviour
{
    public delegate void GameStateHandler(int gameIndex);
    public static event GameStateHandler OnStartGame;
    public AudioSource m_music;
    public static GameState gameState = GameState.Clear;
    public UIManager m_uiManager;

    public delegate void GameOverHandler();
    public static event GameOverHandler OnGameOver;

    [SerializeField] int maxDestroyedPillar = 3;
    public static int m_maxDestroyedPillar;

    [SerializeField] bool showTime = false;
     
    private bool hasCalled = false;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        //gameState = GameState.Clear;
        hasCalled = false;

        Pillar.OnDestroyed += CheckGameOver;
        FireController.OnFireHit += CheckGameOver;

        m_maxDestroyedPillar = maxDestroyedPillar;
    }

    private void OnDestroy()
    {
        Pillar.OnDestroyed -= CheckGameOver;
        FireController.OnFireHit -= CheckGameOver;

    }


    private void CheckGameOver()
    {
        if(PillarManager.pillarsDestroyed >= maxDestroyedPillar)
        {
            StartCoroutine(ResetGame());
        }
    }   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Exit"))
        {
            Application.Quit();
        }

        if (gameState != GameState.Playing)
        {
            if (Input.GetButtonDown("Start1"))
            {
                StartGame(1);
                //if (OnStartGame != null) { OnStartGame(0); }
            }
            else if (Input.GetButtonDown("Start2"))
            {
                StartGame(2);
                //if (OnStartGame != null) { OnStartGame(1); }
            }
        }


        if (showTime)
        {
            m_uiManager.UpdateSystemTime();
        }
    }

    public void ClearScreen()
    {

    }

    public IEnumerator ResetGame()
    {
        gameState = GameState.Failed;
        OnGameOver();
        yield return StartCoroutine(DelayExecution());
        SceneManager.LoadScene(0);
    }

    IEnumerator DelayExecution()
    {
        yield return new WaitForSeconds(1f);
    }

    public void StartGame(int i)
    {
        gameState = GameState.Playing;
        SceneManager.LoadScene(i);

    }

    
}
