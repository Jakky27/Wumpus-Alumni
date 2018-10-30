using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateUpgradeShop : Shop
{

    private AudioSource[] sound;//0 purchase | 1 fail |

    public int cost;

    public int tier = -1;

    public float[] fireRates;//length of these arrays must be the same
    public int[] prices;

	void Start()
    {
        sound = gameObject.GetComponents<AudioSource>();
        cost = prices[0];
        promptToPlayer = "Press 'E' to purchase Fire Rate " + (tier + 2).ToString() + " ($" + cost.ToString() + ")";

    }

    protected override void buy()
    {
        if (tier < fireRates.Length-1 && currPlayer.money >= cost)
        {
            tier++;
            currPlayer.money -= cost;
            currPlayer.transform.GetComponent<GunController>().equippedGun.fireRate = fireRates[tier];
            currPlayer.transform.GetComponent<GunController>().recalculateFireRate();

            cost = prices[tier];

            sound[0].Play();

            if(tier<fireRates.Length-1)
            {
                promptToPlayer = "Press 'E' to purchase Fire Rate " + (tier + 2).ToString() + " ($" + cost.ToString() + ")";//+2 to convert from index to external

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
