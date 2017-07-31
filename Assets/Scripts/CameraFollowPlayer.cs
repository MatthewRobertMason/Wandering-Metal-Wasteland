﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour 
{
    public GameObject player = null;
    public float minCameraDistance = 0;
    public float maxCameraDistance = 105;

    private GameManager gameManager = null;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

	// Update is called once per frame
	void Update () 
    {
        if (player == null)
        {
            Debug.Log("No Player");
            this.transform.position = new Vector3(0.0f, 0.0f, -10.0f);
        }

        if ((player == null) && (gameManager.player != null))
        {
            Debug.Log("Found Player");
            player = gameManager.player;
        }

        if (player != null)
        {
            Debug.Log("Player not null");
            if ((player.transform.position.y > minCameraDistance) && (player.transform.position.y < maxCameraDistance))
            {
                Debug.Log("1");
                this.transform.position = new Vector3(0.0f, player.transform.position.y, -10.0f);
            }
            if (player.transform.position.y < minCameraDistance)
            {
                Debug.Log("2");
                this.transform.position = new Vector3(0.0f, minCameraDistance, -10.0f);
            }
            if (player.transform.position.y > maxCameraDistance)
            {
                Debug.Log("3");
                this.transform.position = new Vector3(0.0f, maxCameraDistance, -10.0f);
            }
        }
    }
}
