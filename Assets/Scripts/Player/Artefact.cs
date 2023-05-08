using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artefact : MonoBehaviour
{
    [SerializeField]
    AnimationCurve curve;

    ArtefactUI artefactUI;
    public int restant = 0;

    float time = 1;
    float currentTime = 0;

    Vector3 originalPosition;

    GameLoop gameLoop;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        artefactUI = GameObject.FindGameObjectWithTag("ArtefactUI").GetComponent<ArtefactUI>();
        gameLoop = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameLoop>();
        artefactUI.Use();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= time)
        {
            currentTime = 0;
        }
        transform.position = new Vector3(originalPosition.x, originalPosition.y + curve.Evaluate(currentTime), originalPosition.z);

    }

    private void OnTriggerEnter(Collider other)
    {
        // CODE
        artefactUI.Validate(restant);
        Debug.Log("Artefact");
        gameLoop.GetArtefact();
        Destroy(gameObject);
    }
}
