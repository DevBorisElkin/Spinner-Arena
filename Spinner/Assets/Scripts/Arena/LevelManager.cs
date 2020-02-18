using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static List<Level> levels;
    private void Awake()
    {
        InitLevels();
    }

    void InitLevels()
    {
        levels = new List<Level>();
        levels.Add(new Level(0, 1,       5,   7));  // The default one
        levels.Add(new Level(1, 1.15f,   15,  9));
        levels.Add(new Level(2, 1.3f,    35,  11));
        levels.Add(new Level(3, 1.45f,   60,  14));
        levels.Add(new Level(4, 1.7f,    95,  17));
        levels.Add(new Level(5, 2f,      125, 21));
        levels.Add(new Level(6, 2.3f,    155, 25));
        levels.Add(new Level(7, 2.6f,    185, 30));
        levels.Add(new Level(8, 3.1f,    215, 35));
        levels.Add(new Level(9, 3.45f,   240, 36));
        levels.Add(new Level(10, 4f,     280, 37));
        levels.Add(new Level(11, 4.4f,   320, 38));
        levels.Add(new Level(12, 5f,     350, 39));
    }

    //public static void LevelUP(this ArenaParticipant participant)
    //{
    //    
    //}
    [System.Serializable]
    public class Level
    {
        public int level;
        public float scale;

        public float mass;
        public float maxSpeed;

        public Level(int level, float scale, float mass, float maxSpeed)
        {
            this.level = level;
            this.scale = scale;
            this.mass = mass;
            this.maxSpeed = maxSpeed;
        }
    }
}
