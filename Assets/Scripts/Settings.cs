﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Settings : MonoBehaviour
    {

        public Canvas optionsCanvas;
        public Text saveMessage;

        // Use this for initialization
        void Start()
        {
            optionsCanvas.enabled = false;
        }

        public void clickedOptions()
        {
            UIController.opencloseCanvas(optionsCanvas);
            UIController.displayText(saveMessage, ""); //clearing messages, might want to create new method for this
        }

        public void clickedMenu()
        {
            UIController.switchScene(-1);
        }

        public void saveGame()
        {
            SaveSystem.SavePlayer(GameMaster.instance.player);
            UIController.displayText(saveMessage, "Saved!");
        }
    }
}