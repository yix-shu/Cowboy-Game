using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData : MonoBehaviour
{
    public int exp;
    public int money;

    public PlayerData(Player player)
    {
        level = player.level;
        money = player.money;
    }
}
