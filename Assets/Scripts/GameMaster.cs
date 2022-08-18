using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameMaster : MonoBehaviour
    {
        public static GameMaster instance;
        public PlayerData player;
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                instance.player.money = 0;
                instance.player.exp = 0;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}