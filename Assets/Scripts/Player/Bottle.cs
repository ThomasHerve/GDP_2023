using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField]
    GameObject decalPrefab;
    [SerializeField]
    AudioSource bottleSource;
    [SerializeField]
    AudioClip[] bottleClip;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject.FindGameObjectWithTag("LeftHand").GetComponent<BottleLauncher>().RegisterBottle(this.gameObject);
    }

    public void Throw()
    {
        transform.SetParent(null, true);

        PlayerStats.isBottleUp = false;
        gameObject.AddComponent<Rigidbody>().AddForce(transform.forward * PlayerStats.throwForce, ForceMode.Impulse);

    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Plane")
        {
            bottleSource.PlayOneShot(bottleClip[Random.Range(0, bottleClip.Length)]);

            Vector3 contactPoint = new Vector3(transform.position.x, 0.1f, transform.position.z);

            Collider[] colliders = Physics.OverlapSphere(contactPoint, PlayerStats.bottleDamageRadius);
            foreach (Ennemy ennemy in colliders.Select(collider => collider.GetComponent<Ennemy>()).Where(ennemy => ennemy != null))
            {
                ennemy.Blast();
            }

            GameObject decal = Instantiate(decalPrefab, contactPoint, Quaternion.identity);
            Destroy(decal, 10f);

            Destroy(gameObject.GetComponent<Rigidbody>());
            transform.position = new Vector3(0, -1, 0);
        }
    }



}