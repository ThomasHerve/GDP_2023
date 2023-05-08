using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Tagged Scene Objects
    [SerializeField]
    GameObject HealthBar;
    [SerializeField]
    GameObject ExpBar;
    [SerializeField]
    GameObject LevelUpPanel;
    [SerializeField]
    UpgradeManager upgradeManager;
    
    bool isPaused = true;



    // Start is called before the first frame update
    void Start()
    {
        PlayerStats.gameManager = this;
        UpdateHealthBarmax();
        UpdateHealthBar();
        updateExpBarMax();
        updateExpBar();


    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerStats.experience >= PlayerStats.NEXT_LEVEL_EXP)
        {
            isPaused = true;
            PlayerStats.LevelUp();
            upgradeManager.buildUpgrades();
            LevelUpPanel.SetActive(true);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().PlayLevelUpSound();
        }

        if (isPaused)
        {
            return;
        }
        
    }


    public void UpdateHealthBar()
    {
        HealthBar.GetComponent<Slider>().value = PlayerStats.hp;
    }
    public void UpdateHealthBarmax()
    {
        HealthBar.GetComponent<Slider>().maxValue = PlayerStats.hpMax;
    }

    public void updateExpBar()
    {
        ExpBar.GetComponent<Slider>().value = PlayerStats.experience;
    }

    public void updateExpBarMax()
    {
        ExpBar.GetComponent<Slider>().maxValue = PlayerStats.NEXT_LEVEL_EXP;
    }
}
