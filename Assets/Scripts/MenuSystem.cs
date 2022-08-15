using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class MenuSystem : MonoBehaviour
    {
        public Player player;

        public void Play()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            GameMaster.instance.player.money = 0;
            GameMaster.instance.player.exp = 0;
        }
        public void Continue()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            PlayerData data = SaveSystem.LoadPlayer();

            GameMaster.instance.player.money = data.money;
            GameMaster.instance.player.exp = data.exp;
            GameMaster.instance.player.costumes = data.costumes;
        }
    }
}