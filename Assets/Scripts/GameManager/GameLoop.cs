using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    PlayerController player;
    [SerializeField]
    float SPAWNTIME = 1;
    [SerializeField]
    int maxEnemy;
    [SerializeField]
    int initialEnnemyWave;
    int nbEnnemySpawn;

    float lastSpawn;

    [Header("Liens canvas")]
    [SerializeField]
    private GameObject startText;

    private void Start()
    {
        nbEnnemySpawn = initialEnnemyWave;
        lastSpawn = SPAWNTIME;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    State state = State.WAITING_TO_START;

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.WAITING_TO_START:
                // TODO: Manage tap to start
                startText.SetActive(true);
                PlayerStats.pause = true;
                if(Input.anyKey)
                {
                    StartGame();
                }
                break;
            case State.START:
                // Set all needed variables
                player.Reset();

                state = State.RUNNING;
                break;
            case State.RUNNING:
                // Game logic over time

                // doute sur le delta

                lastSpawn -= Time.deltaTime;
                if (lastSpawn <= 0)
                {
                    // Check for max
                    int CurrentnbEnnemySpawn = Mathf.Min(nbEnnemySpawn, Mathf.Max(0, maxEnemy - GameObject.FindGameObjectsWithTag("Enemy").Length));
                    GetComponent<WaveManager>().SpawnWave(CurrentnbEnnemySpawn);
                    lastSpawn = SPAWNTIME;
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
    }

    public void EndGame()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<Ennemy>().Freeze();
        }
        state = State.END;
    }

    
}

public enum State
{
    WAITING_TO_START,
    START,
    RUNNING,
    END
}