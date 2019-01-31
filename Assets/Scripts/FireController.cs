using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    public delegate void FireHandler();
    public static event FireHandler OnFireHit;

    [SerializeField] int controlledRow;
    List<FireInfo> controlledFires = new List<FireInfo>();
    [SerializeField] float movementInterval = 1;
    private float movementTimer;
    private int currentActiveIndex = 0;
    [SerializeField] Vector2 initialMovementDirection = new Vector2(1, 0);
    Vector2 movementDirection;
    [SerializeField] float movementFrozenTime = 1;
    private MovementController movementController;


    // Start is called before the first frame update
    void Start()
    {
        movementController = GameObject.FindGameObjectWithTag("PlayerController").GetComponent<MovementController>();

        GameObject[] allFireObjs = GameObject.FindGameObjectsWithTag("Fire");
        FireInfo[] allFireInfos = new FireInfo[allFireObjs.Length];
        for (int i = 0; i < allFireInfos.Length; i++)
        {
            allFireInfos[i] = allFireObjs[i].GetComponent<FireInfo>();
            if((int)allFireInfos[i].numAndRow.y == controlledRow)
            {
                controlledFires.Add(allFireInfos[i]);
            }
        }

        //ClearScreen();

        ResetFire(0);

    }

    private void OnEnable()
    {
        GameStateManager.OnStartGame += ResetFire;
    }

    private void OnDisable()
    {
        GameStateManager.OnStartGame -= ResetFire;

    }

    private void ClearScreen()
    {
        for (int i = 0; i < controlledFires.Count; i++)
        {
            if (controlledFires[i].isActive)
            {
                currentActiveIndex = i;
            }
            controlledFires[i].gameObject.GetComponent<Renderer>().enabled = false;
        }
    }

    private void ResetFire(int gameIndex)
    {
        for (int i = 0; i < controlledFires.Count; i++)
        {
            if (controlledFires[i].isActive)
            {
                currentActiveIndex = i;
            }
            controlledFires[i].gameObject.GetComponent<Renderer>().enabled = false;
        }
        controlledFires[currentActiveIndex].gameObject.GetComponent<Renderer>().enabled = true;
        movementTimer = movementInterval;
        movementDirection = initialMovementDirection;
    }

    private void Update()
    {
        // if (GameStateManager.gameState != GameState.Playing) { return; }

        if (GameStateManager.gameState == GameState.Failed)
        {
            return;
        }

        if (controlledFires[currentActiveIndex].characterBelow 
            && controlledFires[currentActiveIndex].isActive 
            && controlledFires[currentActiveIndex].characterBelow.isActive
            && !movementController.isFrozen)
        {
            movementController.FreezeForSeconds(movementFrozenTime);
            if(OnFireHit!= null) { OnFireHit(); }
        }

        movementTimer -= Time.deltaTime;
        if(movementTimer < 0)
        {
            Vector2 shouldNextNumRow = controlledFires[currentActiveIndex].numAndRow + movementDirection;
            Vector2 nextNumRow;
            int outNextSpriteIndex;
            if(DoesNextSpriteExist(shouldNextNumRow, out outNextSpriteIndex))
            {
                nextNumRow = shouldNextNumRow;
            }
            else
            {
                movementDirection *= -1;
                nextNumRow = controlledFires[currentActiveIndex].numAndRow + movementDirection;
            }
            if (DoesNextSpriteExist(nextNumRow, out outNextSpriteIndex))
            {
                MoveToNextSprite(outNextSpriteIndex);
            }
            movementTimer = movementInterval;
        }
    }

    private bool DoesNextSpriteExist(Vector2 nextNumRow, out int nextSpriteIndex)
    {
        for (int i = 0; i < controlledFires.Count; i++)
        {
            if (controlledFires[i].numAndRow == nextNumRow)
            {
                nextSpriteIndex = i;
                return true;
            }
        }
        nextSpriteIndex = new int();
        return false;
    }

    private void MoveToNextSprite(int nextSpriteIndex)
    {
        controlledFires[currentActiveIndex].isActive = false;
        controlledFires[nextSpriteIndex].isActive = true;
        UpdateVisibility(nextSpriteIndex);
        currentActiveIndex = nextSpriteIndex;
        //if (OnMoved != null)
        //{
        //    OnMoved(0);
        //}
    }

    void UpdateVisibility(int newVisibleNum)
    {
        controlledFires[currentActiveIndex].GetComponent<Renderer>().enabled = false;
        controlledFires[newVisibleNum].GetComponent<Renderer>().enabled = true;

    }

}


