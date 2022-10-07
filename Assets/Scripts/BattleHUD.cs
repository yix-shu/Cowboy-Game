using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Slider hpSlider;

    //add ammo display
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
            bullets[i].sprite = fullBullet;
        }
    }
    public void updateBullets()
    {
        ammoLeft -= 1;
        for (int i = 0; i < bullets.Length; i++)
        {
            if (i <= ammoLeft)
            {
                bullets[i].sprite = fullBullet;
            }
            else
            {
                bullets[i].sprite = emptyBullet;
            }
        }
    }

    public void updateBullets(Unit unit)
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (i <= unit.ammoLeft)
            {
                bullets[i].sprite = fullBullet;
            }
            else
            {
                bullets[i].sprite = emptyBullet;
            }
        }
    }

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
    }
    public void updateHP(int hp)
    {
        hpSlider.value = hp;
    }
}
