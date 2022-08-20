using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class MenuSystem : MonoBehaviour
    {
        public PlayerData player;
        public AudioSource source;

        void Start()
        {
            AudioManager.instance.enableAudioSource(source);
        }
        public void Play()
        {
            AudioManager.instance.disableAudioSource(source);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);

            //setting player data
            GameMaster.instance.player.money = 0;
            GameMaster.instance.player.exp = 0;
            //GameMaster.instance.player.costumes[0] = true;
            //GameMaster.instance.player.costumes[1] = false;
            GameMaster.instance.player.outfitIndex = 0;
        }
        public void Continue()
        {
            AudioManager.instance.disableAudioSource(source);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            PlayerData data = SaveSystem.LoadPlayer();

            //loading player data
            GameMaster.instance.player.money = data.money;
            GameMaster.instance.player.exp = data.exp;
            //GameMaster.instance.player.costumes = data.costumes;
            GameMaster.instance.player.outfitIndex = data.outfitIndex;
        }
    }
}