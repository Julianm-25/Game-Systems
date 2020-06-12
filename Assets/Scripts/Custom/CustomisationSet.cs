using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomisationSet : Stats
{
    #region Variables
    [Header("Character Name")]
    //name of character
    public string characterName;
    public InputField nameField;
    [Header("Character Class")]
    public CharacterClass charClass = CharacterClass.Barbarian;
    public string[] selectedClass = new string[8];
    public int selectedIndex = 0;
    public GameObject statButton;
    public GameObject strButton;
    public GameObject dexButton;
    public GameObject conButton;
    public GameObject intButton;
    public GameObject wisButton;
    public GameObject chaButton;

    [Header("Dropdown Menu")]
    public bool showDropdown;
    public Vector2 scrollPos;
    public string classButton = "";
    public int statPoints = 10;

    [Header("Texture List")]
    public List<Texture2D> skin = new List<Texture2D>();
    public List<Texture2D> hair = new List<Texture2D>();
    public List<Texture2D> eyes = new List<Texture2D>();
    public List<Texture2D> mouth = new List<Texture2D>();
    public List<Texture2D> clothes = new List<Texture2D>();
    public List<Texture2D> armour = new List<Texture2D>();
    [Header("Index")]
    public int skinIndex;
    public int raceIndex;
    public int hairIndex, eyesIndex, mouthIndex, clothesIndex, armourIndex;
    [Header("Renderer")]
    public Renderer characterRenderer;
    public SkinnedMeshRenderer charMesh;
    [Header("Max Index")]
    public int skinMax;
    public int hairMax, eyesMax, mouthMax, clothesMax, armourMax;
    public Mesh humanMesh;
    public Mesh treeMesh;
    #endregion
    #region Start
    private void Start()
    {
        selectedClass = new string[] { "Barbarian", "Bard", "Druid", "Monk", "Paladin", "Ranger", "Sorcerer", "Warlock" };
        string[] tempName = new string[] { "Strength", "Dexterity", "Constitution", "Wisdom", "Intelligence", "Charisma" };
        for (int i = 0; i < tempName.Length; i++)
        {
            characterStats[i].name = tempName[i];
        }
        #region for loop to pull textures from file
        #region Skin
        for (int i = 0; i < skinMax; i++)
        {
            Texture2D temp = Resources.Load("Character/Skin_" + i) as Texture2D;
            skin.Add(temp);
        }
        #endregion
        #region Hair
        for (int i = 0; i < hairMax; i++)
        {
            Texture2D temp = Resources.Load("Character/Hair_" + i) as Texture2D;
            hair.Add(temp);
        }
        #endregion
        #region Eyes
        for (int i = 0; i < eyesMax; i++)
        {
            Texture2D temp = Resources.Load("Character/Eyes_" + i) as Texture2D;
            eyes.Add(temp);
        }
        #endregion
        #region Skin
        for (int i = 0; i < mouthMax; i++)
        {
            Texture2D temp = Resources.Load("Character/Mouth_" + i) as Texture2D;
            mouth.Add(temp);
        }
        #endregion
        #region Clothes
        for (int i = 0; i < clothesMax; i++)
        {
            Texture2D temp = Resources.Load("Character/Clothes_" + i) as Texture2D;
            clothes.Add(temp);
        }
        #endregion
        #region Armour
        for (int i = 0; i < armourMax; i++)
        {
            Texture2D temp = Resources.Load("Character/Armour_" + i) as Texture2D;
            armour.Add(temp);
        }
        #endregion
        #endregion
        characterRenderer = GameObject.FindGameObjectWithTag("CharacterMesh").GetComponent<Renderer>();
        #region Set Textures on Start
        SetTexture("Skin", 0);
        SetTexture("Hair", 0);
        SetTexture("Eyes", 0);
        SetTexture("Mouth", 0);
        SetTexture("Clothes", 0);
        SetTexture("Armour", 0);
        #endregion
        ChooseClass(0);
        PlayerPrefs.DeleteKey("Loaded");
    }
    private void Update()
    {
        statButton.GetComponentInChildren<Text>().text = "Points: " + statPoints;
        strButton.GetComponentInChildren<Text>().text = "Strength: " + (characterStats[0].value + characterStats[0].tempValue);
        dexButton.GetComponentInChildren<Text>().text = "Dexterity: " + (characterStats[1].value + characterStats[1].tempValue);
        conButton.GetComponentInChildren<Text>().text = "Constitution: " + (characterStats[2].value + characterStats[2].tempValue);
        intButton.GetComponentInChildren<Text>().text = "Intelligence: " + (characterStats[3].value + characterStats[3].tempValue);
        wisButton.GetComponentInChildren<Text>().text = "Wisdom: " + (characterStats[4].value + characterStats[4].tempValue);
        chaButton.GetComponentInChildren<Text>().text = "Charisma: " + (characterStats[5].value + characterStats[5].tempValue);

        characterName = nameField.text;
    }
    #endregion
    #region SetTexture
    public void SetTexture(string type, int dir)
    {
        int index = 0, max = 0, matIndex = 0;
        Texture2D[] textures = new Texture2D[0];
        #region Material and Values Switch
        switch (type)
        {
            #region Skin
            case "Skin":
                index = skinIndex;
                max = skinMax;
                textures = skin.ToArray();
                matIndex = 1;
                break;
            #endregion
            #region Hair
            case "Hair":
                index = hairIndex;
                max = hairMax;
                textures = hair.ToArray();
                matIndex = 2;
                break;
            #endregion
            #region Eyes
            case "Eyes":
                index = eyesIndex;
                max = eyesMax;
                textures = eyes.ToArray();
                matIndex = 3;
                break;
            #endregion
            #region Mouth
            case "Mouth":
                index = mouthIndex;
                max = mouthMax;
                textures = mouth.ToArray();
                matIndex = 4;
                break;
            #endregion
            #region Clothes
            case "Clothes":
                index = clothesIndex;
                max = clothesMax;
                textures = clothes.ToArray();
                matIndex = 5;
                break;
            #endregion
            #region Armour
            case "Armour":
                index = armourIndex;
                max = armourMax;
                textures = armour.ToArray();
                matIndex = 6;
                break;
                #endregion
        }
        #endregion
        #region Assign Direction
        index += dir;
        if (index < 0)
        {
            index = max - 1;
        }
        if (index > max - 1)
        {
            index = 0;
        }
        Material[] mat = characterRenderer.materials;

        mat[matIndex].mainTexture = textures[index];
        characterRenderer.materials = mat;
        #endregion
        #region Set Material Switch
        switch (type)
        {
            case "Skin":
                skinIndex = index;
                break;
            case "Hair":
                hairIndex = index;
                break;
            case "Eyes":
                eyesIndex = index;
                break;
            case "Mouth":
                mouthIndex = index;
                break;
            case "Clothes":
                clothesIndex = index;
                break;
            case "Armour":
                armourIndex = index;
                break;
        }
        #endregion
    }
    public void SetTexturePos(string type)
    {
        SetTexture(type, 1);
    }
    public void SetTextureNeg(string type)
    {
        SetTexture(type, -1);
    }
    public void ResetTexture()
    {
        skinIndex = 0;
        SetTexture("Skin", 0);
        hairIndex = 0;
        SetTexture("Hair", 0);
        eyesIndex = 0;
        SetTexture("Eyes", 0);
        mouthIndex = 0;
        SetTexture("Mouth", 0);
        clothesIndex = 0;
        SetTexture("Clothes", 0);
        armourIndex = 0;
        SetTexture("Armour", 0);
    }
    public void RandomTexture()
    {
        skinIndex = Random.Range(0, skinMax);
        SetTexture("Skin", 0);
        hairIndex = Random.Range(0, hairMax);
        SetTexture("Hair", 0);
        eyesIndex = Random.Range(0, eyesMax);
        SetTexture("Eyes", 0);
        mouthIndex = Random.Range(0, mouthMax);
        SetTexture("Mouth", 0);
        clothesIndex = Random.Range(0, clothesMax);
        SetTexture("Clothes", 0);
        armourIndex = Random.Range(0, armourMax);
        SetTexture("Armour", 0);
    }
    #endregion
    #region Stats
    public void StatPlus(int s)
    {
        if (statPoints > 0)
        {
            statPoints--;
            characterStats[s].tempValue++;
        }
    }
    public void StatMinus(int s)
    {
        if (statPoints < 10 && characterStats[s].tempValue > 0)
        {
            statPoints++;
            characterStats[s].tempValue--;
        }
    }
    public void ResetStats()
    {
        statPoints = 10;
        for (int s = 0; s < characterStats.Length; s++)
        {
            characterStats[s].tempValue = 0;
        }
    }
    #endregion

    public void ChooseClass(int classIndex)
    {
        switch (classIndex)
        {
            case 0:
                characterStats[0].value = 18;
                characterStats[1].value = 10;
                characterStats[2].value = 16;
                characterStats[3].value = 6;
                characterStats[4].value = 6;
                characterStats[5].value = 6;
                charClass = CharacterClass.Barbarian;
                break;
            case 1:
                characterStats[0].value = 6;
                characterStats[1].value = 16;
                characterStats[2].value = 6;
                characterStats[3].value = 10;
                characterStats[4].value = 6;
                characterStats[5].value = 18;
                charClass = CharacterClass.Bard;
                break;
            case 2:
                characterStats[0].value = 6;
                characterStats[1].value = 6;
                characterStats[2].value = 6;
                characterStats[3].value = 16;
                characterStats[4].value = 18;
                characterStats[5].value = 10;
                charClass = CharacterClass.Druid;
                break;
            case 3:
                characterStats[0].value = 6;
                characterStats[1].value = 18;
                characterStats[2].value = 6;
                characterStats[3].value = 10;
                characterStats[4].value = 16;
                characterStats[5].value = 6;
                charClass = CharacterClass.Monk;
                break;
            case 4:
                characterStats[0].value = 18;
                characterStats[1].value = 6;
                characterStats[2].value = 6;
                characterStats[3].value = 6;
                characterStats[4].value = 16;
                characterStats[5].value = 10;
                charClass = CharacterClass.Paladin;
                break;
            case 5:
                characterStats[0].value = 6;
                characterStats[1].value = 18;
                characterStats[2].value = 10;
                characterStats[3].value = 6;
                characterStats[4].value = 16;
                characterStats[5].value = 6;
                charClass = CharacterClass.Ranger;
                break;
            case 6:
                characterStats[0].value = 6;
                characterStats[1].value = 6;
                characterStats[2].value = 6;
                characterStats[3].value = 16;
                characterStats[4].value = 10;
                characterStats[5].value = 18;
                charClass = CharacterClass.Sorcerer;
                break;
            case 7:
                characterStats[0].value = 6;
                characterStats[1].value = 16;
                characterStats[2].value = 10;
                characterStats[3].value = 6;
                characterStats[4].value = 6;
                characterStats[5].value = 18;
                charClass = CharacterClass.Warlock;
                break;
        }
    }
    public void ChooseRace(int meshIndex)
    {
        raceIndex = meshIndex;
        switch(meshIndex)
        {
            case 0:
                charMesh.sharedMesh = humanMesh;
                    break;
            case 1:
                charMesh.sharedMesh = treeMesh;
                break;
        }
    }
    public void SaveCharacter()
    {
        PlayerPrefs.SetInt("SkinIndex", skinIndex);
        PlayerPrefs.SetInt("HairIndex", hairIndex);
        PlayerPrefs.SetInt("EyesIndex", eyesIndex);
        PlayerPrefs.SetInt("MouthIndex", mouthIndex);
        PlayerPrefs.SetInt("ClothesIndex", clothesIndex);
        PlayerPrefs.SetInt("ArmourIndex", armourIndex);

        PlayerPrefs.SetString("CharacterName", characterName);

        for (int i = 0; i < characterStats.Length; i++)
        {
            PlayerPrefs.SetInt(characterStats[i].name, (characterStats[i].value + characterStats[i].tempValue));
        }
        PlayerPrefs.SetInt("CharacterClass", selectedIndex);
        PlayerPrefs.SetInt("CharacterRace", raceIndex);
        PlayerPrefs.Save();
    }
    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
    private void OnGUI()
    {
        /*Vector2 scr = new Vector2(Screen.width / 16, Screen.height / 9);
        int i = 0;
        #region Skin
        if(GUI.Button(new Rect(0.25f*scr.x,0.5f*scr.y + (i * 0.5f* scr.y), 0.5f*scr.x,0.5f*scr.y),"<"))
        {
            SetTexture("Skin", -1);
        }
        GUI.Box(new Rect(0.75f * scr.x, 0.5f * scr.y + (i * 0.5f * scr.y), 1.5f * scr.x, 0.5f * scr.y), "Skin");
        if (GUI.Button(new Rect(2.25f * scr.x, 0.5f * scr.y + (i * 0.5f * scr.y), 0.5f * scr.x, 0.5f * scr.y), ">"))
        {
            SetTexture("Skin", +1);
        }
        i++;
        #endregion
        #region Hair
        if (GUI.Button(new Rect(0.25f * scr.x, 0.5f * scr.y + (i * 0.5f * scr.y), 0.5f * scr.x, 0.5f * scr.y), "<"))
        {
            SetTexture("Hair", -1);
        }
        GUI.Box(new Rect(0.75f * scr.x, 0.5f * scr.y + (i * 0.5f * scr.y), 1.5f * scr.x, 0.5f * scr.y), "Hair");
        if (GUI.Button(new Rect(2.25f * scr.x, 0.5f * scr.y + (i * 0.5f * scr.y), 0.5f * scr.x, 0.5f * scr.y), ">"))
        {
            SetTexture("Hair", +1);
        }
        i++;
        #endregion
        #region Eyes
        if (GUI.Button(new Rect(0.25f * scr.x, 0.5f * scr.y + (i * 0.5f * scr.y), 0.5f * scr.x, 0.5f * scr.y), "<"))
        {
            SetTexture("Eyes", -1);
        }
        GUI.Box(new Rect(0.75f * scr.x, 0.5f * scr.y + (i * 0.5f * scr.y), 1.5f * scr.x, 0.5f * scr.y), "Eyes");
        if (GUI.Button(new Rect(2.25f * scr.x, 0.5f * scr.y + (i * 0.5f * scr.y), 0.5f * scr.x, 0.5f * scr.y), ">"))
        {
            SetTexture("Eyes", +1);
        }
        i++;
        #endregion
        #region Mouth
        if (GUI.Button(new Rect(0.25f * scr.x, 0.5f * scr.y + (i * 0.5f * scr.y), 0.5f * scr.x, 0.5f * scr.y), "<"))
        {
            SetTexture("Mouth", -1);
        }
        GUI.Box(new Rect(0.75f * scr.x, 0.5f * scr.y + (i * 0.5f * scr.y), 1.5f * scr.x, 0.5f * scr.y), "Mouth");
        if (GUI.Button(new Rect(2.25f * scr.x, 0.5f * scr.y + (i * 0.5f * scr.y), 0.5f * scr.x, 0.5f * scr.y), ">"))
        {
            SetTexture("Mouth", +1);
        }
        i++;
        #endregion
        #region Clothes
        if (GUI.Button(new Rect(0.25f * scr.x, 0.5f * scr.y + (i * 0.5f * scr.y), 0.5f * scr.x, 0.5f * scr.y), "<"))
        {
            SetTexture("Clothes", -1);
        }
        GUI.Box(new Rect(0.75f * scr.x, 0.5f * scr.y + (i * 0.5f * scr.y), 1.5f * scr.x, 0.5f * scr.y), "Clothes");
        if (GUI.Button(new Rect(2.25f * scr.x, 0.5f * scr.y + (i * 0.5f * scr.y), 0.5f * scr.x, 0.5f * scr.y), ">"))
        {
            SetTexture("Clothes", +1);
        }
        i++;
        #endregion
        #region Armour
        if (GUI.Button(new Rect(0.25f * scr.x, 0.5f * scr.y + (i * 0.5f * scr.y), 0.5f * scr.x, 0.5f * scr.y), "<"))
        {
            SetTexture("Armour", -1);
        }
        GUI.Box(new Rect(0.75f * scr.x, 0.5f * scr.y + (i * 0.5f * scr.y), 1.5f * scr.x, 0.5f * scr.y), "Armour");
        if (GUI.Button(new Rect(2.25f * scr.x, 0.5f * scr.y + (i * 0.5f * scr.y), 0.5f * scr.x, 0.5f * scr.y), ">"))
        {
            SetTexture("Armour", +1);
        }
        i++;
        #endregion
        characterName = GUI.TextField(new Rect(0.25f * scr.x, 0.5f * scr.y + i * (0.5f * scr.y), 2.5f * scr.x, 0.5f * scr.y), characterName, 20);
        i++;
        if (GUI.Button(new Rect(0.25f * scr.x, 0.5f * scr.y + i * (0.5f * scr.y), 2.5f * scr.x, 0.5f * scr.y), "Save and Play"))
        {
            SaveCharacter();
            SceneManager.LoadScene(2);
        }
        #region Character Class
        i = 0;
        if (GUI.Button(new Rect(13.75f * scr.x, 0.5f * scr.y + (i * 0.5f * scr.y), 2 * scr.x, 0.5f * scr.y), classButton))
        {
            showDropdown = !showDropdown;
        }
        i++;
        if(showDropdown)
        {
            scrollPos = GUI.BeginScrollView(new Rect(13.75f * scr.x, 0.5f * scr.y + (i * 0.5f * scr.y), 2 * scr.x, 2f * scr.y), scrollPos, new Rect(0, 0, 0, selectedClass.Length * 0.5f * scr.y), false, true);
            for (int c = 0; c < selectedClass.Length; c++)
            {
                if (GUI.Button(new Rect(0, 0.5f * scr.y * c, 1.75f * scr.x, 0.5f * scr.y), selectedClass[c]))
                {
                    ChooseClass(c);
                    classButton = selectedClass[c];
                    showDropdown = false;
                }
            }

            GUI.EndScrollView();
        }
        GUI.Box(new Rect(12.75f * scr.x, 3.25f * scr.y, 2 * scr.x, 0.5f * scr.y), "Points: " + statPoints);
        statButton.GetComponentInChildren<Text>().text = "Points: " + statPoints;
        for (int s = 0; s < characterStats.Length; s++)
        {
            if (statPoints > 0)
            {
                if (GUI.Button(new Rect(12.25f * scr.x, 3.75f * scr.y + s * 0.5f * scr.y, 0.5f * scr.x, 0.5f * scr.y), "+"))
                {
                    statPoints--;
                    characterStats[s].tempValue++;
                }
            }
            GUI.Box(new Rect(12.75f * scr.x, 3.75f * scr.y + s * 0.5f * scr.y, 2 * scr.x, 0.5f * scr.y), characterStats[s].name + ": " + (characterStats[s].value + characterStats[s].tempValue));
            if (statPoints < 10 && characterStats[s].tempValue > 0)
            {
                if (GUI.Button(new Rect(14.75f * scr.x, 3.75f * scr.y + s * 0.5f * scr.y, 0.5f * scr.x, 0.5f * scr.y), "-"))
                {
                    statPoints++;
                    characterStats[s].tempValue--;
                }
            }
        }
        if (GUI.Button(new Rect(12.75f * scr.x, 6.75f * scr.y + i * 0.5f * scr.y, 2 * scr.x, 0.5f * scr.y), "Reset"))
        {
            statPoints = 10;
            for (int s = 0; s < characterStats.Length; s++)
            {
                characterStats[s].tempValue = 0;
            }
        }
        #endregion*/
    }
}

public enum CharacterClass
{
    Barbarian,
    Bard,
    Druid,
    Monk,
    Paladin,
    Ranger,
    Sorcerer,
    Warlock
}
