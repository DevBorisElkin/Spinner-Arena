using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    Rigidbody rb;

    public float speed = 5f;
    public float rotationSpeed = 5f;
    public float collisionForceMultiplier = 10f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        Movement();
        Action();
    }

    void Action()
    {
        //Debug.Log(transform.forward);
    }

    void Movement()
    {
        rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.VelocityChange);

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(new Vector3(0, 1 * Time.deltaTime * rotationSpeed, 0));
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(new Vector3(0, -1 * Time.deltaTime * rotationSpeed, 0));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<Rigidbody>() == null) return;

        if(collision.impulse.magnitude> 0.1f)
        {
            Debug.Log("Collision force: "+collision.impulse.magnitude);

            collision.collider.GetComponent<Rigidbody>().AddForce(transform.forward * collision.impulse.magnitude * collisionForceMultiplier);
        }
    }
}
