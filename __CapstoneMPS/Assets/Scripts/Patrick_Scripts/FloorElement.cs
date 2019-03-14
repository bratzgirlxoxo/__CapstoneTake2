using UnityEngine;

namespace Patrick_Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class FloorElement : MonoBehaviour
    {
        public AudioSource source;

        public ParticleSystem particleLights;

        public Totem1 firstTotem;

        private void Start()
        {
            source = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {

            //firstTotem.risingTile = transform.gameObject;
            
            if (particleLights.isPlaying)
            {
                particleLights.Stop();
            }
            else
            {
                particleLights.Play();
            }
            
            source.Play();
            Debug.Log("stepped");
        }
    }
}
