using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    // On veut faired es waves
    // get gameobject enemy
    public float distanceSpawn;
    public int nbEnemyPerWave = 7;
    private GameObject player;
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Faire spawn toute les X secondes
        // Increase le nombre de spawn avec le temps
    }

    public void SpawnWave()
    {
        // Spawn grâce à l'instantiation en boucle de "enemy" qui pointe sur le GameObject ennemy
        for (int i = 0; i < nbEnemyPerWave; i++)
        {
            Instantiate(enemy, GetCoordinates(), Quaternion.identity);
        }
    }

    public Vector3 GetCoordinates()
    {
        // Determiner où mettre un monstre par rapport au joueur
        float angle = Random.Range(0, 360);
        float r = Random.Range(25, 35);
        Vector3 playerPostion = player.transform.position;
        float x = (playerPostion.x + r * Mathf.Cos(Mathf.Deg2Rad*angle));
        float z = (playerPostion.z + r * Mathf.Sin(Mathf.Deg2Rad*angle));
        return new Vector3(x, player.transform.position.y, z);
        // return Vector3.zero;
    }
}
