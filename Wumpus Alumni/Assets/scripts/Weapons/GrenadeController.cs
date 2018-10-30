using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GrenadeController : NetworkBehaviour
{

    public GameObject grenadePrefab;
    public GunController gunController;
    public bool canThrowGrenade = true;

    public int maxGrenades = 2;
    public int grenades = 2;

    private Animator anim;
    private Camera cam;

    //private PauseManager pause;
    //private bool isPaused;


    void Start ()
    {
        anim = gameObject.GetComponent<Animator>();
        cam = gameObject.GetComponentInChildren<Camera>();
        gunController = gameObject.GetComponent<GunController>();
        //pause = gameObject.GetComponent<PauseManager>();

    }

    void Update ()
    {
        //isPaused = pause.isPaused;
        if (isLocalPlayer == false)
        {
            return;
        }
        /*if(isPaused)
        {
            canThrowGrenade = false;
        }*/
        else
        {
            canThrowGrenade = true;
        }
        // GRENADE THROW
        if (Input.GetButtonDown("Grenade") && gunController.cannotShoot == false && grenades > 0 && !Input.GetButton("Fire1") && canThrowGrenade == true)
        {
            StartCoroutine(throwGrenade());
        }
    }

    IEnumerator throwGrenade()
    {
        gunController.equippedGun.anim.SetTrigger("grenade");
        grenades--;
        canThrowGrenade = false;
        yield return new WaitForSeconds(0.35f);

        canThrowGrenade = true;
        CmdThrowGrenade();
    }

    [Command]
    void CmdThrowGrenade()
    {
        RpcThrowGrenade();
    }

    [ClientRpc]
    void RpcThrowGrenade()
    {
        GameObject grenade = Instantiate(grenadePrefab, cam.transform.position + cam.transform.forward, transform.rotation);
        grenade.GetComponent<Grenade>().player = transform.root.GetComponent<PlayerController>();
        Rigidbody grenadeRB = grenade.GetComponent<Rigidbody>();
        grenadeRB.AddForce(cam.transform.forward * 600f);
    }
}
