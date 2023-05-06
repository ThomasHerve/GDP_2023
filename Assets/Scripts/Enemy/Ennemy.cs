using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ennemy : MonoBehaviour
{
    [Header("Parametres")]
    [SerializeField]
    private int hp = 20;
    
    [SerializeField]
    private int damage;

    private NavMeshAgent navMeshAgent;
    private GameObject player;
    private PlayerController playerController;

    // Damages
    [SerializeField]
    private float damageTime;
    private float currentDamageTime = 0;
    private bool isCollided = false;

    private float outTime = 0.5f;
    private float currentOutTime = 0;

    private bool frozen = false;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!frozen)
            navMeshAgent.destination = player.transform.position;

        if(currentOutTime > 0)
        {
            currentOutTime-=Time.deltaTime;
            if(currentOutTime <= 0)
            {
                isCollided = false;
                currentDamageTime = 0;
            }
        }

        if(isCollided)
        {
            currentDamageTime += Time.deltaTime;
            if(currentDamageTime > damageTime)
            {
                currentDamageTime = 0;
                playerController.TakeDamages(damage);
            }
        }

    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Player")
        {
            isCollided = true;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            currentOutTime = outTime;
        }
    }

    public void Die()
    {
        GetComponent<Animation>().Play("SimpleDeath");
        Destroy(gameObject,2);
    }

    public void Freeze()
    {
        navMeshAgent.destination = transform.position;
        frozen = true;
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp < 0)
            Die();
    }
}
