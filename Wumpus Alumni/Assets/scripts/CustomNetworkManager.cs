using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomNetworkManager : NetworkManager
{
    public void StartupHost()
    {
        SetPort();
        NetworkManager.singleton.StartHost(); //Have to use .singleton or else the start up is glitchy
        Debug.Log("RunStart");
    }
    void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777; //Hard coding the port for now
    }
    void OnLevelWasLoaded(int level)
    {
        if (level == 2)
        {
            SetupMenuSceneButtons();

        }
        else
        {
            SetupGameSceneButtons();
        }
    }
    void SetupMenuSceneButtons()
    {
        GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.AddListener(StartupHost);


    }
    void SetupGameSceneButtons()
    {

      //GameObject.Find("QuitButton").GetComponent<Button>().onClick.RemoveAllListeners();
      //GameObject.Find("QuitButton").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);

    }
}
