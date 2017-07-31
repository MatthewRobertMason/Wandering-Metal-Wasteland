using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollide : MonoBehaviour 
{
    public GameObject explosion = null;

    void OnDestroy()
    {
        Instantiate(explosion, this.transform.position, Quaternion.Euler(Vector3.zero));
    }
}
