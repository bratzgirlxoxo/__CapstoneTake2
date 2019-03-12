using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem1 : MonoBehaviour
{

    public GameObject firstFloor;

    public float minFirstFloorTime;
    public float maxFirstFloorTime;

    private float firstFloorTime;

    private float entranceTime = 100000000000;

    [HideInInspector] public GameObject risingTile;
    
    // Start is called before the first frame update
    void Start()
    {
        firstFloorTime = Random.Range(minFirstFloorTime, maxFirstFloorTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - entranceTime >= firstFloorTime)
        {
            
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        entranceTime = Time.time;
    }
}
