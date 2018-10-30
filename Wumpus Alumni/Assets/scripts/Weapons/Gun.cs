using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public bool isAuto;
    public bool isShotugn = false;

    public float damage;
    public float knockback;
    public float range;

    public int maxAmmo;
    public int ammoLeft; // Ammo left

    public Vector2 recoil;

    public int clipSize;
    public int roundsLeft; // Ammo in 'magazine' left

    public float fireRate; //rounds/min
    public float convertedFireRate; //delay in sec between rounds

    public float reloadTime; //reload duration short
    public float switchTime; // Some guns take longer to switch out than others. SwitchTime should be longer than switch animation at the least!
    public float equipTime; // Some take longer to equip - must be longer than equip animation

    public float maxRecoil; //in degrees x rotation
    public float minRecoil;

    public float zoomFOV; 
    public float unZoomFOV; 

    public Animator anim;
    protected AudioSource[] sound;//0 gunshot | 1 reload |

    protected ParticleSystem[] particleSystems;//0 muzzleFlash | 1 bullet |

    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        particleSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
        sound = gameObject.GetComponents<AudioSource>();
    }
}
