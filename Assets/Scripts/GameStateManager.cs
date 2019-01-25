using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Clear
}

public class GameStateManager : MonoBehaviour
{
    public GameState gameState = GameState.Clear;
    // Start is called before the first frame update
    void Start()
    {
        
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

    public void ResetGame()
    {
        Debug.Log("Game Over!");
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void StartGame()
    {
        gameState = GameState.Playing;
    }

    
}
