using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    public int index; // The index of the player - differentiates players from each other

    public float maxHp;//player starting health

    [SyncVar]
    public float hp;//tracks current player hp

    public int money = 0;// tracks current player money
    public bool canRepsawn = false;

    public Text promptText;
    public GameObject waveText;

    private MouseLook mouseLook;

    private GameController gameController;

    bool isDead = false;

    void Start ()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        mouseLook = GetComponentInChildren<MouseLook>();
        transform.Find("CanvasSpectator").Find("Player Name").GetComponent<Text>().text = GetComponent<SetupLocalPlayer>().playerName;
        transform.Find("PlayerName").GetComponent<TextMesh>().text = GetComponent<SetupLocalPlayer>().playerName;

        transform.Find("CanvasSpectator").gameObject.SetActive(false);

        GameController.hasPlayerJoined = true;
        promptText = transform.Find("Canvas").Find("PromptText").GetComponent<Text>();
        waveText = transform.Find("Canvas").Find("WaveText").Find("waveNum").gameObject;
        hp = maxHp;


        GameController.players.Add(this.gameObject);
        GameController.hasPlayerRecentlyJoined = true;
        index = GameController.players.Count - 1;

        GameObject.FindObjectOfType<GameController>().GetComponent<GameController>().clientPlayer = this;

        if (isLocalPlayer == true)
        {
            transform.Find("Main Camera").tag = "MainCamera";
            // Turns on the client's camera stuff and HUD - their eyes only
            transform.Find("Main Camera").GetComponent<AudioListener>().enabled = true;
            transform.Find("Main Camera").GetComponent<Camera>().enabled = true;
            transform.Find("Canvas").gameObject.SetActive(true); // Turns on player UI

            // Turns off the client's body mesh - for their friends' eyes only
            transform.Find("Player Mesh").Find("Body").GetComponent<MeshRenderer>().enabled = false;
            transform.Find("Main Camera").Find("Head").GetComponent<MeshRenderer>().enabled = false;
            transform.Find("PlayerName").gameObject.SetActive(false);
            // Client player has their camera as the main camera
        }
    }

    void Update()
    {
        if (isLocalPlayer == false)
        {
            return;
        }

        // Transition player to spectator
        if (canRepsawn == true && Input.anyKeyDown == true)
        {
            gameController.spectate();
            Destroy(this.gameObject);
        }
    }


    public void takeDamage(float amount)
    {
        hp -= amount;
        if(hp <= 0 && isDead == false)
        {
            isDead = true;
            StartCoroutine(die());
        }
        // Recoil from getting hit
        mouseLook.recoil(new Vector2(3,3));
    }

    IEnumerator die()
    {
        GetComponent<GunController>().enabled = false;//stop shooting
        GetComponent<FPSMovement>().enabled = false;//stop movement
        GetComponentInChildren<MouseLook>().enabled = false;//stop aim
        GetComponent<GrenadeController>().enabled = false; // Stops gernade throwing
        GetComponent<Rigidbody>().mass = 1;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;//allows player to fall over

        // Resets the pathfinding for every zombie 
        StartCoroutine(updateZombiePathFinding());

        yield return new WaitForSeconds(3.5f);
        canRepsawn = true;
    }

    //Only runs for the local player.
    public override void OnStartLocalPlayer()
    {

    }

    [Command]
    public void CmdStartWaves()
    {
        RpcStartWaves();
    }

    [ClientRpc]
    public void RpcStartWaves()
    {
        FindObjectOfType<GameController>().GetComponent<GameController>().startWaves();
    }

    IEnumerator updateZombiePathFinding()
    {
        yield return new WaitForSeconds(3f);
        // Remove the playerController from the GameController 
        GameController.players.Remove(this.gameObject);
        foreach (GameObject zombie in GameObject.FindGameObjectsWithTag("Zombie"))
        {
            zombie.GetComponent<ZombieController>().updatePlayers();
        }
    }

    public void updateWaveNum()
    {
        waveText.GetComponent<waveText>().updateWaveNum();
    }

	public void chargeMoney(int m)
	{
		money -= m;
	}
}
