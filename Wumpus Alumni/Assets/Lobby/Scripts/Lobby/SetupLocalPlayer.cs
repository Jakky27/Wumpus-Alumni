using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class SetupLocalPlayer : NetworkBehaviour
{
    [SyncVar]
    public string playerName = "";

    [SyncVar]
    public Color playerColor = Color.white;


    [Command]
    public void CmdChangeName(string newName)
    {
        playerName = newName;
    }

    void Start()
    {
        transform.Find("Player Mesh").Find("Body").GetComponent<MeshRenderer>().material.color = playerColor;
    }

}
