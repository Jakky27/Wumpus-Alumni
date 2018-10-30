using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Collections;

public class GunController : NetworkBehaviour
{
    private float lastShot;//time of last shot
    public bool cannotShoot = false; // Used for weapon switching/equipping and reloading
    public bool isReloading = false; // Used for only reloading
    public bool isSprinting = false;

    bool interruptShotgun = false;

    private MouseLook mouseLook;
    private Camera cam;
    private Animator anim; // Animation of the player that is irrelevant to any guns (maybe?)

    // Objects found inside the child gameObjects
    public AudioSource[] equippedGunSounds; //0 gunshot | 1 reload |
    public ParticleSystem[] equippedGunParticleSystems; //0 muzzleFlash | 1 bullet |

    // Sounds player uses, regardless of things like equipped guns 
    public AudioSource[] playerSounds; // 0 WeaponSwitch || 1 WeaponEquipped || 2 Out of ammo click || 3 Bullet whiz || 

	public bool[] isGunUnlocked;//determines whether a gun can be switched to, unlockable via weapon shop

    public List<Gun> guns; // Player's inventory of guns. The first Gun is the equippedGun
    public Gun equippedGun; // Player's current equipped gun
    public int equippedGunIndex = 0;

    void Awake()
    {
        guns = new List<Gun>();
		isGunUnlocked = new bool[5];
        playerSounds = transform.Find("Player Sounds").GetComponentsInChildren<AudioSource>();
    }

    void Start()
    {
        // TEMP - currently adds all the guns in the player's child 
        foreach (Gun gun in GetComponentsInChildren<Gun>())
        {
            guns.Add(gun);
        }
        equippedGun = guns[0]; // Grabs the first gun in the player's children
        // TEMP - turns off every other gun
        for (int i = 1; i < guns.Count; i++)
        {
            guns[i].gameObject.SetActive(false);
        }

		isGunUnlocked [0] = true;
		for (int i = 1; i < isGunUnlocked.Length; i++) 
		{
			isGunUnlocked [i] = false;
		}

        lastShot = 0f;

        cam = gameObject.GetComponentInChildren<Camera>();
        mouseLook = gameObject.GetComponentInChildren<MouseLook>();

        refreshEquippedGun();
    }

