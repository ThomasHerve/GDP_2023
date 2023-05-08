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

    //Sounds
    [SerializeField]
    AudioSource voiceSource;
    [SerializeField]
    AudioClip[] voicesClip;
    [SerializeField]
    private float voiceCoolDown = 60f;

    [SerializeField]
    AudioSource effectSource;
    [SerializeField]
    AudioClip[] effectsClip;
    [SerializeField]
    private float effectCoolDown = 2f;

    private NavMeshAgent navMeshAgent;
    private GameObject player;
    private PlayerController playerController;

    // Damages
    [SerializeField]
    private float damageTime;

    [SerializeField]
    private bool isBoss;

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

    private float voiceCd;
    private float effectCd;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        voiceCd = voiceCoolDown;
        effectCd = effectCoolDown;

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

        if (voicesClip.Length > 0)
        {
            voiceCd -= Time.deltaTime;
            if (voiceCd <= 0)
            {
                voiceSource.PlayOneShot(voicesClip[Random.Range(0, voicesClip.Length)]);
                voiceCd = voiceCoolDown;
            }
        }
        if (effectsClip.Length>0)
        {
            effectCd -= Time.deltaTime;
            if (effectCd <= 0)
            {
                effectSource.PlayOneShot(effectsClip[Random.Range(0, effectsClip.Length)]);
                effectCd = effectCoolDown;
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
            // Lifesteal
            PlayerStats.Lifesteal();
            dead = true;
            GetComponent<Animation>().Play("SimpleDeath");
            var xp = Instantiate(particles, transform.position, Quaternion.identity);
            xp.GetComponent<XP>().Launch(numberXP, valueXP);
            if(isBoss)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameLoop>().EndGame(true);
            }
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
