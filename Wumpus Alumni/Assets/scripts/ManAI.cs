using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ManAI : MonoBehaviour
{

    enum behavior
    {
        wander,
        run,
        anger,
        dead
    
    }


    public float maxHp = 100f;

    [SerializeField]
    private float currentHp;


    //coroutine states
    private bool isWander;
    private bool isRun;
    private bool isAnger;

    public float wanderRange  = 3f;

    private NavMeshAgent agent;

    [SerializeField]
    private behavior currentBehavior;

    private Vector3 destination;
    private GameObject player;

    private ParticleSystem blood;


	void Start ()
    {
        blood = gameObject.GetComponentInChildren<ParticleSystem>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        currentBehavior = behavior.wander;
        currentHp = maxHp;
        player = GameObject.FindGameObjectWithTag("Player");

    }
	
	void Update ()
    {

        if(currentHp<=0f && currentBehavior!=behavior.dead)
        {
            Die();
        }

        switch(currentBehavior)
        {
            case behavior.wander:
                if (isWander) { break; }
                else { StartCoroutine(wander()); }
                break;


            case behavior.run:
                if(isRun) { break; }
                else { StartCoroutine(run()); }
                break;

            default:
                break;

        }
	}

    Vector3 getNewDest()
    {
        NavMeshHit hit;
        Vector3 randomDir = Random.insideUnitSphere * Random.Range(wanderRange/2f,wanderRange);
        NavMesh.SamplePosition(transform.position + randomDir, out hit, wanderRange * 3f, 1);
        return hit.position;
    }


    IEnumerator wander()
    {
        isWander = true;
        while(true)
        {
            if (currentBehavior != behavior.wander)
            {
                isWander = false;
                yield break;
            }

            destination = getNewDest();


            agent.SetDestination(destination);

            yield return new WaitForSeconds(Random.Range(2f, 5f));
        }
        
    }

    IEnumerator run()
    {
        isRun = true;
        while(true)
        {

            if (currentBehavior!=behavior.run)
            {
                isRun = false;
                yield break;
            }

            NavMeshHit hit;
            NavMesh.SamplePosition(((transform.position - player.transform.position)*wanderRange),out hit,wanderRange*3f,1);

            destination = hit.position;
            
            agent.SetDestination(destination);

            Debug.Log(player.gameObject.name);

            yield return new WaitForSeconds(Random.Range(1f, 1.5f));


        }
    }

    public void takeDamage(float amount)
    {
        currentHp -= amount;
        currentBehavior = behavior.run;
        blood.Emit((int)Random.Range(15f, 25f));

       
    }

    void Die()
    {
        agent.enabled = false;
        currentBehavior = behavior.dead;
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce((transform.position - player.transform.position).normalized * 35f);
        Destroy(gameObject, 5f);
        
    }
}
