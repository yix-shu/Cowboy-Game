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
        }
        public void Continue()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            PlayerData data = SaveSystem.LoadPlayer();

            GameMaster.player.money = data.money;
            GameMaster.player.exp = data.exp;
        }
    }
}