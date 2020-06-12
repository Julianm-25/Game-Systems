using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{

    public bool isActive;
    public bool isClaimed;
    public string title;
    public string description;
    public int experienceReward;
    public int goldReward;
    public QuestGoal goal;
}


