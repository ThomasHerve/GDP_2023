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

    [SerializeField]
    private int numberXP;
    [SerializeField]
    private int valueXP;
    [SerializeField]
    private GameObject particles;

    [SerializeField]
    float knockbackForce = 20f;

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
    private bool dead = false;

    // Invincibility
    private float invicibilityTime = 0.8f;
    private float currentInvicibilityTime = 0;

    // Pause
    private bool pauseTriggered = false;


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
        if (PlayerStats.pause)
        {
            if (!pauseTriggered) {
                pauseTriggered = true;
                navMeshAgent.destination = transform.position;
            }
            return;
        }


        if(!frozen && navMeshAgent.isOnNavMesh)
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
        if (collision.gameObject.tag == "Player")
        {
            isCollided = true;
        }else if(collision.gameObject.tag == "Sauce")
        {
            navMeshAgent.speed /= PlayerStats.SLOW_GRADIENT;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            currentOutTime = outTime;
        }
        else if (collision.gameObject.tag == "Sauce")
        {
            navMeshAgent.speed *= PlayerStats.SLOW_GRADIENT;
        }
    }

    public void Die()
    {
        if(!dead)
        {
            dead = true;
            GetComponent<Animation>().Play("SimpleDeath");
            var xp = Instantiate(particles, transform.position, Quaternion.identity);
            xp.GetComponent<XP>().Launch(numberXP, valueXP);
            Destroy(gameObject, 2);
        }
    }

    public void Freeze()
    {
        navMeshAgent.destination = transform.position;
        frozen = true;
    }

    public void Blast()
    {
        TakeDamage(PlayerStats.bottleDamage);
    }

    public void TakeDamage(int dmg)
    {
        Vector3 knockbackDirection = transform.forward * -1f;
        GetComponent<Rigidbody>().AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);

        // Lifesteal
        PlayerStats.Lifesteal();

        hp -= dmg;
        if (hp < 0)
            Die();
        StartCoroutine(InvincibilityRoutine());
    }

    IEnumerator InvincibilityRoutine()
    {
        GetComponent<Collider>().enabled = false;
        currentInvicibilityTime = 0;
        while(currentInvicibilityTime < invicibilityTime)
        {
            currentInvicibilityTime += Time.deltaTime;
            yield return null;
        }
        GetComponent<Collider>().enabled = true;
    }
}
