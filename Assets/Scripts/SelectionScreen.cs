using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class SelectionScreen : MonoBehaviour
    {
        public Player player;
        //public TextMesh playerName; will add this in the future
        public Text playerLevel;
        public AudioSource audioSource;

        // Use this for initialization
        void Start()
        {
            AudioManager.instance.enableAudioSource(audioSource);
            UIController.displayText(playerLevel, player.exp.ToString());
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