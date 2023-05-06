using System.Collections.Generic;
using UnityEngine;

public class XP : MonoBehaviour
{
    private int xp;

    // XP
    ParticleSystem ps;
    List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ps.Stop();
        Launch(4, 1);
    }


    public void Launch(int number, int xpGained)
    {
        ps.trigger.AddCollider(GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>());
        ps.emission.SetBurst(0, new ParticleSystem.Burst(0, 1, number, 0.1f));
        ps.Play();
        xp = xpGained;
        var main = ps.main;
        main.maxParticles = number;
    }

    // XP
    private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "Player")
        {
            int triggerParticles = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
            for (int i = 0; i < triggerParticles; i++)
            {
                ParticleSystem.Particle p = particles[i];
                p.remainingLifetime = 0;
                other.gameObject.GetComponent<PlayerController>().GetXP(xp);
                particles[i] = p;
            }

            ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
        }
    }
}
