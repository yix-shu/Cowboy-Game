using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1NPC : Unit
{
    string[] choices = { "Shoot", "Reload", "Hold" };
    bool reloaded = false;

    public void onTurn()
    {
        if (reloaded)
        {
            int index = Random.Range(choices.Length);
        }
        else
        {
            int index = Random.Range(1, choices.Length);
        }
    }
}
