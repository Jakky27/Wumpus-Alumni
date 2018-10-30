using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleProjectile : MonoBehaviour
{
    private float damage; // Damage inflicts
    private float knockBack; 

    protected PlayerController player; // The player of the projectile

    protected List<ParticleCollisionEvent> collisionEvents; // A list holding information about a collision 

    public ParticleSystem destroyedParticle; // Particle effect when the particle gets destroyed
    public ParticleSystem hitEnemyParticle; // Temporary blood particle. This particle should be emitted from ZombieController instead

    public ParticleSystem particleSystem;

    void Awake()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
        player = transform.root.GetComponent<PlayerController>();
    }

    void Start()
    {
        damage = transform.parent.GetComponent<Gun>().damage;
        knockBack = transform.parent.GetComponent<Gun>().knockback;

        particleSystem = GetComponent<ParticleSystem>();

        // Sets up all the possible triggers for bullet whizzing
        for (int i = 0; i < GameController.players.Count; i++)
        {
            // Excludes its own collider
            if (GameController.players[i].GetComponent<PlayerController>().index == transform.root.GetComponent<PlayerController>().index)
            {
                continue;
            }

            particleSystem.trigger.SetCollider(i, GameController.players[i].GetComponent<Collider>());
        }
    }

    void Update ()
    {
		if(GameController.hasPlayerRecentlyJoined == true)
        {
            // Sets up all the possible triggers for bullet whizzing
            for (int i = 0; i < GameController.players.Count; i++)
            {
                // Excludes its own collider
                if (GameController.players[i].GetComponent<PlayerController>().index == transform.root.GetComponent<PlayerController>().index)
                {
                    continue;
                }

                particleSystem.trigger.SetCollider(i, GameController.players[i].GetComponent<Collider>());
            }
            GameController.hasPlayerRecentlyJoined = false;
        }
	}

    // Emits n particles at the collision location. 
    // The particle faces directly away from the collision (the normal)
    void emitAtLocation(ParticleSystem particleSystem, ParticleCollisionEvent collisionEvent, int count)
    {
        particleSystem.transform.position = collisionEvent.intersection; // Moves the deathParticle to where the projectile collided
        particleSystem.transform.rotation = Quaternion.LookRotation(collisionEvent.normal); // Rotates the deathParticle AWAY from the collision 
        particleSystem.Emit(count);
    }

    void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(transform.GetComponent<ParticleSystem>(), other, collisionEvents); // Gets the collision data (location, normal, etc.)

        // If zombie was hit. Currently has two blood particles shoot
        Enemy shootable = other.GetComponent<Enemy>();
        if (shootable != null)
        {
            // For loop runs once. 
            foreach (ParticleCollisionEvent collisionEvent in collisionEvents)
            {
                emitAtLocation(hitEnemyParticle, collisionEvent, 10);
                shootable.takeDamage(damage, collisionEvent.intersection, player); // Another blood splatter here
            }

            // Adds force onto anything with a shootable script 
            Rigidbody rigidBody = other.GetComponent<Rigidbody>();
            if (rigidBody != null)
            {
                foreach (ParticleCollisionEvent collsionEvent in collisionEvents)
                {
                    rigidBody.AddForce(-collsionEvent.normal * knockBack); //knock hit object back 
                }
            }
        }
        // If a player was hit
        /*else if(other.tag.Equals("Player"))
        {
            foreach (ParticleCollisionEvent collisionEvent in collisionEvents)
            {
                emitAtLocation(hitEnemyParticle, collisionEvent, 10);
                other.GetComponent<PlayerController>().takeDamage(10);
            }
        }*/
        // If anything is a child of the environment gameObject
        else if (other.transform.root.name.Equals("Environment"))
        {
            foreach (ParticleCollisionEvent collisionEvent in collisionEvents)
            {
                emitAtLocation(destroyedParticle, collisionEvent, 10);
            }
        }
    }
    void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> particleList = new List<ParticleSystem.Particle>(); // List of triggered particles

        int numEnter = particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particleList);
        // Goes through every trigger particle, but there's probably just one
        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = particleList[i];
            // DEBUG - makes the trigger particles the color blue (the second line registers it I think)
            //p.startColor = Color.blue;
            //particleList[i] = p;

            // Determines which player the bullet passed throguh
            foreach (GameObject player in GameController.players)
            {
                if(player.GetComponent<PlayerController>().index == transform.root.GetComponent<PlayerController>().index)
                {
                    continue;
                }

                // If the bullet is within 3f from the player, then play the sound!
                if(Vector3.Distance(player.transform.position, p.position) < 3f)
                {
                    player.transform.root.GetComponent<GunController>().playerSounds[3].Play();
                }
            }

            
        }

        // Setting the particles
        particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particleList);

    }
}
