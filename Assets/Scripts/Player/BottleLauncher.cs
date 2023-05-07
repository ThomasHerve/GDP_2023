using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleLauncher : MonoBehaviour
{
    float bottleCount = 0;
    GameObject bottle;

    void Start(){
        bottle = transform.Find("Bottle").gameObject;
    }

    private void Update()
    {
        if (PlayerStats.isBottleUp)
            return;

        bottleCount += Time.deltaTime;
        if (bottleCount >= Mathf.Max(PlayerStats.bottleCd, 2))
        {
            ResetBottle();
            bottleCount = 0;
        }
    }

    public void ResetBottle() {
        bottle.transform.SetParent(this.transform, true);

        bottle.transform.localPosition = new Vector3(1.2f, 1f, -2.4f);
        bottle.transform.localEulerAngles = new Vector3(0, 0, 30);

        PlayerStats.isBottleUp = true;
    }

    public void ThrowBottle()
    {
        GetComponent<Animation>().Stop();
        bottle.GetComponent<Bottle>().Throw();
    }
}
