﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public static class UIController 
    {

        public static void changeCanvas(Canvas currentCanvas, Canvas newCanvas)
        {
            newCanvas.enabled = true;
            currentCanvas.enabled = false;
        }

        public static void opencloseCanvas(Canvas newCanvas)
        {
            newCanvas.enabled = !newCanvas.enabled;
        }
        public static void switchScene(int forwardOrBackward)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + forwardOrBackward);
        }
    }
}