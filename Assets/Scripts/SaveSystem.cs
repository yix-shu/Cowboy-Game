using UnityEngine;
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

        }
    }
}