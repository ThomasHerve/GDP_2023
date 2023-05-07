using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField]
    GameObject decalPrefab;

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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Plane")
        {
            Vector3 contactPoint = new Vector3(collision.contacts[0].point.x, 0.1f, collision.contacts[0].point.z) ;

            gameObject.AddComponent<SphereCollider>().radius = PlayerStats.bottleDamageRadius;
            Destroy(gameObject.GetComponent<SphereCollider>(),0.1f);

            GameObject decal = Instantiate(decalPrefab, contactPoint, Quaternion.identity);
            Destroy(decal, 10f);

            Destroy(gameObject.GetComponent<Rigidbody>());
            transform.position = new Vector3(0, -1, 0);
        }
    }



}