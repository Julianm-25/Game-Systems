﻿[System.Serializable]
public class PlayerData 
{
    //Data....Get from Game
    public string playerName;
    public int level;
    public float pX, pY, pZ;
    public float rX, rY, rZ, rW;
    public string checkPoint;
    public float currentExp, neededExp, maxExp;
    public int[] stats = new int[6];
    public float[] currentAttributes = new float[3];
    public float[] maxAttributes = new float[3];
    public int playerClass, playerRace, skinTexture, hairTexture, eyeTexture, mouthTexture, clothesTexture, armourTexture;

    public PlayerData(PlayerHandler player)
    {
        playerName = player.name;
        level = player.level;
        checkPoint = player.currentCheckPoint.name;
        currentExp = player.currentExp;
        neededExp = player.neededExp;
        maxExp = player.maxExp;
        //Position
        pX = player.transform.position.x;
        pY = player.transform.position.y;
        pZ = player.transform.position.z;
        //Rotation
        rX = player.transform.rotation.x;
        rY = player.transform.rotation.y;
        rZ = player.transform.rotation.z;
        rW = player.transform.rotation.w;
        //Array
        for (int i = 0; i < currentAttributes.Length; i++)
        {
            currentAttributes[i] = player.attributes[i].currentValue;
            maxAttributes[i] = player.attributes[i].maxValue;
        }
        for (int i = 0; i < stats.Length; i++)
        {
            stats[i] = player.characterStats[i].value + player.characterStats[i].tempValue;
        }
        //Customisation
        playerClass = player.playerClass;
        playerRace = player.playerRace;
        skinTexture = player.skinTexture;
        hairTexture = player.hairTexture;
        eyeTexture = player.eyeTexture;
        mouthTexture = player.mouthTexture;
        clothesTexture = player.clothesTexture;
        armourTexture = player.armourTexture;
    }
}
