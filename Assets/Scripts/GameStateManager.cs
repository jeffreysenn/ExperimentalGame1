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
    // Start is called before the first frame update
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
        m_music.Stop();
        yield return StartCoroutine(DelayExecution());
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
        m_music.Play();
    }

    
}
