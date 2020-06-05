using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionLinearDialogue : MonoBehaviour
{
    #region Variables
    [Header("References")]
    public bool showDlg;
    public int index, optionIndex;
    public Vector2 scr;
    public Player.MouseLook playerMouseLook;
    public string[] dialogueText;
    #endregion

    private void OnGUI()
    {
        if(showDlg)
        {
            scr.x = Screen.width / 16;
            scr.y = Screen.height / 9;

            GUI.Box(new Rect(0, 6 * scr.y, Screen.width, 3 * scr.y), name + " : " + dialogueText[index]);
            if (!(index >= dialogueText.Length - 1 || index == optionIndex))
            { 
                if (GUI.Button(new Rect(15 * scr.x, 8.5F*scr.y,scr.x,0.5f*scr.y),"Next"))
                {
                    index++;
                }
            }

            else if (index == optionIndex)
            {
                if (GUI.Button(new Rect(14 * scr.x, 8.5F * scr.y, scr.x*2, 0.5f * scr.y), "Accept"))
                {
                    index++;
                }
                if (GUI.Button(new Rect(12 * scr.x, 8.5F * scr.y, scr.x*2, 0.5f * scr.y), "Decline"))
                {
                    index = dialogueText.Length - 1;
                }
            }
            else
            {
                if (GUI.Button(new Rect(15 * scr.x, 8.5f * scr.y, scr.x, scr.y * 0.5f),"Bye"))
                {
                    showDlg = false;
                    index = 0;
                    Camera.main.GetComponent<Player.MouseLook>().enabled = true;
                    playerMouseLook.enabled = true;
                    Cursor.lockState = CursorLockMode.Locked;  
                    Cursor.visible = false;
                }
            }
        }
    }
}
