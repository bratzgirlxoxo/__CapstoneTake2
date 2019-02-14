using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// usage: this script should be put on the player game object
// intent: this script controls basic movement.
// it does not cover other core mechanics like popping things
public class PlayerController : MonoBehaviour
{

	private Rigidbody rbody;
	private Vector3 input_vector;
	private Vector3 move_dir;
	private bool can_climb;
	private bool facing_ladder;
	private bool on_flatground = true;
	private bool on_ground;
	private float fwd;
	private float side;

	public float move_scale;
	public float grav_scale = 0.25f;
	public float rotate_scale = 1f;
	public float jump_strength = 1;
	public float grav_up = -30f;
	public float grav_down = 9.8f;
	public Material ledge_mat;
	
	
	// Use this for initialization
	void Start ()
	{
		rbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
		// check to see whether the player is on flatground or not
		if (transform.up == new Vector3(0, 1, 0))
		{
			on_flatground = true;
		}
		else
		{
			on_flatground = false;
		}
		
		
		// get horizontal and vertical input
		side = Input.GetAxis("Horizontal");
		fwd = Input.GetAxis("Vertical");
		
		//AdjustAngle();
		if (can_climb)
			ClimbLadder();

		on_ground = CheckGround();
		
		if (!can_climb)
			input_vector = transform.forward * fwd + transform.right * side; 
		
		
		// jump
		/*
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Jump();
		}
		*/
	}
	
	void FixedUpdate()
	{
		// override object's velocity with desired inputVector direction
		if (GetComponent<Rigidbody>().velocity.y > 0.5f)
		{
			Physics.gravity = new Vector3(0, -grav_up, 0);
			//Debug.Log("Gravityy?");
		}
		else
		{
			Physics.gravity = new Vector3(0, -grav_down, 0);
			//Debug.Log("Weirrdd");
		}

		if (on_flatground)
		{
			Vector3 vel = rbody.velocity;
			vel.x = input_vector.x * move_scale;
			
			// determines how to apply velocity
			// if near ladder, apply up, else, forward
			if (can_climb && on_ground && input_vector.y < 0)
			{
				vel.z = -input_vector.y * move_scale;
			}
			else if (can_climb)
			{
				vel.y = input_vector.y * move_scale;
				Debug.Log("floating?");
			}
			else
			{
				vel.z = input_vector.z * move_scale;
			}
			rbody.velocity = vel;
		}
		else
		{
			GetComponent<Rigidbody>().velocity = input_vector * move_scale;
		}		
	}

	void Jump()
	{
		GetComponent<Rigidbody>().AddForce(transform.up * jump_strength, ForceMode.Impulse);
	}

	void ClimbLadder()
	{
		Ray ladder_ray = new Ray(transform.position, transform.forward);

		float max_raycast_dist = 1f;
		
		Debug.DrawRay(ladder_ray.origin, ladder_ray.direction * max_raycast_dist, Color.yellow);
		
		RaycastHit ray_hit = new RaycastHit();

		bool rcast = Physics.Raycast(ladder_ray, out ray_hit, max_raycast_dist);

		if (!rcast)
		{
			can_climb = false;
		}
		else
		{
			can_climb = true;
		}
		
		if (can_climb && fwd != 0)
		{
			input_vector = transform.up * fwd + transform.right * side;
		}
	}

	bool CheckGround()
	{
		Ray ground_ray = new Ray(transform.position, -transform.up);

		float max_raycast_dist = 1.1f;
		
		Debug.DrawRay(ground_ray.origin, ground_ray.direction * max_raycast_dist, Color.blue);

		bool rcast = Physics.Raycast(ground_ray, max_raycast_dist);

		if (rcast)
		{
			return true;
		}

		return false;
	}
	
	

	void AdjustAngle()
	{
		Ray angle_ray = new Ray(transform.position, -transform.up);

		float max_raycast_dist = 10f;
		
		RaycastHit ray_hit = new RaycastHit();

		bool rcast = Physics.Raycast(angle_ray, out ray_hit, max_raycast_dist);

		if (rcast)
		{
			transform.up = ray_hit.normal;
			Debug.Log("Dist: " + ray_hit.distance);
			if (ray_hit.distance > 0.4f)
			{
				rbody.AddForce(-transform.up * 20f);
			}
		}
	}

	
	void OnTriggerEnter(Collider coll)
	{
		GameObject other = coll.gameObject;
		if (other.CompareTag("Ladder"))
		{
			Debug.Log("-- Player is near Ladder --");
			can_climb = true;
			rbody.useGravity = false;
		}
	}

	void OnTriggerExit(Collider coll)
	{
		GameObject other = coll.gameObject;
		if (other.CompareTag("Ladder"))
		{
			Debug.Log("off ladder");
			can_climb = false;
			rbody.useGravity = true;
		}
	}
	
}
