using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereRodPlate : MonoBehaviour
{
    public SphereRods sculpture;
    public int index;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
            sculpture.RodSpawn();
        }
    }
}
