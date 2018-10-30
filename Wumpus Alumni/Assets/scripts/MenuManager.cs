using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuHolder;
    public GameObject optionsMenuHolder;
    public GameObject lobbyMenuHodler;

    public void OptionsMenu()
    {
        mainMenuHolder.SetActive(false);
        lobbyMenuHodler.SetActive(false);
        optionsMenuHolder.SetActive(true);
    }
    public void MainMenu()
    {
        mainMenuHolder.SetActive(true);
        lobbyMenuHodler.SetActive(false);
        optionsMenuHolder.SetActive(false);
    }
    public void LobbyMenu()
    {
        mainMenuHolder.SetActive(false);
        lobbyMenuHodler.SetActive(true);
        optionsMenuHolder.SetActive(false);
    }
}
