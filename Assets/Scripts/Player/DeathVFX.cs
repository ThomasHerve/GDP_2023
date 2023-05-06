using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathVFX : MonoBehaviour
{
    [SerializeField]
    float time;
    float currentTime = 0;

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= time)Destroy(gameObject);
    }
}
