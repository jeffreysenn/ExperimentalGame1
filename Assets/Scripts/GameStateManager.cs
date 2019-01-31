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

    public delegate void GameOverHandler();
    public static event GameOverHandler OnGameOver;

    [SerializeField] int maxDestroyedPillar = 3;
    public static int m_maxDestroyedPillar;
    // Start is called before the first frame update
    private void OnEnable()
    {
        Pillar.OnDestroyed += CheckGameOver;
        m_maxDestroyedPillar = maxDestroyedPillar;
    }

    private void OnDestroy()
    {
        Pillar.OnDestroyed -= CheckGameOver;
    }



    private void CheckGameOver()
    {
        if(PillarManager.pillarsDestroyed >= maxDestroyedPillar)
        {
            StartCoroutine(ResetGame());
        }
    }


    void Start()
    {
        gameState = GameState.Clear;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState != GameState.Playing && Input.GetButtonDown("Start"))
        {
            StartGame();
            if (OnStartGame != null) { OnStartGame(0); }
        }
    }

    public void ClearScreen()
    {

    }

    public IEnumerator ResetGame()
    {
        OnGameOver();
        gameState = GameState.Failed;
        yield return StartCoroutine(DelayExecution());
        if (m_music) { m_music.Stop(); }
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    IEnumerator DelayExecution()
    {
        yield return new WaitForSeconds(1f);
    }

    public void StartGame()
    {
        gameState = GameState.Playing;
        if (m_music)
        {
        m_music.Play();

        }
    }

    
}
