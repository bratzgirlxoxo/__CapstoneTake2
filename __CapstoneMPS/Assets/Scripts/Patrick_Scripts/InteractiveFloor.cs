using UnityEngine;

namespace Patrick_Scripts
{
    public class InteractiveFloor : MonoBehaviour
    {
        public GameObject musicChip;
        public AudioClip[] inheritedElements;
        public Component[] sources;
        private AudioClip currentClip;

        private bool loaded;
    
        void Start()
        {
            musicChip = GameObject.FindGameObjectWithTag("MusicChip");
            sources = GetComponentsInChildren<AudioSource>();
            inheritedElements = musicChip.GetComponent<MusicChip>()
                .elements;

            loaded = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (!loaded)
            {
                foreach (AudioSource source in sources)
                {
                    currentClip = inheritedElements[Random.Range(0, inheritedElements.Length)];
                    source.clip = currentClip;
                    loaded = true;
                }
            }
        
        }
    }
}
