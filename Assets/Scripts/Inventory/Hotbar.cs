using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    public PlayerHandler player;
    public float timerOne = 10;
    public float timerTwo = 10;
    public GameObject hotbarOne;
    public GameObject hotbarTwo;
    public GameObject consumeOne;
    public GameObject consumeTwo;
    private void Update()
    {
        if(!consumeOne && hotbarOne)
        {
            //timerOne += Time.deltaTime;
        }
        timerOne += Time.deltaTime;
        if (timerOne >= 10)
        {
            consumeOne.SetActive(true);
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (consumeOne)
            {
                if (player.attributes[0].currentValue < player.attributes[0].maxValue)
                {
                    player.attributes[0].currentValue = Mathf.Clamp(player.attributes[0].currentValue += 15, 0, player.attributes[0].maxValue);
                    consumeOne.SetActive(false);
                    timerOne = 0;
                }
            }
        }
        if (!consumeTwo && hotbarTwo)
        {
            //timerTwo += Time.deltaTime;
        }
        timerTwo += Time.deltaTime;
        if (timerTwo >= 10)
        {
            consumeTwo.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (consumeTwo)
            {
                if (player.attributes[0].currentValue < player.attributes[0].maxValue)
                {
                    player.attributes[0].currentValue = Mathf.Clamp(player.attributes[0].currentValue += 30, 0, player.attributes[0].maxValue);
                    consumeTwo.SetActive(false);
                    timerTwo = 0;
                }
            }
        }
    }
}
