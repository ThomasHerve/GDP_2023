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
            Vector3 contactPoint = collision.contacts[0].point;
            GameObject decal = Instantiate(decalPrefab, new Vector3(contactPoint.x, 0.1f, contactPoint.z), Quaternion.identity);
            Destroy(decal, 10f);

            Destroy(gameObject.GetComponent<Rigidbody>());
            transform.position = new Vector3(0, -1, 0);
        }
    }
}