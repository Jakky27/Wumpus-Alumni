using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Shop : MonoBehaviour
{
    public string promptToPlayer;//onscreen prompt

    protected bool active = false;//is the player in range?

    public PlayerController currPlayer; // The player currently inside a shop zone

    void Start()
    {

    }

    void Update()
    {
        if (active && Input.GetKeyDown(KeyCode.E))
        {
            buy();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        currPlayer = other.transform.root.GetComponent<PlayerController>();
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag.Equals("Player") && currPlayer.isLocalPlayer)//if player is within purchase area
        {
            active = true;
            currPlayer = col.transform.root.GetComponent<PlayerController>();
            currPlayer.promptText.text = promptToPlayer;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag.Equals("Player") && currPlayer.isLocalPlayer)
        {
            currPlayer.promptText.text = "";
            active = false;
            currPlayer = null;
        }
    }

    protected abstract void buy();
}
