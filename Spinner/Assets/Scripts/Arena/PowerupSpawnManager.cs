using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawnManager : MonoBehaviour
{
    public GameObject powerupPrefab;
    public GameObject awaitParticles;

    public List<GameObject> pointsForSpawn = new List<GameObject>();

    public float startDelay = 10f;
    public float spawnMinTime = 10f;
    public float spawnMaxTime = 15f;

    public float delayToActuallySpawn = 7f;

    GameObject awaitParticlesInstance;

    private void OnEnable()
    {
        StartCoroutine(PowerupSpawn());
    }

    IEnumerator PowerupSpawn()
    {
        yield return new WaitForSeconds(startDelay);
        while (true)
        {
            yield return new WaitForSeconds(SetRandomTime());
            int randomSpot = GetFreeIndex();
            if (randomSpot != -1)
            {
                SpawnPowerupParticles(randomSpot);
                yield return new WaitForSeconds(delayToActuallySpawn);
                SpawnPowerup(randomSpot);
            }
            else yield return null;
        }
    }

    float SetRandomTime()
    {
        return Random.Range(spawnMinTime, spawnMaxTime);
    }

    void SpawnPowerupParticles(int randomSpot)
    {
        awaitParticlesInstance = Instantiate(awaitParticles, pointsForSpawn[randomSpot].transform.position, pointsForSpawn[randomSpot].transform.rotation);
    }
    void SpawnPowerup(int randomSpot)
    {
        if (awaitParticlesInstance != null) Destroy(awaitParticlesInstance);
        GameObject powerup = Instantiate(powerupPrefab, pointsForSpawn[randomSpot].transform.position, pointsForSpawn[randomSpot].transform.rotation);
        powerup.GetComponent<Powerup>().index = randomSpot;
    }

    int GetFreeIndex()
    {
        Powerup[] powerupsInGame = FindObjectsOfType<Powerup>();
        
        for(int i = 0; i<10; i++)
        {
            int randomSpot = Random.Range(0, pointsForSpawn.Count);
            bool free = true;
            foreach (Powerup a in powerupsInGame)
            {
                if (a.index == randomSpot) free = false;
            }
            if (free)
                return randomSpot;
            else continue;
        }
        return -1;
    }

    public void DestroyAllPowerups()
    {
        Powerup[] powerupsInGame = FindObjectsOfType<Powerup>();
        foreach(Powerup a in powerupsInGame)
        {
            Destroy(a.gameObject);
        }
    }
}
