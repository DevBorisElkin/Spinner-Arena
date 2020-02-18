using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWalker : MonoBehaviour
{
    [HideInInspector]public Rigidbody rb;

    [HideInInspector] public int levelIndex;

    public virtual void RotateTowards(Vector3 rotateTo, float rotationSpeed)
    {
        Quaternion targetRotation = Quaternion.LookRotation(rotateTo - transform.position, rotateTo - transform.position);
        float str = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);
    }
    public virtual void RotateTowards2(Vector3 rotateTo, float rotationSpeed)
    {
        Quaternion targetRotation = Quaternion.LookRotation(rotateTo - transform.position);
        Quaternion targetRotationUpd = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        float str = Mathf.Min(rotationSpeed * Time.deltaTime, 1);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotationUpd, str);

        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    #region jumpRelated
    bool jumpable = true;
    public void LeaveZoneAndJump()
    {
        if (jumpable)
        {
            jumpable = false;
            //Debug.Log("applying jump on "+name);
            rb.AddForce(new Vector3(0, CalcForceToJump(), 0), ForceMode.Impulse);
            Invoke("BecomeJumpable", 1.25f);
        }
    }

    void BecomeJumpable()
    {
        jumpable = true;
    }

    float CalcForceToJump()
    {
        switch (levelIndex)
        {
            case 0:
                return 35;
            case 1:
                return 50 * 2;
            case 2:
                return 85 * 2;
            case 3:
                return 105 * 2;
            case 4:
                return 150 * 2;
            case 5:
                return 200 * 2;
            case 6:
                return 255 * 2;
            case 7:
                return 300 * 2;
            case 8:
                return 420 * 2;
            case 9:
                return 500 * 2;
            case 10:
                return 620 * 2;
            case 11:
                return 800 * 2;
            default:
                return 1000 * 2;
        }
    }

    #endregion
}
