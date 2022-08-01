using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Settings : MonoBehaviour
    {

        public Canvas optionsCanvas;
        public Player player;

        // Use this for initialization
        void Start()
        {
            optionsCanvas.enabled = false;
        }

        public void clickedOptions()
        {
            UIController.opencloseCanvas(optionsCanvas);
        }

        public void clickedMenu()
        {
            UIController.switchScene(-1);
        }

        public void saveGame()
        {
            SaveSystem.SavePlayer(player);
        }
    }
}