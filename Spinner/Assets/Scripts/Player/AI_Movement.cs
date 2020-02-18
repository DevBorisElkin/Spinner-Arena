using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI_Movement : ArenaParticipant
{
    public enum BotType { PointWalker, Shy, Agressive}

    public BotType botType;

    public List<GameObject> PointsToGo;

    public GameObject targetPoint;

    public override void InitObject()
    {
        base.InitObject();
        InitBot();
    }

    void InitBot()
    {
        SetType();
        SetPoints();
        SetRandomHitsAmount();
        StartCoroutine(BotManagementCoroutine());
    }

    void SetType()
    {
        if(botType == 0)
        {
            int a = Random.Range(0, 101);
            if (a < 30)
            {
                botType = BotType.Shy;
            }
            else { botType = BotType.Agressive; }
        }
    }

    void SetPoints()
    {
        PointsToGo = new List<GameObject>();
        GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
        PointsToGo = points.ToList(); 
    }
    public override void RotateToDirection()
    {
        if (targetPoint != null)
        {
            RotateTowards2(targetPoint.transform.position, turnSpeed);
        }
        else ChangeGoal();
    }
    IEnumerator BotManagementCoroutine()
    {
        while (true)
        {
            ManageBehaviour(botType);
            yield return new WaitForSeconds(0.1f);
        }
    }

    bool reachedGoal = true;
    void ManageBehaviour(BotType botType)
    {
        CheckAchievingGoal();
        PowerupCheck();
    }

    void PowerupCheck()
    {
        GameObject powerup = GetNearestPowerup(15f);
        if (powerup != null)
        {
            targetPoint = powerup;
            return;
        }
    }


    public int hitsToMake;
    void SetRandomHitsAmount()
    {
        hitsToMake = Random.Range(5,15);
    }
    void CheckAchievingGoal()
    {
        if (targetPoint != null)
        {
                  // Target is player
            if (targetPoint.GetComponentInParent<ArenaParticipant>() != null)
            {
                // target is killed
                if (!targetPoint.GetComponentInParent<ArenaParticipant>().enabled)  reachedGoal = true; 

                if (hitsToMake <= 0) reachedGoal = true;
            }
            else  // Target is point or Powerup
            {
                if (Vector3.Distance(transform.position, targetPoint.transform.position) < 1f * transform.localScale.x * 1.5f)
                {
                    reachedGoal = true;
                }
            }
        }
        if (reachedGoal) { ChangeGoal(); return; }
    }
    void ChangeGoal()
    {
        reachedGoal = false;
        if(botType == BotType.PointWalker)
        {
            targetPoint = PointsToGo[Random.Range(0, PointsToGo.Count)];
        }
        else if(botType == BotType.Shy)
        {
            int a = Random.Range(0, 101);
            if (a < 70)
            {
                targetPoint = PointsToGo[Random.Range(0, PointsToGo.Count)];
            }
            else
            {
                targetPoint = GetPLayerToAttack();
            }
        }
        else if(botType == BotType.Agressive)
        {
            int a = Random.Range(0, 101);
            if (a < 45)
            {
                targetPoint = PointsToGo[Random.Range(0, PointsToGo.Count)];
            }
            else
            {
                targetPoint = GetPLayerToAttack();
            }
        }
    }

    GameObject GetPLayerToAttack(bool closest = false)
    {
        ArenaParticipant[] participants = FindObjectsOfType<ArenaParticipant>();
        List<ArenaParticipant> activeParticipants = new List<ArenaParticipant>();
        foreach(ArenaParticipant a in participants)
        {
            if (a.enabled && a.gameObject != gameObject) activeParticipants.Add(a);
        }
        // gotta fix self - finding
        if (activeParticipants.Count > 0)
        {
            SetRandomHitsAmount();
            return activeParticipants[Random.Range(0, activeParticipants.Count)].gameObject;
        }
        return null;
    }

    

    GameObject GetNearestPowerup(float distanceToCheck = 12.5f)
    {
        GameObject[] powerups = GameObject.FindGameObjectsWithTag("Powerup");
        if (powerups.Length > 0)
        {
            GameObject closest = GetClosestGameobject(powerups);
            if(Vector3.Distance(transform.position, closest.transform.position) < distanceToCheck)
            {
                return closest;
            }
            return null;
        }
        else
        {
            return null;
        }
    }

    GameObject GetClosestGameobject(GameObject[] array)
    {
        GameObject closest = array[0];
        for (int i = 1; i < array.Length; i++)
        {
            if(Vector3.Distance(transform.position, array[i].transform.position) < Vector3.Distance(transform.position, closest.transform.position))
            {
                closest = array[i];
            }
        }
        return closest;
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.collider.GetComponentInParent<ArenaParticipant>() != null)
        {
            hitsToMake--;
        }
    }
}
