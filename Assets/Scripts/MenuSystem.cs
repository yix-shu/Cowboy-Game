using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

namespace Assets.Scripts
{
    public class MenuSystem : MonoBehaviour
    {
        public PlayerData player;
        public AudioSource source;
        public Text errorText;

        void Start()
        {
            AudioManager.instance.enableAudioSource(source);
        }
        public void Play()
        {
            AudioManager.instance.disableAudioSource(source);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
        public void Continue()
        {
            PlayerData data = SaveSystem.LoadPlayer();
            if (data == null)
            {
                GameMaster.instance.player.money = 0;
                GameMaster.instance.player.exp = 0;
                //GameMaster.instance.player.costumes = data.costumes;
                GameMaster.instance.player.outfitIndex = 0;
                errorText.gameObject.SetActive(true);
                return;
            }
            AudioManager.instance.disableAudioSource(source);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            //loading player data
            GameMaster.instance.player.money = data.money;
            GameMaster.instance.player.exp = data.exp;
            //GameMaster.instance.player.costumes = data.costumes;
            GameMaster.instance.player.outfitIndex = data.outfitIndex;
        }
    }
}