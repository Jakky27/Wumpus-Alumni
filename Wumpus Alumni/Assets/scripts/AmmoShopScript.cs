using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoShopScript : Shop
{
    public Gun tempGun; // The GunController of the current player
    public GrenadeController grenadeController;

    private AudioSource[] sound;//0 purchase | 1 fail |

    public int cost = 500;
    public int ammoAmount = 150;

    public int grenadeAmount = 1;


    void Start()
    {
        sound = gameObject.GetComponents<AudioSource>();
        promptToPlayer = "Purchase ammo\n($500)";
        
    }

    protected override void buy()
    {
        tempGun = currPlayer.transform.GetComponent<GunController>().equippedGun;
        grenadeController = currPlayer.transform.GetComponent<GrenadeController>();

        if (currPlayer.money >= cost && (tempGun.ammoLeft < tempGun.maxAmmo || grenadeController.grenades < grenadeController.maxGrenades))//purchaseable
        {
            sound[0].Play();
            currPlayer.money -= cost;
            if (tempGun.ammoLeft + ammoAmount > tempGun.maxAmmo)
            {
                tempGun.ammoLeft = tempGun.maxAmmo;
            }
            else
            {
                tempGun.ammoLeft += ammoAmount;
            }

            if(grenadeController.grenades + grenadeAmount > grenadeController.maxGrenades)
            {
                grenadeController.grenades = grenadeController.maxGrenades;
            }
            else
            {
                grenadeController.grenades += grenadeAmount;
            }

        }
        else
        {
            if (!sound[1].isPlaying)
            {
                sound[1].Play();

            }
        }
    }
}
