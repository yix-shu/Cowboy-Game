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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
            GameMaster.instance.player.money = 0;
            GameMaster.instance.player.exp = 0;
            GameMaster.instance.player.costumes[0] = true;
            GameMaster.instance.player.costumes[1] = false;
            //GameMaster.instance.player.outfit = defaultOutfit;
        }
        public void Continue()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            PlayerData data = SaveSystem.LoadPlayer();

            GameMaster.instance.player.money = data.money;
            GameMaster.instance.player.exp = data.exp;
            GameMaster.instance.player.costumes = data.costumes;
            GameMaster.instance.player.outfit = data.outfit;
        }
    }
}