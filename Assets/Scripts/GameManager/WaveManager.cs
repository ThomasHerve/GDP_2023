using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class WaveManager : MonoBehaviour
{
    // On veut faired es waves
    // get gameobject enemy
    public float distanceSpawn;
    private GameObject player;
    public GameObject enemy;

    Vector3 boxSize = new Vector3(3,3,3);

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

    public void SpawnWave(int nbEnemyPerWave)
    {
        // Spawn grâce à l'instantiation en boucle de "enemy" qui pointe sur le GameObject ennemy
        for (int i = 0; i < nbEnemyPerWave; i++)
        {
            Instantiate(enemy, GetCoordinates(), Quaternion.identity);
        }
    }

    public Vector3 GetCoordinates()
    {
        Vector3 pos = Vector3.zero;
        bool found = false;
        var count = 0;
        while(!found && count < 10)
        {
            count++;
            found = true;
            pos = CalculateCoordinates();

            // CHeck coordinate is valid
            Vector3 boxExtents = boxSize / 2f;
            Vector3 boxMin = pos - boxExtents;
            Vector3 boxMax = pos + boxExtents;

            // Récupérer tous les colliders situés dans la boîte
            Collider[] colliders = Physics.OverlapBox(pos, boxExtents);

            // Parcourir tous les colliders récupérés
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
            }
        }
        Debug.Log(pos);
        return pos;
        // return Vector3.zero;
    }

    public Vector3 CalculateCoordinates()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        // Determiner où mettre un monstre par rapport au joueur
        float angle = Random.Range(0, 360);
        float r = Random.Range(25, 35);
        Vector3 playerPostion = player.transform.position;
        float x = (playerPostion.x + r * Mathf.Cos(Mathf.Deg2Rad * angle));
        float z = (playerPostion.z + r * Mathf.Sin(Mathf.Deg2Rad * angle));

        return new Vector3(x, 0, z);
    }
}
