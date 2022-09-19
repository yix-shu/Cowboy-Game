using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

namespace Assets.Scripts
{
    public class SelectionScreen : MonoBehaviour
    {
        //public TextMesh playerName; will add this in the future
        public Text playerLevel;
        public AudioSource audioSource;
        public Transform spawn;

        public Button pvp;
        public Button computer;

        GameObject playerPrefab = OutfitManager.instance.playerOutfit;

        // Use this for initialization
        void Awake()
        {
            AudioManager.instance.enableAudioSource(audioSource);
            UIController.displayText(playerLevel, "XP: " + GameMaster.instance.player.exp.ToString());
            GameObject playerGO = Instantiate(playerPrefab, spawn);

            pvp.gameObject.SetActive(false);
            computer.gameObject.SetActive(false);
        }

        public void Back()
        {
            AudioManager.instance.disableAudioSource(audioSource);
            UIController.switchScene(-1);
        }
        public void DuelSelect()
        {
            pvp.gameObject.SetActive(!pvp.gameObject.activeSelf);
            computer.gameObject.SetActive(!computer.gameObject.activeSelf);
        }
        public void ComputerSelect()
        {
            AudioManager.instance.disableAudioSource(audioSource);
            UIController.switchScene(1);
        }
    }
}