using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class facePlayer : MonoBehaviour 
{
    private GameManager gameManager = null;
    private GameObject player = null;

	// Use this for initialization
	void Awake() 
    {
        gameManager = FindObjectOfType<GameManager>();
        player = gameManager.player;
	}
	
	// Update is called once per frame
	void Update() 
    {
        if (player != null)
        {
            Vector2 forward = player.transform.position - this.transform.position;
            this.transform.up = forward;
        }
	}
}
