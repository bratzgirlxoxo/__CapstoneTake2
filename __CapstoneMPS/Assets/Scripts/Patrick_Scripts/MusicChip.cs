using UnityEngine;

namespace Patrick_Scripts
{
    public class MusicChip : MonoBehaviour
    {

        public AudioClip support;
        public AudioClip[] elements;
        private AudioSource chipSource;

        void Start()
        {
            chipSource = GetComponent<AudioSource>();
            chipSource.clip = support;
            chipSource.loop = true;
            chipSource.Play();
        }

    }
}
