using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;

    public int maxHP;
    public int currentHP;
    //comment
    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        return (currentHP <= 0);
    }
    
}
