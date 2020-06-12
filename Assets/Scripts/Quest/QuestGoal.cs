using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public QuestState questState;
    public GoalType goalType;
    public string enemyType;
    public int itemId;
    public int requiredAmount;
    public int currentAmount;
    public bool isReached;
    public void EnemyKilled(string type)
    {
        if(goalType == GoalType.Kill && type == enemyType)
        {
            currentAmount++;
            if(currentAmount >= requiredAmount)
            {
                isReached = true;
                questState = QuestState.Complete;
                Debug.Log("Quest Complete");
            }
        }
    }
    public void ItemCollected(int id)
    {
        if(goalType == GoalType.Gather && id == itemId)
        {
            currentAmount++;
            if (currentAmount >= requiredAmount)
            {
                isReached = true;
                questState = QuestState.Complete;
                Debug.Log("Quest Complete");
            }
        }
    }
}
public enum GoalType
{
    Kill,
    Gather
}
public enum QuestState
{
    Available,
    Active,
    Complete,
    Claimed
}
