using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    [SerializeField] int controlledRow;
    List<FireInfo> controlledFires = new List<FireInfo>();
    [SerializeField] float movementInterval = 1;
    private float movementTimer;
    private int currentActiveIndex = 0;
    [SerializeField] Vector2 initialMovementDirection = new Vector2(1, 0);
    Vector2 movementDirection;

    // Start is called before the first frame update
    void Start()
    {
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
        Debug.Log(controlledFires.Count);

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


