using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//this script can be found in the Component section under the option NPC/Dialogue
[AddComponentMenu("Game Systems/RPG/NPC/Dialogue Linear")]

public class Dialogue : MonoBehaviour
{
    #region Variables
    [Header("References")]
    //boolean to toggle if we can see a characters dialogue box
    public bool showDlg;
    //index for our current line of dialogue and an index for a set question marker of the dialogue 
    public int index;
    public Vector2 scr;
    [Header("NPC Name and Dialogue")]
    //name of this specific NPC
    public new string name;
    //array for neutral text
    public string[] neutralarray;
    //array for positive text
    public string[] positivearray;
    //array for negative text
    public string[] negativearray;
    //npc approval
    public int approval = 0;
    //dialogue box
    public GameObject dialoguebox;
    //name of the NPC talking
    public Text dialoguename;
    //where the text is displayed
    public Text dialoguetext;
    //displays npc approval status
    public Text approvalstatus;
    #endregion
    #region Start
    //find and reference the player object by tag
    //find and reference the maincamera by tag and get the mouse look component 
    #endregion
    public void DialogueBegin()
    {
        showDlg = true;
        dialoguebox.SetActive(true);
        dialoguename.text = name;
        FindObjectOfType<PlayerHandler>().Disable();
        if(approval == 0)
        {
            approvalstatus.text = "Status: Neutral";
            dialoguetext.text = neutralarray[0];
        }
        if(approval == 1)
        {
            approvalstatus.text = "Status: Friendly";
            dialoguetext.text = positivearray[0];
        }
        if(approval == -1)
        {
            approvalstatus.text = "Status: Unfriendly";
            dialoguetext.text = negativearray[0];
        }
        
    }
    public void DialogueAdvance()
    {
        if(!showDlg)
        {
            DialogueBegin();
        }
        else if(approval == 0)
        {
            approvalstatus.text = "Status: Neutral";
            index++;
            if(index >= neutralarray.Length)
            {
                DialogueEnd();
            }
            else
            {
                dialoguetext.text = neutralarray[index];
            }
        }
        else if (approval == 1)
        {
            approvalstatus.text = "Status: Friendly";
            index++;
            if (index >= positivearray.Length)
            {
                DialogueEnd();

            }
            else
            {
                dialoguetext.text = positivearray[index];
            }
        }
        else if (approval == -1)
        {
            approvalstatus.text = "Status: Unfriendly";
            index++;
            if (index >= negativearray.Length)
            {
                DialogueEnd();
            }
            else
            {
                dialoguetext.text = negativearray[index];
            }
        }
    }
    public void PositiveOption()
    {
        approval += 1;
        if(approval > 1)
        {
            approval = 1;
        }
        DialogueAdvance();
    }
    public void NegativeOption()
    {
        approval -= 1;
        if(approval < -1)
        {
            approval = -1;
        }
        DialogueAdvance();
    }
    public void DialogueEnd()
    {
        showDlg = false;
        dialoguebox.SetActive(false);
        FindObjectOfType<PlayerHandler>().Enable();
        index = 0;
        FindObjectOfType<DialogueController>().reference = null;
    }
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyBindManager.keys["Interact"]))
        {
            Ray interact;

            interact = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

            RaycastHit hitInfo;


            if (Physics.Raycast(interact, out hitInfo, 10))
            {
                if (hitInfo.collider.CompareTag("NPC"))
                {
                    dialoguebox.SetActive(true);
                    dialoguename.text = name;
                    dialoguetext.text = dialogueText[0];
                    Time.timeScale = 0;
                }
            }
        }
    }*/
}