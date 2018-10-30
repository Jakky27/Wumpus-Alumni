using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MouseLook : NetworkBehaviour
{
    PlayerController player; 

    private Rigidbody rb;

    private float xRotation;
    private float yRotation;

    private float xRotSmooth;
    private float yRotSmooth;

    public float sensitivity;
    public float smoothing;

	void Start ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = gameObject.GetComponentInParent<Rigidbody>();
        player = gameObject.GetComponentInParent<PlayerController>();

    }
	
	void Update ()
    {
        if (!GameController.isPaused)
        {
            if (player.isLocalPlayer == false)
            {
                return;
            }

            xRotation -= Input.GetAxis("Mouse Y") * sensitivity;
            yRotation += Input.GetAxis("Mouse X") * sensitivity;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            xRotSmooth = Mathf.Lerp(xRotSmooth, xRotation, Time.smoothDeltaTime * smoothing);
            yRotSmooth = Mathf.Lerp(yRotSmooth, yRotation, Time.smoothDeltaTime * smoothing);


            transform.rotation = Quaternion.Euler(xRotSmooth, yRotSmooth, 0f);
            rotateBody();
        }
	}

    void rotateBody()
    {
        Quaternion bodyRot = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        rb.rotation = bodyRot;
    }

    public void recoil(Vector2 amount) //x is vertical recoil, y is horizontal recoil (all in degrees)
    {
        xRotation -= amount.x;

        int rndSign = Random.Range(0, 2) * 2 - 1; //either returns 1 or -1, indicating positive or negative signs

        yRotation += amount.y * rndSign; //horizontal recoil can go in either direction
    }
}
