using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public int exp;
    public int money;

    public PlayerData(Player player)
    {
        exp = player.exp;
        money = player.money;
    }
}
