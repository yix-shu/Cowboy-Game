using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class MenuSystem : MonoBehaviour
    {
        public Player player;
        /*
        void Start()
        {
            Debug.Log(player.money);
            Debug.Log(player.exp);
        }
        */
        public void Play()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        public void Continue()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            PlayerData data = SaveSystem.LoadPlayer();

            player.money = data.money; 
            player.exp = data.exp;

            Debug.Log(player.money);
            Debug.Log(player.exp);
            
        }
    }
}