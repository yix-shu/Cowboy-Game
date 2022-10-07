using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;

    public int maxHP;
    public int currentHP;
    public bool reloaded = false;

    public int ammoLeft = 10;
    //comment
    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        return (currentHP <= 0);
    }
    public void Reload()
    {
        reloaded = true;
        ammoLeft -= 1;
    }
    public void defaultProperties()
    {
        unitName = "Player";
        maxHP = 10;
        currentHP = 2;
        ammoLeft = 10;
    }
}
