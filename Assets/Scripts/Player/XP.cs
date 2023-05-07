using System.Collections.Generic;
using UnityEngine;

public class XP : MonoBehaviour
{
    PlayerController player;
    private int xp;
    private int number;
    private bool up = false;
    private bool started = false;
    private int launched = 0; 

    // XP
    ParticleSystem ps;
    List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        ps = GetComponent<ParticleSystem>();
        ps.trigger.AddCollider(GameObject.FindGameObjectWithTag("Player").GetComponent<CapsuleCollider>());
        ps.Stop();
    }


    public void Launch(int number, int xpGained)
    {
        xp = xpGained;
        this.number = number;
        launched = 1;
    }

    private void Update()
    {
        if(launched == 1)
        {
            ps.emission.SetBurst(0, new ParticleSystem.Burst(0, 1, number, 0.1f));
            ps.Play();
            var main = ps.main;
            main.maxParticles = number;
            started = true;
            launched = 2;
        }
        if (!started)
            return;
        if (up)
        {
            if (ps.particleCount < number)
            {
                for (int i = ps.particleCount; i < number; i++)
                {
                    player.GetXP(xp);
                }
                number = ps.particleCount;
            }
        }
        else
        {
            if (ps.particleCount == number)
            {
                up = true;
                var e = ps.externalForces;
                e.enabled = true;
            }
        }
    }

}
