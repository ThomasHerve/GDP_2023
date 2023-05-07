using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whip : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT");
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Ennemy>().TakeDamage(PlayerStats.damage);
        }
    }
}
