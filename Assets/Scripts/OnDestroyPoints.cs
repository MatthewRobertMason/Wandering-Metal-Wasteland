using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroyPoints : MonoBehaviour 
{
    public int points = 0;

    private GameManager gameManager = null;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnDestroy()
    {
        gameManager.addScore(points);
    }
}
