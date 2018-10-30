using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.Networking;


public class PauseManager : MonoBehaviour
{
    public Transform canvas;
    public Transform player;
    public Transform pauseMenu;
    //public Transform settingsMenu;
    public bool isPaused;

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Pause();
        }
    }
    public void Pause()
    {
        if (canvas.gameObject.activeInHierarchy == false)
        {
            if(pauseMenu.gameObject.activeInHierarchy == false)
            {
                pauseMenu.gameObject.SetActive(true);
               // settingsMenu.gameObject.SetActive(false);
            }

            canvas.gameObject.SetActive(true);
            Time.timeScale = 0;
            isPaused = true;
            //player.GetComponent<FirstPersonController>().enabled = false;
        }
        else
        {

            canvas.gameObject.SetActive(false);
            Time.timeScale = 1;
            isPaused = false;
            //player.GetComponent<FirstPersonController>().enabled = true;
        }
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(wait());
        GameController.isPaused = false;

    }
   /* public void Settings(bool Open)
    {
        if(Open)
        {
            settingsMenu.gameObject.SetActive(true);
            pauseMenu.gameObject.SetActive(false);
        }
        else
        {
            settingsMenu.gameObject.SetActive(false);
            pauseMenu.gameObject.SetActive(true);
        }
    }
    */
    public void Quit(bool quit)
    {
        if(quit)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenuFix");

            CustomNetworkManager.singleton.StopHost();
        }
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
    }
}
