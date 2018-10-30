using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour
{
    public  bool isGameStarted = false; // When wave starts
    public static bool hasPlayerJoined = false; // Checks if at least one player has joined. Prevents client from pressing F5 and starting waves before anyone has joined
    public static bool hasPlayerRecentlyJoined = false; // When a new player joins, then update particleSystems' triggers for bullet whizzing
    public bool isSpectating = false;
    public static bool isPaused = false;
    public static int waveNum = 1;
    public int remainingZombies = 0;//number of zombies alive or waiting to be spawned
    public int leftToSpawn = 10;//number of zombies to be spawned in a wave 
    public Transform pauseMenu;

    private List<SpawnerScript> zombieSpawners;

    public static List<GameObject> players; // List of all players 

    public PlayerController clientPlayer; // The GameController's client player // EACH CLIENT DOES NOT HAVE THEIR OWN INSTANCE OF GAMECONTROLLER, so this variable isn't functional

    int playerIndex = 0;

    void Awake()
    {
        players = new List<GameObject>();
    }

    void Start ()
    {
        this.zombieSpawners = new List<SpawnerScript>();

        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Respawn");//find all spawners
        foreach (GameObject spawner in spawners)
        {
            this.zombieSpawners.Add(spawner.GetComponent<SpawnerScript>());//add their script to a list
        }
	}

    void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            CmdPause();
        }
        // To start the waves
        if (hasPlayerJoined == true && isGameStarted == false && Input.GetKeyUp(KeyCode.F5) && !isPaused)
        {
            clientPlayer.CmdStartWaves();
        }

        // Locks mouse cursor when clicking the screen
        if (hasPlayerJoined == true && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !pauseMenu.gameObject.activeInHierarchy && !WeaponShop.isInShop)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if(pauseMenu.gameObject.activeInHierarchy == true)
        {
            Cursor.lockState = CursorLockMode.None;

        }


    }

    [Command]
    void CmdPause()
    {
        RpcPause();
    }

    [ClientRpc]
    void RpcPause()
    {
        isPaused = !isPaused;
    }

    IEnumerator waitToQuitGame()
    {
        // Sends every player back to main menu
        //Time.timeScale = 1;
        SceneManager.LoadScene("MainMenuFix");

        CustomNetworkManager.singleton.StopHost();
        yield break;

    }

    public void spectate()
    {
        if(players.Count != 0)
        {
            players[playerIndex].GetComponentInChildren<Camera>().enabled = true;
            players[playerIndex].transform.Find("CanvasSpectator").gameObject.SetActive(true);
        }
        // There are no players left to spectate, so make them go back to main menu
        else
        {
            // Sends every player back to main menu
            StartCoroutine(waitToQuitGame());
        }
    }

    public void startWaves()
    {
        GameObject.Find("Start Game Prompt Text").SetActive(false); // Erases the start prompt text

        // The server will start the game (spawning zombies)
        if(clientPlayer.isServer == true)
        {
            StartCoroutine(gameLoop());//start the game
        }
    }


    void nextWave()
    {
        waveNum++;
        foreach(GameObject player in players)
        {
            player.GetComponent<PlayerController>().updateWaveNum();
        }
        if (waveNum <= 5)
        {
            leftToSpawn = (waveNum * 2) + 5;
        }
        else leftToSpawn = (waveNum * 4) + 5;
    }
	
	IEnumerator gameLoop()
    {
        while(true)
        {
            remainingZombies = leftToSpawn;//start of new wave

            foreach(SpawnerScript spawner in zombieSpawners)
            {
                spawner.StartCoroutine(spawner.spawnZombies());//tell teach spawner to begin spawning zombies again
            }

            while(remainingZombies > 0)//wait until all zombies are dead
            {
                yield return new WaitForSeconds(0.5f);//prevents checking too often, performance 
            }

            yield return new WaitForSeconds(10f);//end of wave 

            nextWave();
        }
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        Debug.Log("PLAYER DISCONNECTED");
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);

    }
}
