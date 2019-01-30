using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public struct TimeAndDifficulty
{
    public Vector2 timeSlot;
    public Vector2 activationPeriod;
}

public class PillarManager : MonoBehaviour
{
    public TimeAndDifficulty[] timeAndDifficulties;
    private int tdIndex;

    public static int pillarsDestroyed = 0;

    public GameStateManager gameStateManager;
    public Pillar[] m_pillars;

    public List<Pillar>[] pillarsOfRows;

    public float m_activationPeriod;
    private float m_activationTime;

    public delegate void GameOverHandler();
    public static event GameOverHandler OnGameOver;

    public bool m_gameOver = false;

    private void OnEnable()
    {
        ScoreManager.OnAddedLife += RepairAllDestroyedPillars;
    }

    private void OnDisable()
    {
        ScoreManager.OnAddedLife -= RepairAllDestroyedPillars;

    }

    void Start()
    {
        GameObject[] pillarObjs = GameObject.FindGameObjectsWithTag("Pillar");
        m_pillars = new Pillar[pillarObjs.Length];
        for(int i = 0; i<pillarObjs.Length;i++)
        {
            m_pillars[i] = pillarObjs[i].GetComponent<Pillar>();
        }

        SortPillarsIntoRows();
        tdIndex = 0;
    }

    void SortPillarsIntoRows()
    {
        int rowNum = GetRowNum();

        pillarsOfRows = new List<Pillar>[rowNum];
        for (int i = 0; i < rowNum; i++)
        {
            pillarsOfRows[i] = new List<Pillar>();
        }
        foreach (Pillar pillar in m_pillars)
        {
            for (int i = 0; i < rowNum; i++)
            {
                if ((int)pillar.row == i)
                {
                    pillarsOfRows[i].Add(pillar);
                }
            }
        }

        //for (int i = 0; i < rowNum; i++)
        //{
        //    Debug.Log("Row: " + i);
        //    foreach (Pillar pillar in pillarsOfRows[i])
        //    {
        //        Debug.Log(pillar.gameObject.name);
        //    }
        //}
    }

    int GetRowNum()
    {
        List<int> seenRowNums = new List<int>();
        seenRowNums.Add((int)m_pillars[0].row);

        foreach (Pillar pillar in m_pillars)
        {
            bool hasSeen = false;
            foreach (int seenRowNum in seenRowNums)
            {
                if ((int)pillar.row == seenRowNum)
                {
                    hasSeen = true;
                    break;
                }
            }
            if (!hasSeen) { seenRowNums.Add((int)pillar.row); }
        }

        return seenRowNums.Count();
    }

    void Update()
    {
        if (GameStateManager.gameState == GameState.Clear)
        {
            return;
        }


        if(tdIndex < timeAndDifficulties.Length)
        {
            if ((Timer.timer - timeAndDifficulties[tdIndex].timeSlot.x) * (Timer.timer - timeAndDifficulties[tdIndex].timeSlot.y) < 0)
            {
                m_activationPeriod = Mathf.Lerp(timeAndDifficulties[tdIndex].activationPeriod.x, timeAndDifficulties[tdIndex].activationPeriod.y, (Timer.timer - timeAndDifficulties[tdIndex].timeSlot.x)/ (timeAndDifficulties[tdIndex].timeSlot.y - timeAndDifficulties[tdIndex].timeSlot.x));
            }
            else if(Time.time > timeAndDifficulties[tdIndex].timeSlot.y)
            {
                tdIndex++;
            }
        }
        

        m_activationTime += Time.deltaTime;
        if (m_activationTime >= m_activationPeriod)
        {
            m_activationTime = 0;
            CrackRandomPillar();
        }

        //if (Input.GetKeyDown(KeyCode.Space))
          //  RepairAllPillars();

    }

    public void RepairAllDestroyedPillars()
    {
        foreach (Pillar m_pillar in m_pillars)
        {
            if (m_pillar.m_state == PillarStates.Destroyed)
            {
                m_pillar.Repair();

            }
        }
    }

    void RepairAllPillars()
    {
        foreach(Pillar m_pillar in m_pillars)
        {
            m_pillar.Repair();
        }
    }

    void CrackRandomPillar()
    {
        if (m_pillars.All(m_pillar => m_pillar.IsDestructing))
            return;


        List<Pillar> m_intactPillars = new List<Pillar>();

        foreach (Pillar m_pillar in m_pillars)
        {
            if (!m_pillar.IsDestructing)
            {
                m_intactPillars.Add(m_pillar);
            }
        }
        int m_random = Random.Range(0, m_intactPillars.Count);

        m_intactPillars[m_random].TriggerDestruction();
    }

    public void ReportPillarDestruction(int rowNum)
    {
        if (ArePillarsOfRowDestroyed(rowNum))
        {
            OnGameOver();
            m_gameOver = true;
            EnablePillars(false);
            StartCoroutine(gameStateManager.ResetGame());
        }
    }

    private void EnablePillars(bool p_value)
    {
        foreach (Pillar m_pillar in m_pillars)
            m_pillar.enabled = p_value;
    }

    private bool ArePillarsOfRowDestroyed(int rowNum)
    {
        foreach(Pillar pillar in pillarsOfRows[rowNum])
        {
            if(pillar.m_state != PillarStates.Destroyed)
            {
                return false;
            }
        }
        return true;
    }
}
