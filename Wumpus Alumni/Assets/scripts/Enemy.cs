using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class Enemy : NetworkBehaviour
{
    public float maxHp;

    [SerializeField]
    public static ParticleSystem getHitParticle;

    [SerializeField]
    [SyncVar] // Hopefully syncing the health of shootables will prevent some zombies being alives and others dead on different clients
    private float hp;

    void Awake()
    {
        hp = maxHp;
        getHitParticle = gameObject.GetComponent<ParticleSystem>();
    }

    public void takeDamage(float amount, Vector3 location, PlayerController player)
    {
        hp -= amount;
        getHitParticle.transform.position = location;
        getHitParticle.Play();
        if(hp <= 0f)
        {
            die(player);
        }
    }

    protected abstract void attack();

    public abstract void die(PlayerController player);

}
