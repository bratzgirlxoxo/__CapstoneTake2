using UnityEngine;

namespace Patrick_Scripts
{
    public class FloorElement : MonoBehaviour
    {
        public AudioSource source;

        private void Start()
        {
            source = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
            
            }
        
            source.Play();
            Debug.Log("stepped");
        }
    }
}
