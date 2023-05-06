using UnityEngine.ParticleSystemJobs;
using UnityEngine;

public class XP : MonoBehaviour
{

    // XP
    ParticleSystem ps;
    ParticleSystem.Particle[] particles;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // XP
    private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("it works");
        }
    }
}
