using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public GameObject UIManagerObj;
    public GameObject gameStateManagerObj;
    public static float timer = 0;
    int preIntTime = 0;
    int intTime = 0;

    // Update is called once per frame
    private void Start()
    {
        ResetTimer();
    }

    public void ResetTimer()
    {
        timer = 0;
        preIntTime = 0;
        intTime = 0;
        UIManagerObj.GetComponent<UIManager>().UpdateTime(intTime);

    }

    void Update()
    {
        if(GameStateManager.gameState == GameState.Failed) { return; }
        timer += Time.deltaTime;
        intTime = Mathf.FloorToInt(timer);
        if(intTime != preIntTime)
        {
            if(UIManagerObj == null) { return; }
            UIManagerObj.GetComponent<UIManager>().UpdateTime(intTime);
            preIntTime = intTime;
        }
    }
}
