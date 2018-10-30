using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnerScript : NetworkBehaviour
{
    public GameController gs;

    public GameObject[] zombieList;

    void Start()
    {
    }

    // Only the server will spawn zombies
    public IEnumerator spawnZombies()
    {
        while(gs.leftToSpawn>0)
        {
            int randomNum = (int)Random.Range(0,zombieList.Length);
            
            GameObject zombie = Instantiate(zombieList[randomNum], transform.position, transform.rotation);
            NetworkServer.Spawn(zombie);
            gs.leftToSpawn--;

            yield return new WaitForSeconds(Random.Range(1.5f,2.5f));//random time as precation against spawners double spawning
        }

        yield break;
    }

    void OnNetworkInstantiate(NetworkMessageInfo info)
    {
        Debug.Log("Something was instantiated");
    }



}
