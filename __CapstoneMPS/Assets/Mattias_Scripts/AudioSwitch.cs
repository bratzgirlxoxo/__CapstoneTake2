using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSwitch : MonoBehaviour
{

    public GameObject AudioObj;
    
    private AudioSource audioSrc;

    private bool canPlaySound;
    private bool soundPlaying;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canPlaySound && Input.GetKeyDown(KeyCode.E))
        {
            soundPlaying = !soundPlaying;

            if (soundPlaying)
            {
                audioSrc.Play();
                AudioObj.GetComponent<AudioObject>().activated = true;
                Debug.Log("Activating");
            }
            else
            {
                audioSrc.Stop();
                AudioObj.GetComponent<AudioObject>().activated = false;
                Debug.Log("Deactivating");
            }
        }
        
    }

    void OnTriggerEnter(Collider coll)
    {
        GameObject otherObj = coll.gameObject;

        if (otherObj.CompareTag("Player"))
        {
            Debug.Log("In Range");
            canPlaySound = true;
        }
    }

    void OnTriggerExit(Collider coll)
    {
        canPlaySound = false;
    }
}
