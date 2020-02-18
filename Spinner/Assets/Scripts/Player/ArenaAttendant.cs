using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaAttendant : SimpleWalker
{
    // State maintance related
    [HideInInspector] public bool pushing = true;
    [HideInInspector] public bool checkingMaxSpeed = true;
    [HideInInspector] public bool inGame = true;

    public IEnumerator disable;

    public float maxSpeed = 7f;

    [Header("Level related")]
    [SerializeField]
    public LevelManager.Level level;


    #region level increase related
    public void IncreaseLevel(int increaseTo = 1)
    {
        if (!inGame) return;
        levelIndex+=increaseTo;
        if (LevelManager.levels.Count > levelIndex)
        {
            level = LevelManager.levels[levelIndex];
            ApplyLevelStats();
        }
        else
        {
            Debug.Log(name + " has already reached maximum level");
        }
    }
    public void ApplyLevelStats()
    {
        transform.localScale = new Vector3(level.scale, level.scale, level.scale);
        rb.mass = level.mass;
        maxSpeed = level.maxSpeed;
    }
    #endregion

    public ArenaParticipant lastPusher;
    public void OutOfGame()
    {
        if (inGame)
        {
            inGame = false;
            this.enabled = false;
            if (lastPusher != null)
            {
                lastPusher.IncreaseLevel();
            }
        }
    }


    public int secondsUntilDestruction = 5;
    public IEnumerator SafeSpaceCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            secondsUntilDestruction--;
            if (secondsUntilDestruction <= 0)
            {
                OutOfGame();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "SafeSpace")
        {
            secondsUntilDestruction = 5;
        }
    }
}
