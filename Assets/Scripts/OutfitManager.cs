using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class OutfitManager : MonoBehaviour
    {
        public static OutfitManager instance;
        public GameObject[] outfits;
        public GameObject playerOutfit;
        // Use this for initialization
        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                DontDestroyOnLoad(instance.playerOutfit);
                instance.playerOutfit = outfits[0];
            }
            else
            {
                Destroy(gameObject);
                return;
            } 
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}