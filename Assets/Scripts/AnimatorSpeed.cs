using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorSpeed : MonoBehaviour 
{
    public float animatorSpeed = 1.0f;
    private Animator animator = null;
	
    // Use this for initialization
	void Start () 
    {
        animator = this.GetComponent<Animator>();

        if (animator != null)
        {
            animator.speed = animatorSpeed;
        }
	}	
}
