using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Math;
using TMPro;

public class GameLoop : MonoBehaviour
{
    PlayerController playerController;
    float FIRSTSPAWNTIME = 1;
    [SerializeField]
    static float SPAWNTIME = 40;
    static float SUB_SPAWNTIME = 5;
    [SerializeField]
    int maxEnemy = 50;
    [SerializeField]
    int initialEnemyWave;
    int nbUnlockedEnemyTypes;
    private bool bossUnlocked;
    public static int nbEnemyType = 5;

    float timeInGame;
    float TIMETOBOSS = SPAWNTIME * (nbEnemyType + 2);
    float lastSpawn;
    float lastSubSPawn;
    [Header("Player")]
    [SerializeField]
    private GameObject player;

    [Header("Liens LevelUpPanel")]
    [SerializeField]
    private GameObject startText;
    [SerializeField]
    private GameObject victory;
    [SerializeField]
    private GameObject defeat;

    [Header("Hugo")]
    [SerializeField]
    private AnimationCurve nbEnemiesToSpawnUnconstrained;
    [SerializeField]
    private TextMeshProUGUI time;
    [SerializeField]
    private GameObject[] Artefacts;
    private int index = 0;
    private bool spawnArtefact = true;
    private bool bossSpawned = false;
    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        nbUnlockedEnemyTypes = 0;
        bossUnlocked = false;
        lastSpawn = FIRSTSPAWNTIME;
        time.text = numberOfSecondsToDisplayableString(0f);
        time.color = new Color(1f, 1f, 1f, 0.1f);
        playerController = player.GetComponent<PlayerController>();
    }

    State state = State.WAITING_TO_START;

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.WAITING_TO_START:
                // TODO: Manage tap to start
                startText.SetActive(true);
                PlayerStats.pause = true;
                if (Input.anyKey)
                {
                    StartGame();
                }
                break;
            case State.START:
                // Set all needed variables
                playerController.Reset();
                state = State.RUNNING;
                break;
            case State.RUNNING:
                // Artefacts
                if(spawnArtefact)
                {
                    if(nbUnlockedEnemyTypes - 2 >= index && Artefacts[index] != null && !Artefacts[index].activeSelf)
                    {
                        Artefacts[index].SetActive(true);
                        Artefacts[index].GetComponent<Artefact>().restant = 2 - (nbUnlockedEnemyTypes - 2);
                        spawnArtefact = false;
                        if(index < 2)
                            index++;
                    }
                }

                double gameProgressionRate = ((double)(nbUnlockedEnemyTypes)) / ((double)nbEnemyType + 1);
                lastSpawn -= Time.deltaTime;
                lastSubSPawn -= Time.deltaTime;
                timeInGame += Time.deltaTime;
                if (lastSpawn <= 0)
                {
                    time.color = new Color(
                        1f - (float)gameProgressionRate,
                        1f,
                        0f,
                        (0.2f + 0.8f * (float)gameProgressionRate)
                        );

                    if (nbUnlockedEnemyTypes < nbEnemyType) nbUnlockedEnemyTypes++;

                    if ((nbUnlockedEnemyTypes == nbEnemyType) && !bossUnlocked)
                    {
                        if (timeInGame >= TIMETOBOSS)
                        {
                            if(!spawnArtefact)
                            {
                                // We lose
                                EndGame(false);
                                PlayerStats.hp = 0;
                                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Die();
                                time.text = "Vous n'avez pas collecté les artefacts à temps";
                                time.color = Color.red;
                                return;
                            }
                            bossUnlocked = true;
                            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                            foreach (GameObject enemy in enemies)
                            {
                                Destroy(enemy, 0f);
                            }
                            GetComponent<WaveManager>().SpawnBoss(nbUnlockedEnemyTypes);
                            bossSpawned = true;
                        }
                        else
                        {
                            Debug.Log("Just waiting for the Boss.");
                        }
                    }
                    else if (!bossUnlocked)
                    {
                        int nbEnemyToSpawnUnconstrained = initialEnemyWave + maxEnemy * (int)System.Math.Floor(nbEnemiesToSpawnUnconstrained.Evaluate((float)gameProgressionRate));
                        int nbEnemyToSpawn = Mathf.Min(nbEnemyToSpawnUnconstrained, Mathf.Max(0, maxEnemy - GameObject.FindGameObjectsWithTag("Enemy").Length));
                        if (nbEnemyToSpawn > 0) GetComponent<WaveManager>().SpawnWave(nbEnemyToSpawn, nbUnlockedEnemyTypes);
                        lastSpawn = SPAWNTIME;
                        lastSubSPawn = SUB_SPAWNTIME;
                    }
                    else {
                        Debug.Log("Here doing nothing else than printing this is normal: nothing spawns after the B0$$!");
                    }

                } if(lastSubSPawn <= 0 && nbUnlockedEnemyTypes >= 1 && !bossSpawned)
                {
                    int nbEnemyToSpawnUnconstrained = initialEnemyWave + maxEnemy * (int)System.Math.Floor(nbEnemiesToSpawnUnconstrained.Evaluate((float)gameProgressionRate));
                    int nbEnemyToSpawn = Mathf.Min(nbEnemyToSpawnUnconstrained, Mathf.Max(0, maxEnemy - GameObject.FindGameObjectsWithTag("Enemy").Length));
                    if (nbEnemyToSpawn > 0) GetComponent<WaveManager>().SpawnWave(nbEnemyToSpawn, Mathf.Min(nbUnlockedEnemyTypes, 4));
                    lastSubSPawn = SUB_SPAWNTIME;
                }

                if (!bossUnlocked)
                {
                    time.text = numberOfSecondsToDisplayableString(timeInGame);
                }
                if (bossUnlocked)
                {
                    time.text = "BOSS\nUNLOCKED!";
                    time.color = Color.red;
                }

                break;
            case State.END:
                // Manage end of the game

                break;
            default:
                break;
        }
    }

    // Functions to invoke 

    public void StartGame()
    {
        PlayerStats.pause = false;
        startText.SetActive(false);
        state = State.START;
        initialEnemyWave = 4;
        nbUnlockedEnemyTypes = 0;
        bossUnlocked = false;
        time.text = numberOfSecondsToDisplayableString(TIMETOBOSS);
        time.color = new Color(0f, 1f, 0f, 0.1f);
        Instantiate(player, Vector3.zero, Quaternion.identity);
        timeInGame = 0f;
    }

    public void EndGame(bool win)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<Ennemy>().Freeze();
        }
        state = State.END;
        if(win)
        {
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            time.text = "";
            victory.SetActive(true);
        } else
        {
            defeat.SetActive(true);
        }
    }

    public void Replay()
    {
        time.text = "";
        victory.SetActive(false);
        defeat.SetActive(false);
        state = State.WAITING_TO_START;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }
        GameObject[] xp = GameObject.FindGameObjectsWithTag("XP");
        for (int i = 0; i < enemies.Length; i++)
        {
            try
            {
                Destroy(xp[i]);
            }
            catch
            {

            }

        }
    }

    public void GetArtefact()
    {
        spawnArtefact = true;
    }

    private string numberOfSecondsToDisplayableString(float time)
    {
        time = TIMETOBOSS - time;
        int seconds = ((int) time % 60);
        int minutes = ((int) time / 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}

public enum State
{
    WAITING_TO_START,
    START,
    RUNNING,
    END
}