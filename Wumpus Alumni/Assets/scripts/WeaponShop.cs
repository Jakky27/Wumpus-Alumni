using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShop : Shop
{

    public static bool isInShop;

    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Player"))//if player is within purchase area
        {
            if(currPlayer.isLocalPlayer == true)
            {
                isInShop = true;
                currPlayer = other.transform.root.GetComponent<PlayerController>();
                currPlayer.transform.Find("CanvasWeaponShop").gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if (currPlayer == null)
            {
                return;
            }

            if(currPlayer.isLocalPlayer == true)
            {
                isInShop = false;
                currPlayer.transform.Find("CanvasWeaponShop").gameObject.SetActive(false);
                currPlayer = null;
                Cursor.lockState = CursorLockMode.Locked;
            }

        }
    }
    protected override void buy()
    {
        
    }
}
