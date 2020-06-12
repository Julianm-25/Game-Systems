using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public PlayerHandler player;
    public LinearInventory inventory;
    public GameObject questWindow;
    public Text menuTitle;
    public Text menuDescription;
    public Text menuExperienceReward;
    public Text menuGoldReward;

    public Text titleText, descriptionText, experienceText, goldText;
    public void OpenQuestWindow()
    {
        questWindow.SetActive(true);
        titleText.text = quest.title;
        menuTitle.text = quest.title;
        descriptionText.text = quest.description;
        menuDescription.text = quest.description;
        experienceText.text = quest.experienceReward.ToString() + "exp";
        menuExperienceReward.text = quest.experienceReward.ToString() + "exp";
        goldText.text = "$" + quest.goldReward.ToString();
        menuGoldReward.text = "$" + quest.goldReward.ToString();
    }
    public void AcceptQuest()
    {
        quest.goal.questState = QuestState.Active;
        player.quest = quest;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }
    public void DeclineQuest()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        questWindow.SetActive(false);
    }
    public void ClaimQuest()
    {
        player.currentExp += quest.experienceReward;
        LinearInventory.money += quest.goldReward;
        quest.goal.questState = QuestState.Claimed;
        Debug.Log("You got " + quest.experienceReward + "exp points and $" + quest.goldReward);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        menuTitle.text = "";
        menuDescription.text = "";
        menuExperienceReward.text = "";
        menuGoldReward.text = "";
    }
}
