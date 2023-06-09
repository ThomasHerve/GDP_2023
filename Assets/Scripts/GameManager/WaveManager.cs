using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    // On veut faire des waves
    // get gameobject enemy
    public float distanceSpawn;
    private GameObject player;
    public GameObject[] enemyType;
    private static System.Random rng;
    public 

    Vector3 boxSize = new Vector3(1.5f,1.5f,1.5f);

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rng = new System.Random();
        //enemyType = new GameObject[nbEnemyType];
    }

    // Update is called once per frame
    void Update()
    {
        // We spawn every 2 minutes

        // Faire spawn toute les X secondes
        // Increase le nombre de spawn avec le temps
    }

    public void SpawnWave(int nbEnemyToSpawn, int nbUnlockedEnemyTypes)
    {
        Debug.Log(nbUnlockedEnemyTypes.ToString());
        Instantiate(enemyType[nbUnlockedEnemyTypes - 1], GetCoordinates(), Quaternion.identity);
        // Spawn gr�ce � l'instantiation en boucle de "enemy" qui pointe sur le GameObject enemy
        for (int i = 0; i < nbEnemyToSpawn - 1; i++)
        {
            Instantiate(enemyType[nbUnlockedEnemyTypes - 1], GetCoordinates(), Quaternion.identity); //rng.Next(0, nbUnlockedEnemyTypes - 1)
        }
    }
    public void SpawnBoss(int nbUnlockedEnemyTypes)
    {
        Instantiate(enemyType[nbUnlockedEnemyTypes - 1], Vector3.zero, Quaternion.identity);
    }
    public Vector3 GetCoordinates()
    {
        Vector3 pos = Vector3.zero;
        bool found = false;
        var count = 0;
        while(!found && count < 100)
        {
            count++;
            found = true;
            pos = CalculateCoordinates();

            // CHeck coordinate is valid
            Vector3 boxExtents = boxSize / 2f;
            Vector3 boxMin = pos - boxExtents;
            Vector3 boxMax = pos + boxExtents;

            // R�cup�rer tous les colliders situ�s dans la bo�te
            Collider[] colliders = Physics.OverlapBox(pos, boxExtents);

            // Parcourir tous les colliders r�cup�r�s
            foreach (Collider collider in colliders)
            {
                if(collider.gameObject.tag != "Plane")
                {
                    found = false;
                }
            }
            if (colliders.Length != 1)
            {
                found = false;
            } else if (colliders[0].gameObject.tag != "Plane")
            {
                found=false;
            }
        }
        return pos;
        // return Vector3.zero;
    }

    public Vector3 CalculateCoordinates()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        // Determiner o� mettre un monstre par rapport au joueur
        float angle = Random.Range(0, 360);
        float r = Random.Range(25, 35);
        Vector3 playerPostion = player.transform.position;
        float x = (playerPostion.x + r * Mathf.Cos(Mathf.Deg2Rad * angle));
        float z = (playerPostion.z + r * Mathf.Sin(Mathf.Deg2Rad * angle));

        return new Vector3(x, 0, z);
    }
}
