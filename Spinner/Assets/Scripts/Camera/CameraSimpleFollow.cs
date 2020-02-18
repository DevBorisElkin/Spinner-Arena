using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSimpleFollow : MonoBehaviour
{
    public GameObject playerToFollow;

    public Vector3 offset;
    void Update()
    {
        transform.position = playerToFollow.transform.position + offset;
    }
}
