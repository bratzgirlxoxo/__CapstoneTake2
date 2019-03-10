using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class SphereRods : MonoBehaviour
{
    public GameObject rod;
    public float growthSpeed;

    private Vector3[] verts;
    private Vector3[] normals;
    private List<GameObject> rods = new List<GameObject>();
    private List<int> usedIndices = new List<int>();

    public AnimationCurve growthCurve;
    
    
    // Start is called before the first frame update
    void Start()
    {
        verts = GetComponent<MeshFilter>().mesh.vertices;
        Debug.Log(verts.Length);
        normals = GetComponent<MeshFilter>().mesh.normals;
        Debug.Log(normals.Length);
    }

    public void RodSpawn()
    {
        GameObject newRod = Instantiate(rod);
        newRod.transform.localScale = new Vector3(1, 0, 1);

        int vertIdx = 0;
        do
        {
            vertIdx = Random.Range(0, verts.Length);
        } while (usedIndices.Contains(vertIdx));
        
        
        
        usedIndices.Add(vertIdx);
        newRod.transform.position = transform.TransformPoint(verts[vertIdx]);
        newRod.transform.up = transform.TransformVector(normals[vertIdx]);

        StartCoroutine(RodGrow(newRod));
    }

    
    public IEnumerator RodGrow(GameObject growingRod)
    {
        float t = 0;

        while (t < 1)
        {
            growingRod.transform.localScale = new Vector3(0.5f, Mathf.LerpUnclamped(0, 1, growthCurve.Evaluate(t)), 0.5f);
            t += Time.deltaTime * growthSpeed;
            yield return 0;
        }
    }
    
}
