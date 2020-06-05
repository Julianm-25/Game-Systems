using UnityEngine;
using System.Collections;
//this script can be found in the Component section under the option Intro RPG/Player/Interact
[AddComponentMenu("Game Systems/RPG/Player/Interact")]
public class Interact : MonoBehaviour
{
    #region Variables
    //[Header("Player and Camera connection")]
    #endregion
    private void Update()
    {
        if (Input.GetKeyDown(KeyBindManager.keys["Interact"]))
        {
            Ray interact;

            interact = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));

            RaycastHit hitInfo;


            if (Physics.Raycast(interact, out hitInfo, 10))
            {
                #region NPC

                if (hitInfo.collider.CompareTag("NPC"))
                {
                    if(hitInfo.collider.GetComponent<Dialogue>())
                    {
                        hitInfo.collider.GetComponent<Dialogue>().DialogueAdvance();
                        GetComponent<DialogueController>().reference = hitInfo.collider.GetComponent<Dialogue>();
                    }
                    if (hitInfo.collider.GetComponent<OptionLinearDialogue>())
                    {
                        hitInfo.collider.GetComponent<OptionLinearDialogue>().showDlg = true;
                    }
                    
                }
                #endregion

                #region Item
                if (hitInfo.collider.CompareTag("Item"))
                {
                    Debug.Log("Pick Up Item");
                    ItemHandler handler = hitInfo.transform.GetComponent<ItemHandler>();
                    if(handler != null)
                    {
                        handler.OnCollection();
                    }
                }
                #endregion

                #region Chest
                if(hitInfo.collider.CompareTag("Chest"))
                {
                    Chest chest = hitInfo.transform.GetComponent<Chest>();
                    if(chest != null)
                    {
                        chest.showChestInv = true;
                        LinearInventory.showInv = true;
                        Time.timeScale = 0;
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        LinearInventory.currentChest = chest;
                    }
                }
                #endregion
                #region Shop
                if (hitInfo.collider.CompareTag("Shop"))
                {
                    Shop shop = hitInfo.transform.GetComponent<Shop>();
                    if (shop != null)
                    {
                        shop.showShopInv = true;
                        LinearInventory.showInv = true;
                        Time.timeScale = 0;
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        LinearInventory.currentShop = shop;
                    }
                }
                #endregion
            }
        }
    }
}