    void Update()
    {
        if (!GameController.isPaused && !WeaponShop.isInShop)
        {
            if (isLocalPlayer == false)
            {
                return;
            }

            // OUT OF AMMO SOUND
            if (equippedGun.roundsLeft <= 0 && Input.GetButtonDown("Fire1") && Time.time > lastShot + equippedGun.convertedFireRate)
            {
                playerSounds[2].Play();
            }

            // SHOOTING
            if (equippedGun.roundsLeft > 0 && equippedGun.isAuto && cannotShoot == false && Input.GetButton("Fire1") && Time.time > lastShot + equippedGun.convertedFireRate && !GameController.isPaused)//if bullets remain, not reloading and input and can shoot
            {
                shoot();
                CmdShoot();
            }
            //SEMI AUTO SHOOTING
            if (equippedGun.roundsLeft > 0 && !equippedGun.isAuto && cannotShoot == false && Input.GetButtonDown("Fire1") && Time.time > lastShot + equippedGun.convertedFireRate)//if bullets remain, not reloading and input and can shoot
            {
                shoot();
                CmdShoot();
            }
            // RELOAD
            if (equippedGun.roundsLeft < equippedGun.clipSize && Input.GetButtonDown("Reload") && !cannotShoot && equippedGun.ammoLeft > 0 && isReloading == false && Time.time > lastShot + equippedGun.convertedFireRate)
            {
                cannotShoot = true;
                CmdReload();
            }
            // AIMING 
            if (Input.GetButton("Fire2"))//ads
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, equippedGun.zoomFOV, Time.smoothDeltaTime * 7f);
            }
            else//if not ads return to normal fov
            {
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, equippedGun.unZoomFOV, Time.smoothDeltaTime * 7f);
            }
            // SWITCHING WEAPONS
            // Cycles weapons, making the first weapon in the list of guns the equippedGun
            if (Input.mouseScrollDelta.y != 0 && cannotShoot == false && isSprinting == false)
            {
                if (Input.mouseScrollDelta.y > 0)
                {
                    // Create switching animation for previous gun, and creates a delay
                    CmdSwitchWeapon(true);
                }
                else if (Input.mouseScrollDelta.y < 0)
                {
                    // Create switching animation for previous gun, and creates a delay
                    CmdSwitchWeapon(false);
                }
            }
        }
    }

    public void UnlockWeapon(int w)//w = the weapon unlocked, 1 = smg, 2 = shotgun, 3 = scout rifle, 4 = assault rifle
    {
        isGunUnlocked[w] = true;
    }


    // Refreshes properites because of a new equipped gun 
    private void refreshEquippedGun()
    {
        equippedGun.convertedFireRate = 1f / (equippedGun.fireRate / 60f);
        equippedGunParticleSystems = equippedGun.gameObject.GetComponentsInChildren<ParticleSystem>();
        equippedGunSounds = equippedGun.gameObject.GetComponents<AudioSource>();
    }

    void shoot()
    {
        lastShot = Time.time;
        equippedGun.roundsLeft--;

        mouseLook.recoil(equippedGun.recoil);//apply recoil
    }

    // As the client, it tells the server that it shot
    // The command attribute tells the server to run whatever is in this method
    [Command]
    void CmdShoot()
    {
        RpcShoot();
    }

    // Tells every client to do this. Only the server runs this. 
    [ClientRpc]
    void RpcShoot()
    {
        equippedGun.anim.SetTrigger("recoil");
        equippedGunSounds[0].Play();
        equippedGunParticleSystems[0].Emit(1); // Muzzle
        // Shotguns get to shoot a burst of particles
        if(equippedGun.isShotugn == true)
        {
            equippedGunParticleSystems[1].Emit(7); // Projectile
            equippedGunSounds[2].Play();
        }
        // For every other gun that isn't a shotgun
        else
        {
            equippedGunParticleSystems[1].Emit(1); // Projectile
        }
    }

    [Command]
    void CmdSwitchWeapon(bool scrolledUp)
    {
        RpcSwitchWeapon(scrolledUp);
    }

    [ClientRpc]
    void RpcSwitchWeapon(bool scrolledUp)
    {
        if (scrolledUp == true)
        {
            do
            {
                // Flips back to zero if at the end of the index number
                if (equippedGunIndex == guns.Count - 1)
                {
                    equippedGunIndex = 0;
                }
                else
                {
                    equippedGunIndex++;
                }
            }
            while (isGunUnlocked[equippedGunIndex] == false);
        }
        else
        {
            do
            {
                if (equippedGunIndex == 0)
                {
                    equippedGunIndex = guns.Count - 1;
                }
                else
                {
                    equippedGunIndex--;
                }
            }
            while (isGunUnlocked[equippedGunIndex] == false);

        }

        StartCoroutine(switchWeapon());
    }

    [Command]
    void CmdReload()
    {
        RpcReload();
    }

    [ClientRpc]
    void RpcReload()
    {
        if(equippedGun.isShotugn == true)
        {
            StartCoroutine(reloadShotgun());
        }
        else
        {
            StartCoroutine(reload());
        }
    }

    IEnumerator reload()
    {
        cannotShoot = true;
        isReloading = true;
        equippedGun.anim.SetTrigger("reload");
        equippedGunSounds[1].Play();

        yield return new WaitForSeconds(equippedGun.reloadTime);

        if (equippedGun.ammoLeft >= equippedGun.clipSize)
        {
            equippedGun.ammoLeft -= (equippedGun.clipSize - equippedGun.roundsLeft);
            equippedGun.roundsLeft += (equippedGun.clipSize - equippedGun.roundsLeft);
        }
        else
        {
            int temp = equippedGun.ammoLeft;
            equippedGun.ammoLeft = 0;
            equippedGun.roundsLeft += temp;
        }
        cannotShoot = false;
        isReloading = false;
    }

    IEnumerator reloadShotgun()
    {
        cannotShoot = true;
        isReloading = true;
        interruptShotgun = false;
        // Transitioning into reload animation
        equippedGun.anim.SetTrigger("transitionToReload");
        yield return new WaitForSeconds(.333f); // The transition animation time 
        equippedGun.anim.SetBool("isReloading", true); // Creates transition animations when turned to false

        StartCoroutine(checkToShoot());
        if(interruptShotgun == true)
        {
            yield break;
        }
        equippedGun.anim.SetTrigger("reload"); // Plays this trigger once to loop reload animation 

        while (equippedGun.roundsLeft < equippedGun.clipSize - 1 && equippedGun.ammoLeft > 0)
        {
            if (interruptShotgun == true)
            {
                yield break;
            }
            equippedGun.roundsLeft++;
            equippedGun.ammoLeft--;
            equippedGunSounds[1].Play();
            yield return new WaitForSeconds(equippedGun.reloadTime);

            if (interruptShotgun == true)
            {
                yield break;
            }
        }
        // QUICK FIX just wanted to get the reloading right when reloading only one bullet
        if(equippedGun.roundsLeft == equippedGun.clipSize - 1)
        {
            equippedGun.anim.SetTrigger("singleReload");
        }
        // Becuase of animations, we have to do the last reload during the transition 
        equippedGun.roundsLeft++;
        equippedGun.ammoLeft--;
        equippedGunSounds[1].Play();

        // Transitioning out of reload animation
        equippedGun.anim.SetBool("isReloading", false);

        cannotShoot = false;
        isReloading = false;
    }

    IEnumerator checkToShoot()
    {
        while(cannotShoot && !GameController.isPaused && isReloading)
        {
            if(equippedGun.roundsLeft > 0 && Input.GetButtonDown("Fire1"))
            {
                shoot();
                CmdShoot();
                interruptShotgun = true;
                isReloading = false;
                cannotShoot = false;
                yield break;
            }
            yield return new WaitForSeconds(.01f);
        }
        yield break;
    }

    IEnumerator switchWeapon()
    {

        cannotShoot = true;
        equippedGun.anim.SetTrigger("switchWeapon");
        playerSounds[0].Play();
        yield return new WaitForSeconds(equippedGun.switchTime);

        // Disables the pervious gun and activates the new gun
        equippedGun.gameObject.SetActive(false);
        equippedGun = guns[equippedGunIndex];
        equippedGun.anim.SetTrigger("equipWeapon");
        equippedGun.gameObject.SetActive(true);
        playerSounds[1].Play();

        // Reinitializes all the equippedGun properties
        refreshEquippedGun();

        yield return new WaitForSeconds(equippedGun.equipTime); // Prevent players from doing anything else while equipping

        cannotShoot = false;
    }

    public void recalculateFireRate()
    {
        equippedGun.convertedFireRate = 1f / (equippedGun.fireRate / 60f);
    }
}
