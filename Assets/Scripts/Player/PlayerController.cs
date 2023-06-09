using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    // Parameters
    [Header("Parametres")]
    [SerializeField]
    float speed;
    [SerializeField]
    float acceleration;

    [Header("Graphics")]
    [SerializeField]
    float deathAnimationTime;
    [SerializeField]
    GameObject deathParticleEffect;


    [Header("Sounds")]
    [SerializeField]
    AudioSource footStepSource;
    [SerializeField]
    AudioClip[] footStepClips;
    [SerializeField]
    AudioSource whipSource;
    [SerializeField]
    AudioClip whipClip;
    [SerializeField]
    AudioSource voiceSource;
    [SerializeField]
    AudioClip[] attackClip;
    [SerializeField]
    AudioClip[] bottleClip;
    [SerializeField]
    AudioClip[] idleClip;
    [SerializeField]
    AudioClip[] levelUpClip;
    [SerializeField]
    AudioClip[] deathClip;
    [SerializeField]
    AudioClip[] hitClip;

    // Velocity management
    private Vector3 targetVelocity;
    private Vector3 currentVelocity;
    
    // Keyboard specific
    Plane m_Plane;
    Vector3 mousePosition;

    // Components
    private Rigidbody rigidbody;
    private GameLoop gameLoop;

    private float startYPosition;

    private Animator animator;
    private float movementThreshold = 3f;
    private bool isMoving = false;
    private float footStepTimer = 0f;
    private float whipCd = 0f;
    private float talkInThisManyFrames;
    static private System.Random rng;


    // Start is called before the first frame update
    void Start()
    {
        rng = new System.Random();
        talkInThisManyFrames = rng.Next(1, 2);

        rigidbody = GetComponent<Rigidbody>();
        m_Plane = new Plane(Vector3.up, Vector3.zero);
        gameLoop = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameLoop>();
        startYPosition = transform.position.y;
        animator = GetComponent<Animator>();
        GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>().Follow = transform;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < enemies.Length; i++)
        {
            Destroy(enemies[i]);
        }
        GameObject[] xp = GameObject.FindGameObjectsWithTag("XP");
        for (int i = 0; i < enemies.Length; i++)
        {
            try
            {
                Destroy(xp[i]);
            } catch
            {

            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStats.pause)
            return;
        if (PlayerStats.hp > 0)
        {
            talkInThisManyFrames -= Time.deltaTime;
            if (talkInThisManyFrames <= 0)
            {
                voiceSource.PlayOneShot(idleClip[Random.Range(0, idleClip.Length)]);
                talkInThisManyFrames = rng.Next(2, 10);
            }

            currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, Time.deltaTime * acceleration);
            rigidbody.velocity = currentVelocity;

            if (isMoving)
            {
                if (rigidbody.velocity.magnitude < movementThreshold)
                {
                    isMoving = false;
                    animator.SetBool("isWalking", false);
                }

                footStepTimer -= Time.deltaTime;
                if(footStepTimer <= 0)
                {
                    footStepTimer = 0.4f;
                    footStepSource.PlayOneShot(footStepClips[Random.Range(0, footStepClips.Length)]);
                }
            }
            else
            {
                if (rigidbody.velocity.magnitude > movementThreshold)
                {
                    isMoving = true;
                    animator.SetBool("isWalking", true);
                }

            }

            if (mousePosition != null)
            {
                ComputeRotateMouse();
            }
        }
        if (whipCd > 0)
        {
            whipCd -= Time.deltaTime;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 newVelocity = context.ReadValue<Vector2>();
        targetVelocity = new Vector3(newVelocity.x, 0, newVelocity.y) * speed;
    }

    public void Rotate(InputAction.CallbackContext context)
    {
        Vector3 v = new Vector3(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y, 0);
        
        if (v != Vector3.zero)
        {
            if (context.control.device.name.Contains("Mouse"))
            {
                mousePosition = v;
            }
            else
            {
                transform.forward = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
            }
        }

    }

    private void ComputeRotateMouse()
    {
        if (mousePosition != Vector3.zero)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);

            //Initialise the enter variable
            float enter = 0.0f;

            if (m_Plane.Raycast(ray, out enter))
            {
                //Get the point that is clicked
                Vector3 v = mousePosition;
                v = ray.GetPoint(enter);
                /*
                float angle = Vector3.SignedAngle((v - transform.position).normalized, new Vector3(0, 0, 1), Vector3.up) * -1;
                transform.eulerAngles = new Vector3(0, angle, 0);
                */
                transform.LookAt(new Vector3(v.x, transform.position.y, v.z));
            }
        }
    }

    public void TakeDamages(int damages)
    {
        voiceSource.PlayOneShot(hitClip[Random.Range(0, hitClip.Length)]);
        if (PlayerStats.hp > 0)
        {
            PlayerStats.TakeDamage(damages);
            if (PlayerStats.hp <= 0)
            {
                gameLoop.EndGame(false);
                Die();
            }
        }
    }

    public void Die()
    {
        StartCoroutine(deathCoroutine());
    }

    public void Reset()
    {
        PlayerStats.Reset();
        transform.localScale = new Vector3(1, 1, 1);
        transform.position = new Vector3(transform.position.x, startYPosition, transform.position.z);
    }

    IEnumerator deathCoroutine()
    {
        voiceSource.PlayOneShot(deathClip[Random.Range(0, deathClip.Length)]);
        float currentDeathAnimationTime = 0;
        while(currentDeathAnimationTime < deathAnimationTime)
        {
            currentDeathAnimationTime += Time.deltaTime;
            if(1 - (currentDeathAnimationTime / deathAnimationTime) > 0)
            {
                transform.localScale = new Vector3(1 - (currentDeathAnimationTime / deathAnimationTime), 1 - (currentDeathAnimationTime / deathAnimationTime), 1 - (currentDeathAnimationTime / deathAnimationTime));
                transform.position = new Vector3(transform.position.x, transform.localScale.y / 2, transform.position.z);
            }
            yield return null;
        }
        Instantiate(deathParticleEffect, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
    
    // XP
    public void GetXP(int value)
    {
        PlayerStats.GainXP(value);
    }

    public void Hit()
    {
        if (whipCd > 0)
            return;
        whipCd = 0.4f;

        transform.Find("RightHand").GetComponent<Animation>().Play();
        double standardDeviation = 0.1;
        double u1 = 1.0 - Random.Range(0,1f); //uniform(0,1] random doubles
        double u2 = 1.0 - Random.Range(0, 1f);
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
        double randNormal = 1 + standardDeviation * randStdNormal; //random normal(mean,stdDev^2)
        whipSource.pitch = (float)randNormal;
        whipSource.PlayOneShot(whipClip);
        voiceSource.PlayOneShot(attackClip[Random.Range(0, attackClip.Length)]);
    }

    public void ThrowBottle()
    {
        if (PlayerStats.isBottleUp)
        {
            transform.Find("LeftHand").GetComponent<Animation>().Play();
            voiceSource.PlayOneShot(bottleClip[Random.Range(0, bottleClip.Length)]);
        }

    }

    public void PlayLevelUpSound()
    {
        voiceSource.PlayOneShot(levelUpClip[Random.Range(0, levelUpClip.Length)]);
    }
}
