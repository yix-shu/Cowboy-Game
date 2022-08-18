using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class SelectionScreen : MonoBehaviour
    {
        //public TextMesh playerName; will add this in the future
        public Text playerLevel;
        public AudioSource audioSource;

        // Use this for initialization
        void Awake()
        {
            AudioManager.instance.enableAudioSource(audioSource);
            UIController.displayText(playerLevel, "XP: " + GameMaster.instance.player.exp.ToString());
        }

        public void Back()
        {
            AudioManager.instance.disableAudioSource(audioSource);
            UIController.switchScene(-1);
        }
        public void DuelSelect()
        {
            AudioManager.instance.disableAudioSource(audioSource);
            UIController.switchScene(1);
        }
    }
}