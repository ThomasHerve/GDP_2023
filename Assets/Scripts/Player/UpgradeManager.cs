using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerStats;

public class UpgradeManager : MonoBehaviour
{   
    int[] upgrades = new int[3];


    public void buildUpgrades()
    {
        PlayerStats.pause = true;
        Time.timeScale = 0;
        Transform panel = transform.Find("LevelUpPanel");

        upgrades[0] = Random.Range(0, System.Enum.GetNames(typeof(UpgradableStats)).Length);
        upgrades[1] = Random.Range(0, System.Enum.GetNames(typeof(UpgradableStats)).Length);
        upgrades[2] = Random.Range(0, System.Enum.GetNames(typeof(UpgradableStats)).Length);

        panel.Find("ButtonA").GetComponentInChildren<TextMeshProUGUI>().text = ((UpgradableStats)upgrades[0]).ToString();
        panel.Find("ButtonB").GetComponentInChildren<TextMeshProUGUI>().text = ((UpgradableStats)upgrades[1]).ToString();
        panel.Find("ButtonC").GetComponentInChildren<TextMeshProUGUI>().text = ((UpgradableStats)upgrades[2]).ToString();

    }

    public void selectUpgrade(int upgrade)
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);

        string fieldName = ((UpgradableStats)upgrades[upgrade]).ToString();

        FieldInfo field = typeof(PlayerStats).GetField(fieldName, BindingFlags.Static | BindingFlags.Public);
        FieldInfo upgradeField = typeof(PlayerStats).GetField(fieldName + "Augment", BindingFlags.Static | BindingFlags.Public);

        field.SetValue(null, (int)field.GetValue(null) + (int)upgradeField.GetValue(null));

        upgrades = new int[3];
        PlayerStats.pause = false;
    }


}
