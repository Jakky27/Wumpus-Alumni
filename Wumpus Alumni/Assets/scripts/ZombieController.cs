using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class ZombieController : Enemy
{
    public float damage = 10f;
    public float attkRange = 1.3f;//range of attacks
    public float attkCD = 1f;//attack cooldown

    public int reward = 100;//amount of money awarded for kill

    private float lastAttack = 0f;

    private NavMeshAgent agent;
    private GameObject targetPlayer;

    private GameController gameController;//refrence to game control

    private bool dead = false;

    private AudioSource[] sound;//0 idle | 1 attack

    public float distance = Mathf.Infinity; // Distance away from closest player

    void Start()
    {
        gameObject.transform.SetParent(GameObject.Find("Zombies").transform); // Adds zombie to a parent
        agent = gameObject.GetComponent<NavMeshAgent>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        sound = gameObject.GetComponents<AudioSource>();
        targetPlayer = GameController.players[0]; // Sets the first player as the target initially
    }

    void Update()
    {
        if (!GameController.isPaused)
        {
            if (agent.enabled)
            {
                // Checks for whoever is closest player
                distance = Vector3.Distance(transform.position, targetPlayer.transform.position); // Recalculates the distance away from the closest player
                foreach (GameObject player in GameController.players)
                {
                    if (Vector3.Distance(transform.position, player.transform.position) < distance)
                    {
                        distance = Vector3.Distance(transform.position, player.transform.position);
                        targetPlayer = player;
                    }
                }
                agent.SetDestination(targetPlayer.transform.position);

                if (Vector3.Distance(transform.position, targetPlayer.transform.position) <= attkRange && Time.time >= lastAttack + attkCD)//check if attack is possible
                {
                    attack();
                }
            }

            if (getHitParticle == null)
            {
                getHitParticle = transform.root.Find("bloodSpray").GetComponent<ParticleSystem>();
            }
        }
    }

    // Once a player dies, zombies need to updated with the new list of players 
    public void updatePlayers()
    {
        distance = Mathf.Infinity;
        targetPlayer = GameController.players[0];
    }

    protected override void attack()
    {
        sound[1].pitch = Random.Range(0.8f, 1.2f);//variation
        sound[1].Play();
        lastAttack = Time.time;
        targetPlayer.GetComponent<PlayerController>().takeDamage(damage);

    }

    public override void die(PlayerController player)
    {
        if (!dead)//prevents multiple calls of death method
        {
            player.GetComponent<PlayerController>().money += reward;
            sound[0].Stop();
            dead = true;
            gameController.remainingZombies--;
            //gameObject.GetComponent<Collider>().enabled = false;
            agent.enabled = false;
            GetComponent<Rigidbody>().isKinematic = false;//allows physics interaction
            GetComponent<NetworkTransform>().enabled = false;
            StartCoroutine(waitToDie());
        }

    }

    // If for some reason a zombie dies in one client but not the others, then destroy it in all clients
    IEnumerator waitToDie()
    {
        yield return new WaitForSeconds(1f);
        NetworkServer.Destroy(gameObject);
    }
}
