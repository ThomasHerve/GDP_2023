using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Tagged Scene Objects
    GameObject canvas;
    
    bool isPaused = true;



    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerStats.experience >= PlayerStats.nextLevelExp)
        {
            isPaused = true;
            PlayerStats.LevelUp();
            canvas.GetComponent<UpgradeManager>().buildUpgrades();
            canvas.SetActive(true);
        }

        if (isPaused)
        {
            return;
        }
        
    }


}
