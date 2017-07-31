using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnCollisionPower : MonoBehaviour 
{
    public float powerAmount = 0.0f;

    //private GameManager gameManager = null;

    void Awake()
    {
        //gameManager = FindObjectOfType<GameManager>();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();

        if (player != null)
        {
            if (player.PP < player.maxPP)
            {
                player.PP += powerAmount;
            }
        }
    }
}
