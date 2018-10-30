using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUpgradeShop : Shop
{
    private AudioSource[] sound; //0 purchase | 1 fail |

    public Gun tempGun; // The GunController of the current player

    public int cost;

    public int tier = -1;

    public float[] damage;//length of these arrays must be the same
    public int[] prices;


    void Start ()
    {
        sound = gameObject.GetComponents<AudioSource>();
        cost = prices[0];
        promptToPlayer = "Press 'E' to purchase Damage " + (tier + 2).ToString() + " ($" + cost.ToString() + ")";
    }

    protected override void buy()
    {
        tempGun = currPlayer.transform.GetComponent<GunController>().equippedGun;

        if (tier < damage.Length - 1 && currPlayer.money >= cost)
        {
            tier++;
            currPlayer.money -= cost;
            tempGun.damage = damage[tier];

            cost = prices[tier];

            sound[0].Play();

            if (tier < damage.Length - 1)
            {
                promptToPlayer = "Press 'E' to purchase Damage " + (tier + 2).ToString() + " ($" + cost.ToString() + ")";//+2 to convert from index to external

            }
            else
            {
                promptToPlayer = "You have purchased all the upgrades here!";
            }
        }
        else
        {
            sound[1].Play();
        }
    }
}
