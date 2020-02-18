using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaZoning : MonoBehaviour
{
    public enum ZoneType { LeaveAndFail, LeaveAndJump }
    public ZoneType zoneType = ZoneType.LeaveAndFail;

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<ArenaParticipant>() == null) return;

        ArenaParticipant arenaParticipant = other.GetComponentInParent<ArenaParticipant>();
        if (zoneType == ZoneType.LeaveAndFail)
        { // Old better to rework
            arenaParticipant.enabled = false;
            Rigidbody rb = other.GetComponentInParent<Rigidbody>();
            rb.mass += 35f;
            rb.useGravity = true;
            arenaParticipant.OutOfGame();
        }else if(zoneType == ZoneType.LeaveAndJump)
        {
            arenaParticipant.LeaveZoneAndJump();
        }
    }
}
