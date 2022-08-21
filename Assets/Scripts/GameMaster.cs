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

                //setting player data
                instance.player.money = 0;
                instance.player.exp = 0;
                //GameMaster.instance.player.costumes[0] = true;
                //GameMaster.instance.player.costumes[1] = false;
                instance.player.outfitIndex = 0;
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