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
    static float SPAWNTIME = 3;
    [SerializeField]
    int maxEnemy = 50;
    [SerializeField]
    int initialEnemyWave;
    int nbUnlockedEnemyTypes;
    public static int nbEnemyType = 5;

    float timeInGame;
    float TIMETOWIN = SPAWNTIME * (nbEnemyType + 1);
    float lastSpawn;
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
   
    private void Start()
    {
        lastSpawn = SPAWNTIME;
        playerController = player.GetComponent<PlayerController>();
        // curve.Evaluate();
        nbUnlockedEnemyTypes = 0;
        // nbEnemySpawn = initialEnnemyWave;
        lastSpawn = FIRSTSPAWNTIME;
        time.text = numberOfSecondsToDisplayableString(0f);
        time.color = new Color(1f, 1f, 0f, 0.2f);
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
                double gameProgressionRate = ((double)(nbUnlockedEnemyTypes)) / ((double)nbEnemyType);
                lastSpawn -= Time.deltaTime;
                timeInGame += Time.deltaTime;
                if (lastSpawn <= 0)
                {
                    time.color = new Color(
                        1f - (float)gameProgressionRate,
                        1f,
                        0f,
                        (0.2f + 0.7f * (float)gameProgressionRate)
                        );
                    nbUnlockedEnemyTypes++;
                    
                    int nbEnemyToSpawnUnconstrained = initialEnemyWave + maxEnemy * (int)System.Math.Floor(nbEnemiesToSpawnUnconstrained.Evaluate((float) gameProgressionRate));
                    int nbEnemyToSpawn = Mathf.Min(nbEnemyToSpawnUnconstrained, Mathf.Max(0, maxEnemy - GameObject.FindGameObjectsWithTag("Enemy").Length));
                    if (nbEnemyToSpawn > 0) GetComponent<WaveManager>().SpawnWave(nbEnemyToSpawn, nbUnlockedEnemyTypes);
                    lastSpawn = SPAWNTIME;
                }
                time.text = numberOfSecondsToDisplayableString(timeInGame);
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
        time.text = numberOfSecondsToDisplayableString(TIMETOWIN);
        time.color = new Color(1f, 1f, 0f, 0.2f);
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
            victory.SetActive(true);
        } else
        {
            defeat.SetActive(true);
        }
    }

    public void Replay()
    {
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

    private string numberOfSecondsToDisplayableString(float time)
    {
        time = TIMETOWIN - time;
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