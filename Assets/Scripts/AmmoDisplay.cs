using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
    public int ammoLeft = 10;

    public Image[] bullets;
    public Sprite fullBullet;
    public Sprite emptyBullet;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
