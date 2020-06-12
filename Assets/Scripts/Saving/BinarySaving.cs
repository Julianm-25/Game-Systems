using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class BinarySaving
{
    public static void SavePlayerData(PlayerHandler player)
    {
        //Refernce bf
        BinaryFormatter formatter = new BinaryFormatter();
        //location to save
        string path = Application.persistentDataPath + "/" + "InfantileFeline" + ".jpeg";
        //create file
        FileStream stream = new FileStream(path, FileMode.Create);
        //choose data
        PlayerData data = new PlayerData(player);
        //write and convert to bytes
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayerData(PlayerHandler player)
    {
        //location to save
        string path = Application.persistentDataPath + "/" + "InfantileFeline" + ".jpeg";
        //if file exists
        if (File.Exists(path))
        {
            //get bf
            BinaryFormatter formatter = new BinaryFormatter();
            //read from path
            FileStream stream = new FileStream(path, FileMode.Open);
            //set values based on save
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            //send useable data back to saving script
            return data;
        }
        else
        {
            return null;
        }
    }
}
