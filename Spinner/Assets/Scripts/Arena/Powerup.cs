using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public GameObject particlesPickedPrefab;
    public int index;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"&& other.GetComponentInParent<ArenaParticipant>() != null)
        {
            GetPicked(other.gameObject);
        }
    }
    void GetPicked(GameObject picker)
    {
        Instantiate(particlesPickedPrefab, transform.position, transform.rotation);
        int chance = Random.Range(0, 100);
        int increaseTo;
        if (chance > 40){increaseTo = 1;}
        else if (chance > 9){increaseTo = 2;}
        else{increaseTo = 3;}
        picker.GetComponentInParent<ArenaParticipant>().IncreaseLevel(increaseTo);
        Destroy(gameObject);
    }
}
