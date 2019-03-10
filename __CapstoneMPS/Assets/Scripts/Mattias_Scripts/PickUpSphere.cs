using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSphere : MonoBehaviour
{

    public float rayDist;
    //public Material orbMat;

    public Vector3 orbHoldPos;

    private Transform parentCapsule;
    private Transform orbTransform;
    private bool canPickUp;
    private bool holding;
    
    
    // Start is called before the first frame update
    void Start()
    {
        parentCapsule = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        Ray camRay = new Ray(transform.position, transform.forward);
        
        RaycastHit rayHit = new RaycastHit();
        
        Debug.DrawRay(camRay.origin, camRay.direction, Color.green, rayDist);

        if (Physics.Raycast(camRay, out rayHit, rayDist))
        {
            if (rayHit.collider.CompareTag("Orb"))
            {
                //orbMat.SetFloat("_OrbGlowAlpha", 1);
                orbTransform = rayHit.transform;
                canPickUp = true;
            }
            else
            {
                //orbMat.SetFloat("_OrbGlowAlpha", 1);
                orbTransform = null;
                canPickUp = false;
            }
        }

        if (canPickUp && Input.GetKeyDown(KeyCode.E) && orbTransform != null)
        {
            holding = true;
            orbTransform.parent = parentCapsule;
            orbTransform.localPosition = orbHoldPos;
        }
    }
}
