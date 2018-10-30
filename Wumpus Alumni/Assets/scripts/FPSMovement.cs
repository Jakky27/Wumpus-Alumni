using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FPSMovement : NetworkBehaviour
{
    public Rigidbody rb;

    private Animator anim; // Animation of the equippedGun

    public float speed = 10f;//target speed
    public float jumpForce = 250f;//force applied by jump

    public float sprintCoef = 2f;//how much faster sprinting makes the player move forward
    public float strafeCoef = 0.7f;//coef of sideways movement
    public float backCoef = 0.6f;//coef of backwards movement


    private Vector3 movement;
    [SerializeField]
    private bool isGrounded;//is the player on the ground?

    private float jumpCD = 0.25f;//delay between jumps, prevents double bouncing
    private float lastJump;//holds time of last jump

    GunController gunController;
    
    
	void Start ()
    {
        lastJump = Time.time;
        gunController = GetComponent<GunController>();
    }

    void Update ()
    {
        if (!GameController.isPaused)
        {
            if (isLocalPlayer == false)
            {
                return;
            }

            // TEMP
            anim = gameObject.GetComponent<GunController>().equippedGun.anim;


            movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            movement.Normalize();
            movement.x *= strafeCoef;
            if (movement.z < 0f)//backwards
            {
                movement.z *= backCoef;
            }
            else//forwards
            {
                movement.z *= checkSprint();
            }
            //Debug.Log(movement);

            checkGround();

            if (Input.GetButtonDown("Jump"))
            {
                jump();
            }

            if (movement == Vector3.zero)
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    void FixedUpdate()
    {
        if (!GameController.isPaused)
        {
            //transform.Translate(movement * speed * Time.deltaTime);
            if (isLocalPlayer == false)
            {
                return;
            }

            Vector3 localMovement = (transform.right * movement.x + transform.forward * movement.z);

            rb.MovePosition(transform.position + localMovement * speed * Time.deltaTime);


        }
    }

    void checkGround()//checks if player is on ground or acceptable slope
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        RaycastHit[] raycastHits = Physics.RaycastAll(ray);

        foreach (RaycastHit raycastHit in raycastHits)
        {
            // Ignore registering the raycast on the invisible shop colliders
            if (raycastHit.transform.tag.Equals("Ground"))
            {
                continue;
            }
            isGrounded = (raycastHit.distance > 0f && raycastHit.distance < 1.01f); //0.86min, higher value allows steeper slopes
            //Debug.Log(raycastHit.distance);//test height of player
        }
    }

    void jump()
    {
        if(Time.time>lastJump+jumpCD && isGrounded)
        {
            transform.Translate(Vector3.up / 10f);//small movement off floor, helps prevent sticking
            rb.AddForce(Vector3.up * jumpForce);
            
        }
    }

    float checkSprint()
    {
        // dont allow shooting or reloading and sprinting
        if (Input.GetButton("Fire1") == false && gunController.isReloading == false)
        {
            // If the playing is holding the shift buttion AND player is moving forward
            if (Input.GetButton("Sprint") && movement.z > 0f)
            {
                anim.SetBool("isSprinting", true);
                gunController.isSprinting = true;
                return sprintCoef;
            }
            else
            {
                gunController.isSprinting = false;
            }
            anim.SetBool("isSprinting", false);

            return 1f;
        }
        // Player is either firing or is reloading, so they're unable to sprint
        else
        {
            anim.SetBool("isSprinting", false);
            return 1f;
        }


    }

   

    
}
