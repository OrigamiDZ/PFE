using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorScript : MonoBehaviour {

    public Animator animator;
    public AudioSource audioLooping;
    public AudioSource audioIdle;
    public AudioSource audioMisunderstand;

    public bool land = false;
    public bool takeoff = false;
    public bool confused = false;
    public bool looping = false;

    private bool isAudioPlayingIdle = false;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !isAudioPlayingIdle) {
            audioIdle.enabled =true;
        }

        //GESTION DE L'ATERRISSAGE
        if (land)
        {
            animator.SetBool("isLanding", true);
            audioIdle.enabled = false;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Landing"))
        {
            animator.SetBool("isLanding", false);
            land = false;
        }

        //GESTION DU DÉCOLLAGE
        if (takeoff)
        {
            animator.SetBool("isTakingOff", true);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("TakeOff"))
        {
            animator.SetBool("isTakingOff", false);
            takeoff = false;
        }

        //GESTION DE L'EXPRESSION D'INCOMPRÉHENSION
        if (confused && (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")))
        {
            animator.SetBool("isMisunderstanding", true);
            audioMisunderstand.Play();
            audioIdle.enabled = false;
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Misunderstanding"))
        {
            animator.SetBool("isMisunderstanding", false);
            confused = false;
        }

        //GESTION DU LOOPING
        if (looping && (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")))
        {
            animator.SetBool("isLooping", true);
            audioLooping.Play();
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Looping"))
        {
            animator.SetBool("isLooping", false);
            looping = false;
        }
    }
}
