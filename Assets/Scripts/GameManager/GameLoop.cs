using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    PlayerController player;
    const float SPAWNTIME = 1;
    public float lastSpawn = SPAWNTIME;
    

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        GetComponent<WaveManager>().SpawnWave();
    }

    State state = State.WAITING_TO_START;

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.WAITING_TO_START:
                // TODO: Manage tap to start
                state = State.START;
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
                    GetComponent<WaveManager>().SpawnWave();
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