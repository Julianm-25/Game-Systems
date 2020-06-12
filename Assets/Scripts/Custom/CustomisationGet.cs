using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomisationGet : MonoBehaviour
{
    public PlayerHandler player;
    public Renderer characterMesh;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHandler>();
        characterMesh = GameObject.FindGameObjectWithTag("CharacterMesh").GetComponent<Renderer>();

        string[] tempName = new string[] { "Strength", "Dexterity", "Constitution", "Wisdom", "Intelligence", "Charisma" };
        for (int i = 0; i < tempName.Length; i++)
        {
            player.characterStats[i].name = tempName[i];
        }
        Load();
    }
    public void Load()
    {
        if (!PlayerPrefs.HasKey("CharacterName"))
        {
            SceneManager.LoadScene(1);
        }
        player.gameObject.name = PlayerPrefs.GetString("CharacterName");

        SetTexture("Skin", player.skinTexture);
        SetTexture("Hair", player.hairTexture);
        SetTexture("Eyes", player.eyeTexture);
        SetTexture("Mouth", player.mouthTexture);
        SetTexture("Clothes", player.clothesTexture);
        SetTexture("Armour", player.armourTexture);

        for (int i = 0; i < player.characterStats.Length; i++)
        {
            player.characterStats[i].value = PlayerPrefs.GetInt(player.characterStats[i].name);
        }
    }
    void SetTexture(string type, int index)
    {
        Texture2D texture = null;
        int matIndex = 0;
        switch (type)
        {
            case "Skin":
                texture = Resources.Load("Character/Skin_" + index) as Texture2D;
                matIndex = 1;
                break;
            case "Hair":
                texture = Resources.Load("Character/Hair_" + index) as Texture2D;
                matIndex = 2;
                break;
            case "Eyes":
                texture = Resources.Load("Character/Eyes_" + index) as Texture2D;
                matIndex = 3;
                break;
            case "Mouth":
                texture = Resources.Load("Character/Mouth_" + index) as Texture2D;
                matIndex = 4;
                break;
            case "Clothes":
                texture = Resources.Load("Character/Clothes_" + index) as Texture2D;
                matIndex = 5;
                break;
            case "Armour":
                texture = Resources.Load("Character/Armour_" + index) as Texture2D;
                matIndex = 6;
                break;
        }
        Material[] mats = characterMesh.materials;
        mats[matIndex].mainTexture = texture;
        characterMesh.materials = mats;
    }
    public void LoadFromPrefs()
    {
        player.playerClass = PlayerPrefs.GetInt("CharacterClass");
        player.playerRace = PlayerPrefs.GetInt("CharacterRace");
        player.skinTexture = PlayerPrefs.GetInt("SkinIndex");
        player.hairTexture = PlayerPrefs.GetInt("HairIndex");
        player.eyeTexture = PlayerPrefs.GetInt("EyesIndex");
        player.mouthTexture = PlayerPrefs.GetInt("MouthIndex");
        player.clothesTexture = PlayerPrefs.GetInt("ClothesIndex");
        player.armourTexture = PlayerPrefs.GetInt("ArmourIndex");

        player.name = PlayerPrefs.GetString("CharacterName");
        for (int i = 0; i < player.characterStats.Length; i++)
        {
            player.characterStats[i].value = PlayerPrefs.GetInt(player.characterStats[i].name);
        }
        Load();
    }
}
