using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKitShop : Shop
{
    private AudioSource[] sound;//0 purchase | 1 fail |

    public int cost = 250;
    public float hpAmount = 25f;

    void Start()
    {
        sound = gameObject.GetComponents<AudioSource>();
        promptToPlayer = "Press 'E' to purchase medkit\n($250)";
    }

    protected override void buy()
    {
        if (currPlayer.money >= cost && currPlayer.hp < currPlayer.maxHp)//purchaseable
        {
            sound[0].Play();
            currPlayer.money -= cost;
            currPlayer.hp = Mathf.Clamp(currPlayer.hp + hpAmount, 0, currPlayer.maxHp);
        }
        else
        {
            if(!sound[1].isPlaying)
            {
                sound[1].Play();
            }
        }
    }
}
