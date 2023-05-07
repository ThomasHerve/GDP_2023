using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartUI : MonoBehaviour
{
    [SerializeField]
    AnimationCurve curve;
    [SerializeField]
    float amplitude;

    float time;
    float currentTime = 0;

    TextMeshProUGUI textMeshPro;

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();
        time = curve.keys[curve.keys.Length - 1].time;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= time)
        {
            currentTime = 0;
        }
        textMeshPro.color = new Color32((byte)255, (byte)Mathf.FloorToInt(curve.Evaluate(currentTime) * 255), (byte)Mathf.FloorToInt(curve.Evaluate(currentTime) * 255), 255);
    }
}
