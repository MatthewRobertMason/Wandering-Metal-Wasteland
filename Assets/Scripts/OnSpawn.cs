using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OnSpawn : MonoBehaviour 
{
    public AudioClip awakeSound = null;

    void Awake()
    {
        if (awakeSound != null)
            this.GetComponent<AudioSource>().PlayOneShot(awakeSound);
    }
}
