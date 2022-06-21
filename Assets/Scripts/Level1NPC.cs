using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1NPC : Unit
{
    string[] choices = { "Shoot", "Hold", "Reload"};

    public string enemyChoose()
    {
        int index = 2;
        if (reloaded)
        {
            index = Random.Range(0, 2);
        }
        else
        {
            index = Random.Range(1, 3);
        }
        reloaded = (choices[index] == "Reload");
        return choices[index];
    }
}
