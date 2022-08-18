﻿using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; //Binary Formatter for translating data

namespace Assets.Scripts
{
    public static class SaveSystem   
    {
        public static void SavePlayer(Player player)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/player.fun"; //Application.persistentDataPath uses the OS's system file path
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerData data = new PlayerData(player);

            formatter.Serialize(stream, data);
            stream.Close();

            Debug.Log("Saved!");
            Debug.Log(path);
        }

        public static PlayerData LoadPlayer()
        {
            string path = Application.persistentDataPath + "/player.fun";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                stream.Close();

                return data;
            }
            else
            {
                Debug.LogError("Save file not found in " + path);
                return null;
            }
        }
    }
}