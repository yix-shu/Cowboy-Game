using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        void Awake()
        {
            Debug.Log("Hi");
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }
        public void changeMusic(AudioSource source, AudioClip audio)
        {
            source.clip = audio;
        }
        public void disableAudioSource(AudioSource source)
        {
            source.enabled = false;
        }
        public void enableAudioSource(AudioSource source)
        {
            source.enabled = true;
        }
    }
}