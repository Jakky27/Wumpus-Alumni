using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyBackToMain : MonoBehaviour
{
    public GameObject mainMenuHolder;
    public GameObject optionsMenuHolder;
    public GameObject lobbyMenuHodler;

    public Button backButton;

    void OnEnable()
    {
        backButton.onClick.AddListener(delegate { OnBackButtonClick(); });
    }

    public void OnBackButtonClick()
    {
        Debug.Log("Back");
        mainMenuHolder.SetActive(true);
        lobbyMenuHodler.SetActive(false);
        optionsMenuHolder.SetActive(false);
    }

}
