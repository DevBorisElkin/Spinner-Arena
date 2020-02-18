using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesFollowScript : MonoBehaviour
{
    public GameObject objToFollow;
    void Update()
    {
        if(objToFollow.activeSelf)
        transform.position = objToFollow.transform.position;
    }
}
