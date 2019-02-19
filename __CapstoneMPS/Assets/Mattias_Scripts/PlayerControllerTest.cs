using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTest : MonoBehaviour
{
    private Rigidbody rbody;
    private Vector3 input_vector;
    
    private float fwd;
    private float side;
    
    
    public float move_scale;
    public float float_scale;
    public float grav_scale = 0.25f;

    private bool canFloat;
    
    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        // get horizontal and vertical input
        side = Input.GetAxis("Horizontal");
        fwd = Input.GetAxis("Vertical");
        
        
        input_vector = transform.forward * fwd + transform.right * side;

        if (canFloat && Input.GetKeyDown(KeyCode.Space))
        {
            rbody.useGravity = !rbody.useGravity;

            if (!rbody.useGravity)
            {
                input_vector.y = 1f;
                Debug.Log("Floating");
            }
            else
            {
                input_vector.y = 0f;
            }
        }

        if (!rbody.useGravity && canFloat)
        {
            input_vector.y = 1f;
        }
        
    }

    void FixedUpdate()
    {
        Vector3 vel = rbody.velocity;
        vel.x = input_vector.x * move_scale;

        if (!rbody.useGravity)
            vel.y = input_vector.y * float_scale;
        else
            vel.y = grav_scale * Physics.gravity.y;
        vel.z = input_vector.z * move_scale;

        rbody.velocity = vel;
        
        Debug.Log(rbody.velocity);
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Ladder"))
        {
            canFloat = true;
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.CompareTag("Ladder"))
        {
            canFloat = false;
            rbody.useGravity = true;
        }
    }
}
