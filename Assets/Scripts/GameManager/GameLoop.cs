using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoop : MonoBehaviour
{
    PlayerController playerController;
    [SerializeField]
    float SPAWNTIME = 1;
    [SerializeField]
    int maxEnemy;
    [SerializeField]
    int initialEnnemyWave;
    int nbEnnemySpawn;

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

    private void Start()
    {
        nbEnnemySpawn = initialEnnemyWave;
        lastSpawn = SPAWNTIME;
        playerController = player.GetComponent<PlayerController>();
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
                playerController.Reset();
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
        Instantiate(player, Vector3.zero, Quaternion.identity);
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
    }

    
}

public enum State
{
    WAITING_TO_START,
    START,
    RUNNING,
    END
}