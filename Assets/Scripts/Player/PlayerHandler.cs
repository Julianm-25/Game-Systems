using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerHandler : Character
{
    #region Variables
    public static bool controllerMovement;
    [Header("Physics")]
    public CharacterController controller;
    public float gravity = 20f;
    public Vector3 moveDirection;
    [Header("Level Data")]
    public int level = 0;
    public float currentExp, neededExp, maxExp;
    public Quest quest;
    [Header("Damage Flash and Death")]
    public Image damageImage;
    public Image deathImage;
    public Text deathText;
    public AudioClip deathClip;
    public AudioSource playersAudio;
    public Transform currentCheckPoint;
    public Color flashColour = new Color(1,0,0,0.2f);
    public float flashSpeed = 5f;
    public static bool isDead;
    public bool isDamaged;
    public bool canHeal;
    public float healDelayTimer;
    #region Customisation
    public int playerClass, playerRace, skinTexture, hairTexture, eyeTexture, mouthTexture, clothesTexture, armourTexture;
    #endregion
    #endregion
    #region Behaviour
    void Start()
    {
        controller = this.gameObject.GetComponent<CharacterController>();
        attributes[0].maxValue = 100 + characterStats[2].value;
        attributes[1].maxValue = 100 + characterStats[2].value;
        attributes[2].maxValue = 100 + characterStats[3].value;
    }
    public override void Movement()
    {
        
        if(!isDead)
        {
            float horizontal = 0;
            float vertical = 0;
            if (!controllerMovement)
            {
                if (Input.GetKey(KeyBindManager.keys["Forward"]))
                {
                    vertical++;
                }
                if (Input.GetKey(KeyBindManager.keys["Left"]))
                {
                    horizontal--;
                }
                if (Input.GetKey(KeyBindManager.keys["Right"]))
                {
                    horizontal++;
                }
                if (Input.GetKey(KeyBindManager.keys["Backward"]))
                {
                    vertical--;
                }
            }
            else
            {
                horizontal = Input.GetAxis("Horizontal");
                vertical = Input.GetAxis("Vertical");
            }
            if (Input.GetKey(KeyBindManager.keys["Sprint"]))
            {
                if(attributes[1].currentValue > 0)
                {
                    speed = sprint + characterStats[1].value;
                    attributes[1].currentValue -= Time.deltaTime * 10;
                }
            }
            else if(Input.GetKey(KeyBindManager.keys["Crouch"]))
            {
                speed = crouch;
            }
            else
            {
                speed = 5f;
            }
            if (controller.isGrounded)
            {
                moveDirection = transform.TransformDirection(new Vector3(horizontal, 0, vertical));
                moveDirection *= speed;
                if (Input.GetButton("Jump"))
                {
                    moveDirection.y = jumpSpeed;
                }
            }
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }       
    }
    public void Abilities()
    {
        if (Input.GetKeyDown(KeyBindManager.keys["Ability1"]) && attributes[2].currentValue >= 20)
        {
            if(playerRace == 0)
            {
                Debug.Log("Human Ability");
            }
            else
            {
                Debug.Log("Tree Ability");
            }
            attributes[2].currentValue -= 20;
        }
        if (Input.GetKeyDown(KeyBindManager.keys["Ability2"]) && attributes[2].currentValue >= 20)
        {
            if (playerClass == 0)
            {
                Debug.Log("Barbarian Ability");
            }
            else if (playerClass == 1)
            {
                Debug.Log("Bard Ability");
            }
            else if (playerClass == 2)
            {
                Debug.Log("Druid Ability");
            }
            else if (playerClass == 3)
            {
                Debug.Log("Monk Ability");
            }
            else if (playerClass == 4)
            {
                Debug.Log("Paladin Ability");
            }
            else if (playerClass == 5)
            {
                Debug.Log("Ranger Ability");
            }
            else if (playerClass == 6)
            {
                Debug.Log("Sorcerer Ability");
            }
            else if (playerClass == 7)
            {
                Debug.Log("Warlock Ability");
            }
            attributes[2].currentValue -= 20;
        }
    }
    public override void Update()
    {
        base.Update();
        Abilities();
        if(currentExp >= maxExp)
        {
            level++;
            currentExp -= maxExp;
            maxExp += 10;
            attributes[0].maxValue += 1 + characterStats[2].value;
            attributes[1].maxValue += 1 + characterStats[2].value;
            attributes[2].maxValue += 1 + characterStats[3].value;
        }
        #region Bar Update

        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].displayImage.fillAmount = Mathf.Clamp01(attributes[i].currentValue/ attributes[i].maxValue);
        }
        #endregion
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.X))
        {
            DamagePlayer(5);
        }
        #endif
        #region Damage Flash
        if (isDamaged && !isDead)
        {
            damageImage.color = flashColour;
            isDamaged = false;
        }
        else if(damageImage.color.a > 0)
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        #endregion
        if(!canHeal)
        {
            healDelayTimer += Time.deltaTime;
            if(healDelayTimer >= 5)
            {
                canHeal = true;
            }           
        }
        if (canHeal && attributes[0].currentValue < attributes[0].maxValue && attributes[0].currentValue > 0)
        {
            HealOverTime();
        }
        if (attributes[1].currentValue < attributes[1].maxValue)
        {
            StaminaOverTime();
        }
        if (attributes[2].currentValue < attributes[2].maxValue)
        {
            ManaOverTime();
        }
    }
    public void DamagePlayer(float damage)
    {
        //Turn on the red flicker
        isDamaged = true;
        //take damage
        attributes[0].currentValue -= damage;
        //delay regen healing
        canHeal = false;
        healDelayTimer = 0;
        if(attributes[0].currentValue <= 0 && !isDead)
        {
            Death();
        }
    }
    public void HealOverTime()
    {
        attributes[0].currentValue += Time.deltaTime * (attributes[0].regenValue + characterStats[2].value);
    }
    public void StaminaOverTime()
    {
        attributes[1].currentValue += Time.deltaTime * (attributes[1].regenValue + characterStats[2].value);
    }
    public void ManaOverTime()
    {
        attributes[2].currentValue += Time.deltaTime * (attributes[2].regenValue + characterStats[3].value);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CheckPoint"))
        {
            currentCheckPoint = other.transform;
            for (int i = 0; i < attributes.Length; i++)
            {
                attributes[i].regenValue += 7;
            }
            PlayerSaveAndLoad.Save();
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CheckPoint")
        {
            for (int i = 0; i < attributes.Length; i++)
            {
                attributes[i].regenValue -= 7;
            }
            PlayerSaveAndLoad.Save();

        }
    }
    #endregion
    #region Quest
    public void KilledCreature(string enemyTag)
    {
        if(quest.goal.questState == QuestState.Active)
        {
            quest.goal.EnemyKilled(enemyTag);
        }
    }
    public void ItemCollected(int id)
    {
        if (quest.goal.questState == QuestState.Active)
        {
            quest.goal.ItemCollected(id);
        }
    }
    #endregion
    #region Enable and Disable
    public void Enable()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Camera.main.GetComponent<Player.MouseLook>().enabled = true;
        GetComponent<Player.MouseLook>().enabled = true;
    }
    public void Disable()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Camera.main.GetComponent<Player.MouseLook>().enabled = false;
        GetComponent<Player.MouseLook>().enabled = false;
    }
    #endregion

    #region Death and Respawn

    void Death()
    {
        //Set the death flag to dead
        isDead = true;
        //clear existing text just in case!
        deathText.text = "";
        //Set and Play the audio clip
        playersAudio.clip = deathClip;
        playersAudio.Play();
        //Trigger death screen
        deathImage.GetComponent<Animator>().SetTrigger("isDead");
        //in 2 seconds set our text when we die
        Invoke("DeathText", 2f);
        //in 6 seconds set our text when we respawn
        Invoke("RespawnText", 6f);
        //in 9 seconds respawn us
        Invoke("Respawn", 9f);
    }
    void DeathText()
    {
        deathText.text = "You've Fallen in Battle...";
    }
    void RespawnText()
    {
        deathText.text = "...But the Gods have decided it is not your time...";
    }
    void Respawn()
    {
        //RESET EVERYTHING!
        deathText.text = "";
        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].currentValue = attributes[i].maxValue;
        }
        isDead = false;
        //load position
        this.transform.position = currentCheckPoint.position;
        this.transform.rotation = currentCheckPoint.rotation;
        //Respawn
        deathImage.GetComponent<Animator>().SetTrigger("Respawn");
    }
    #endregion
}
