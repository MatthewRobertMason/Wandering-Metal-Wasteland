using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnTriggerLevelChange : MonoBehaviour 
{
    public string sceneName = null;
    private GameManager gameManager = null;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if ((sceneName != null) && (player != null))
        {
            gameManager.playTitle();
            gameManager.NextLevel(sceneName);
        }
    }
}
