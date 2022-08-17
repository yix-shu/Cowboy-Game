﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class SelectionScreen : MonoBehaviour
    {
        public Player player;
        //public TextMesh playerName; will add this in the future
        public Text playerLevel;

        // Use this for initialization
        void Start()
        {
            UIController.displayText(playerLevel, player.exp.ToString());
        }

        public void Back()
        {
            UIController.switchScene(-1);
        }
        public void DuelSelect()
        {
            UIController.switchScene(1);
        }
    }
}