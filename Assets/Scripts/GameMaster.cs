using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameMaster : MonoBehaviour
    {
        public static GameMaster instance;
        public Player player;
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                Debug.Log("RAWR");
                Debug.Log(instance.player.exp);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                Debug.Log("meow");
                Debug.Log(instance.player.exp);
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}