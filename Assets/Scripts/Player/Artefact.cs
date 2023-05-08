using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artefact : MonoBehaviour
{
    ArtefactUI artefactUI;
    public int restant = 0;

    // Start is called before the first frame update
    void Start()
    {
        artefactUI = GameObject.FindGameObjectWithTag("ArtefactUI").GetComponent<ArtefactUI>();
        artefactUI.Use();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // CODE
        artefactUI.Validate(restant);
        Debug.Log("Artefact");
        Destroy(gameObject);
    }
}
