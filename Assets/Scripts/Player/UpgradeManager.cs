using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerStats;

public class UpgradeManager : MonoBehaviour
{
    public Transform panel;
    int[] upgrades = new int[3];
    UpgradableStats[] backups = new UpgradableStats[3];

    [SerializeField]
    Sprite[] sprites;

    Dictionary<UpgradableStats, int> spriteIndices = new Dictionary<UpgradableStats, int>(){
        {UpgradableStats.hpMax, 0},
        {UpgradableStats.resistance, 1},
        {UpgradableStats.damage, 2},
        {UpgradableStats.bottleSlow, 3},
        {UpgradableStats.lifesteal , 4}
    };
    public void buildUpgrades()
    {
        PlayerStats.pause = true;
        Time.timeScale = 0;
        panel.gameObject.SetActive(true);

        upgrades[0] = Random.Range(0, System.Enum.GetNames(typeof(UpgradableStats)).Length);
        upgrades[1] = upgrades[0];
        while (upgrades[1] == upgrades[0])
            upgrades[1] = Random.Range(0, System.Enum.GetNames(typeof(UpgradableStats)).Length);
        upgrades[2] = upgrades[1];
        while (upgrades[2] == upgrades[1] || upgrades[2] == upgrades[0])
            upgrades[2] = Random.Range(0, System.Enum.GetNames(typeof(UpgradableStats)).Length);

        backups[0] = (UpgradableStats)upgrades[0];
        backups[1] = (UpgradableStats)upgrades[1];
        backups[2] = (UpgradableStats)upgrades[2];
        panel.Find("ButtonA").GetComponent<Image>().sprite = sprites[spriteIndices[backups[0]]];
        panel.Find("ButtonB").GetComponent<Image>().sprite = sprites[spriteIndices[backups[1]]];
        panel.Find("ButtonC").GetComponent<Image>().sprite = sprites[spriteIndices[backups[2]]];
    }

    public void selectUpgrade(int upgrade)
    {
        Time.timeScale = 1;
        panel.gameObject.SetActive(false);
        PlayerStats.ApplyAugment[backups[upgrade]]();
        upgrades = new int[3];
        backups = new UpgradableStats[3];
        PlayerStats.pause = false;
    }


}
