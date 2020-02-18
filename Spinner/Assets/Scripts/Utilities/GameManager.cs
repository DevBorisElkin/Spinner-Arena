using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum State { FirstAwake, Menu, InGame}
    public State currentState;

    public GameObject MainPanelUI;

    public GameObject exitToMenuButton;
    public GameObject sureWantExitMenu;

    PowerupSpawnManager powerupManager;

    List<SpawnPosition> spawnPositions;

    [Header("For madness watching")]
    public List<Material> materialsForMadness = new List<Material>();
    class SpawnPosition
    {
        public GameObject position;
        public bool free;

        public SpawnPosition(GameObject position, bool free)
        {
            this.position = position;
            this.free = free;
        }
    }
    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject AI_Prefab;

    private void Awake()
    {
        powerupManager = FindObjectOfType<PowerupSpawnManager>();
        SetSpawnPositions();
        SetState(State.FirstAwake);
    }

    void SetSpawnPositions()
    {
        spawnPositions = new List<SpawnPosition>();
        GameObject[] spawnsRaw = GameObject.FindGameObjectsWithTag("SpawnPosition"); 
        foreach(GameObject a in spawnsRaw)
        {
            spawnPositions.Add(new SpawnPosition(a, true));
        }
    }
    
    public void SetState(State state)
    {
        currentState = state;
        if (state == State.Menu)
        {
            StopAllCoroutines();
            MainPanelUI.SetActive(true);

            exitToMenuButton.SetActive(false);
            sureWantExitMenu.SetActive(false);

            powerupManager.enabled = false;
            powerupManager.DestroyAllPowerups();
            powerupManager.StopAllCoroutines();
            DestroyAllPlayers();
            FreeSpawnPositions();
            SpawnPlayersMimik();

        }
        else if(state == State.FirstAwake)
        {
            StopAllCoroutines();
            MainPanelUI.SetActive(true);

            exitToMenuButton.SetActive(false);
            sureWantExitMenu.SetActive(false);

            powerupManager.enabled = false;
            powerupManager.DestroyAllPowerups();
            powerupManager.StopAllCoroutines();
        }
        else if (state == State.InGame)
        {
            MainPanelUI.SetActive(false);
            exitToMenuButton.SetActive(true);

            powerupManager.enabled = true;

            DestroyAllPlayers();
            Invoke("LittleDelayAction", 0.05f);
        }
    }

    void LittleDelayAction()
    {
        if (!botsMadness)
            SpawnPlayersClassic();
        else SpawnPlayersBotsMadness();

        StartCoroutine(GameProgressCheck());
    }

    public void DestroyAllPlayers()
    {
        ArenaParticipant[] participants = FindObjectsOfType<ArenaParticipant>();
        foreach(ArenaParticipant a in participants)
        {
            Destroy(a.gameObject);
        }
    }
    //public GameObject exitToMenuButton;
    //public GameObject sureWantExitMenu;

    /*
    if(Time.timeScale !=0)
            Time.timeScale = 0;
        else Time.timeScale = 1;
     */
    public void OnClick_ButtonMenu()
    {
        Time.timeScale = 0;
        exitToMenuButton.SetActive(false);
        sureWantExitMenu.SetActive(true);
    }

    public void OnClick_ConfirmToMenu()
    {
        Time.timeScale = 1;
        SetState(State.Menu);
    }

    public void OnClick_CancelMenu()
    {
        Time.timeScale = 1;
        exitToMenuButton.SetActive(true);
        sureWantExitMenu.SetActive(false);
    }

    public void OnClick_Quit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        playersToSpawn = 5;
        disabledPlay = false;
        botsMadness = false;
        SetState(State.InGame);
    }
    public void StartMadnessGame()
    {
        playersToSpawn = 45;
        disabledPlay = false;
        botsMadness = false;
        SetState(State.InGame);
    }
    public void StartDisabledGame()
    {
        playersToSpawn = 12;
        disabledPlay = true;
        botsMadness = false;
        SetState(State.InGame);
    }
    public void StartBotsMadnessGame()
    {
        playersToSpawn = 50;
        disabledPlay = false;
        botsMadness = true;
        SetState(State.InGame);
    }

    int playersToSpawn = 6;
    bool disabledPlay;
    bool botsMadness;
    void SpawnPlayersClassic()
    {
        participants.Clear();
        GameObject spawnPos = GetRandomSpawnPosition();
        participants.Add(Instantiate(playerPrefab, spawnPos.transform.position, spawnPos.transform.rotation).GetComponent<ArenaParticipant>());

        for (int i = 1; i < (playersToSpawn+1); i++)
        {
            spawnPos = GetRandomSpawnPosition();
            participants.Add(Instantiate(AI_Prefab, spawnPos.transform.position, spawnPos.transform.rotation).GetComponent<ArenaParticipant>()); 
        }

        if (disabledPlay)
        {
            for(int i = 1; i< participants.Count; i++)
            {
                participants[i].DisableCompletely();
            }
        }

        if (playersToSpawn > 20)
        {
            for (int i = 1; i < materialsForMadness.Count+1; i++)
            {
                participants[i].PaintToColor(materialsForMadness[i-1]);
            }
        }
    }

    void SpawnPlayersMimik()
    {
        participants.Clear();
        GameObject spawnPos;
        for (int i = 0; i < (materialsForMadness.Count); i++)
        {
            spawnPos = GetRandomSpawnPosition();
            participants.Add(Instantiate(AI_Prefab, spawnPos.transform.position, spawnPos.transform.rotation).GetComponent<ArenaParticipant>());
        }

        for (int i = 0; i < materialsForMadness.Count; i++)
        {
            participants[i].PaintToColor(materialsForMadness[i]);
        }
    }

    void SpawnPlayersBotsMadness()
    {
        participants.Clear();
        GameObject spawnPos;
        for (int i = 0; i < (playersToSpawn); i++)
        {
            spawnPos = GetRandomSpawnPosition();
            participants.Add(Instantiate(AI_Prefab, spawnPos.transform.position, spawnPos.transform.rotation).GetComponent<ArenaParticipant>());
        }

        for (int i = 0; i < materialsForMadness.Count; i++)
        {
            participants[i].PaintToColor(materialsForMadness[i]);
        }

    }

    GameObject GetRandomSpawnPosition()
    {
        int i = 0;
        while (i<150)
        {
            int randomIndex = Random.Range(0, spawnPositions.Count);
            if (spawnPositions[randomIndex].free)
            {
                spawnPositions[randomIndex].free = false;
                return spawnPositions[randomIndex].position;
            }
            i++;
        }
        return spawnPositions[0].position;
    }

    void FreeSpawnPositions()
    {
        foreach(SpawnPosition a in spawnPositions)
        {
            a.free = true;
        }
    }
    


    private void Update()
    {
        if(currentState == State.InGame)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnClick_ButtonMenu();
            }
        }
    }

    List<ArenaParticipant> participants = new List<ArenaParticipant>();
    IEnumerator GameProgressCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            CheckWinLoose();
        }
    }

    void CheckWinLoose()
    {
        int activePlayers = 0;
        for (int i = 0; i < participants.Count; i++)
        {
            if (participants[i].enabled) activePlayers++;
        }
        if (activePlayers <= 1)
        {
            StopAllCoroutines();
            Invoke("SetStateMenu", 2f);
        }
    }
    void SetStateMenu()
    {
        SetState(State.Menu);
    }
}
