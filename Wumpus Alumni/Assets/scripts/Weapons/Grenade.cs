using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    public float fuseTime = 3f;
    private float timeLeft;

    public float range = 5f;
    public float damage = 300f;

    private bool exploded = false;

    public PlayerController player; // The player that threw the grenade

    private ParticleSystem[] ps;//
    private AudioSource[] sound;

	void Start ()
    {
        timeLeft = fuseTime;
        ps = gameObject.GetComponentsInChildren<ParticleSystem>();
        sound = gameObject.GetComponents<AudioSource>();
	}
	
	void Update ()
    {
        timeLeft -= Time.deltaTime;
        if(!exploded && timeLeft<=0)
        {
            explode();
        }
	}

    void explode()
    {

        exploded = true;
        Collider[] col = Physics.OverlapSphere(transform.position, range);


        foreach(ParticleSystem p in ps)
        {
            p.Play();
        }

        foreach(AudioSource s in sound)
        {
            s.Play();
        }

        foreach(Collider c in col)
        {
            Enemy shootable = c.GetComponent<Enemy>();
            if(shootable != null)
            {
                float taken = damage*(1f-(Vector3.Distance(transform.position,c.transform.position)/range));
                shootable.takeDamage(taken, c.ClosestPoint(transform.position), this.player);
            }
            PlayerController player = c.transform.root.GetComponent<PlayerController>();
            if(player)
            {
                Debug.Log("PLAYER HIT");
                float taken = (damage/2f) * (1f - (Vector3.Distance(transform.position, c.transform.position) / range));//less self inflicted damage
                player.takeDamage(taken);
            }

            Rigidbody rigidbody = c.GetComponent<Rigidbody>();
            if(rigidbody)
            {
                rigidbody.AddExplosionForce(300f, transform.position, range);
            }
        }


        Destroy(gameObject.GetComponent<BoxCollider>());
        Destroy(gameObject.GetComponent<Renderer>());

        Destroy(gameObject,5f);
    }
}
