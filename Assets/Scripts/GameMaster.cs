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
                DontDestroyOnLoad(gameObject);
                instance = this;
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