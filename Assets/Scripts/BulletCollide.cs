using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollide : MonoBehaviour 
{
    public GameObject explosion = null;
    //private GameObject _explosion = null;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(explosion, this.transform.position, Quaternion.Euler(Vector3.zero));
    }
}
