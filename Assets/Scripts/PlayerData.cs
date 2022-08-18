using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public int exp;
    public int money;
    //public bool[] costumes;
    //public Sprite outfit;

    public PlayerData(Player player)
    {
        exp = player.exp;
        money = player.money;
        //costumes = player.costumes;
        //outfit = player.outfit;
    }
}

