using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObject : MonoBehaviour
{
    public Color[] colors = new Color[2];

    public bool activated;

    public float blinkFrequency;
    
    private float counter;
    private int ticker;

    private Material mat;

    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        mat.color = colors[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            counter += Time.deltaTime;
        }
        else
        {
            counter = 0;
            mat.color = colors[0];
        }

        if (activated && counter >= blinkFrequency)
        {
            counter = 0f;
            ticker++;
            mat.color = colors[ticker % 2];
        }
        GetComponent<MeshRenderer>().material = mat;
    }
}
