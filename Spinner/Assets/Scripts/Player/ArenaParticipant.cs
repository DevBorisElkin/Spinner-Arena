using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaParticipant : ArenaAttendant
{
    protected Vector3 curDir;
    protected Quaternion targetRot;

    [Header("Movement")]
    public float speed = 200f;
    public float turnSpeed = 30f;
    

    [Header("Collision related")]
    public float stopMovingFor = 1f;
    public float collisionForceMultiplier = 10f;


    public virtual void Start()
    {
        InitObject();
    }

    public virtual void InitObject()
    {
        rb = GetComponent<Rigidbody>();
        disable = DisableMovement(stopMovingFor);

        level = LevelManager.levels[0];
        ApplyLevelStats();
        StartCoroutine(SafeSpaceCheck());
    }

    #region movement related
    public virtual void Update()
    {
        ManageMovement();
    }

    public virtual void ManageMovement()
    {
        PushTowards();
        CheckMaxSpeed();
        SetDirection();
        RotateToDirection();
    }

    public virtual void PushTowards()
    {
        if(pushing)
        rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.VelocityChange);
    }

    public virtual void CheckMaxSpeed()
    {
        if (checkingMaxSpeed)
        {
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }
    }

    public virtual void SetDirection() { }

    public virtual void RotateToDirection()
    {
        if (curDir != Vector3.zero)
        {
            targetRot = Quaternion.LookRotation(curDir);
            if (rb.rotation != targetRot)
            {
                rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRot, turnSpeed); 
            }
        }
    }

    public void DisableMovement()
    {
        StopCoroutine(disable);
        disable = DisableMovement(stopMovingFor);
        StartCoroutine(disable);
    }
    IEnumerator DisableMovement(float time)
    {
        pushing = false;
        checkingMaxSpeed = false;
        yield return new WaitForSeconds(time);
        pushing = true;
        checkingMaxSpeed = true;
    }

    #endregion

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.impulse.magnitude < 0.5f) return;
        //Debug.Log(collision.impulse.magnitude+" when colliding with "+name);
        
        if (collision.collider.tag == "Player")
        {
            if (collision.collider.GetComponentInParent<ArenaParticipant>() != null)
            {
                collision.collider.GetComponentInParent<ArenaParticipant>().MakePhysicalImpact(transform.forward, collision.impulse.magnitude);
                collision.collider.GetComponentInParent<ArenaParticipant>().lastPusher = this;
            }
        }
    }

    void MakePhysicalImpact(Vector3 HitterVectorForward, float hitForce)
    {
        DisableMovement();
        rb.AddForce(HitterVectorForward * hitForce * collisionForceMultiplier);
    }

    public void DisableCompletely()
    {
        speed = 0;
    }

    [Header("Customization related")]
    public GameObject top;
    public GameObject bottom;
    [Space(15)]
    public Material red;
    public Material green, blue, purple, cyan;

    public void PaintToColor(Material material)
    {
        MeshRenderer meshTop = top.GetComponent<MeshRenderer>();
        MeshRenderer meshBot = bottom.GetComponent<MeshRenderer>();
        meshTop.material = material;
        meshBot.material = material;
    }
}
