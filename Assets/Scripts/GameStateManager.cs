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
    public AudioSource m_music;
    public static GameState gameState = GameState.Clear;

    [SerializeField] int losingDestroyedPillar = 3;
    // Start is called before the first frame update

    private void IncreasePillarDestroyed()
    {
        if(PillarManager.pillarsDestroyed >= losingDestroyedPillar)
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
        if (Input.GetButtonDown("Start"))
        {
            StartGame();
        }
    }

    public void ClearScreen()
    {

    }

    public IEnumerator ResetGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        if (!m_music) { yield return null; }
        m_music.Stop();
        yield return StartCoroutine(DelayExecution());
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
