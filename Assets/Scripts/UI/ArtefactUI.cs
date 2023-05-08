using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArtefactUI : MonoBehaviour
{

    [SerializeField]
    Color textColor;
    [SerializeField]
    string text1;
    [SerializeField]
    string text2;
    [SerializeField]
    float time;

    TextMeshProUGUI textMeshProUGUI;

    // Start is called before the first frame update
    void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.text = "";
    }

    public void Use()
    {
        textMeshProUGUI.text = text1;
        StartCoroutine(EffectCoroutine());
    }

    public void Validate(int restant)
    {
        textMeshProUGUI.text = text2;
        StartCoroutine(EffectCoroutine());
    }

    private IEnumerator EffectCoroutine()
    {
        float currentTime = 0;
        while(currentTime < time)
        {
            currentTime += Time.deltaTime;
            textMeshProUGUI.color = new Color(textColor.r, textColor.g, textColor.b, 255 * (time - currentTime )/ time);
            yield return null;
        }

    }
}
