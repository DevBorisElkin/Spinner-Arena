using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float destroyAfter = 5f;
    void Start()
    {
        Invoke("Destroy", destroyAfter);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
