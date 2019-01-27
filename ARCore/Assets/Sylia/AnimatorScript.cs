using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorScript : MonoBehaviour {

    public Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        //GESTION DE L'ATERRISSAGE
        if (Input.GetKey("down"))
        {
            animator.SetBool("isLanding", true);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Landing"))
        {
            animator.SetBool("isLanding", false);
        }

        //GESTION DU DÉCOLLAGE
        if (Input.GetKey("up"))
        {
            animator.SetBool("isTakingOff", true);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("TakeOff"))
        {
            animator.SetBool("isTakingOff", false);
        }

        //GESTION DE L'EXPRESSION D'INCOMPRÉHENSION
        if (Input.GetKey("left") && (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")))
        {
            animator.SetBool("isMisunderstanding", true);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Misunderstanding"))
        {
            animator.SetBool("isMisunderstanding", false);
        }

        //GESTION DU LOOPING
        if (Input.GetKey("right") && (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")))
        {
            animator.SetBool("isLooping", true);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Looping"))
        {
            animator.SetBool("isLooping", false);
        }
    }
}
