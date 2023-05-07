using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Tagged Scene Objects
    [SerializeField]
    GameObject canvas;
    [SerializeField]
    UpgradeManager upgradeManager;
    
    bool isPaused = true;



    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerStats.experience >= PlayerStats.NEXT_LEVEL_EXP)
        {
            isPaused = true;
            PlayerStats.LevelUp();
            upgradeManager.buildUpgrades();
            canvas.SetActive(true);
        }

        if (isPaused)
        {
            return;
        }
        
    }


}
