using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour 
{
    private static GameManager instance = null;
	// Use this for initialization
	void Start () 
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}
}
