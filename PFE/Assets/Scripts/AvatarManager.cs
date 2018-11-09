using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AvatarManager : MonoBehaviour {

    [SerializeField] private float maxTimeBeforeNewMove = 10;
    private float lastTimeAction = Time.time;
    private AvatarAnimation avatarAnimation;
    private AvatarActions avatarCurrentAction = AvatarActions.none;
    
    void Start () {
        avatarAnimation = this.GetComponent<AvatarAnimation>();
        avatarCurrentAction = avatarAnimation.AvatarCurrentAction;

    }

    void Update () {
		if (avatarCurrentAction == AvatarActions.joyAnimation) {
            Debug.Log("Joy Animation");
            lastTimeAction = Time.time;
        }

        if (Time.time - lastTimeAction > maxTimeBeforeNewMove) {
            Debug.Log("Joy Animation automatique");
            lastTimeAction = Time.time;
        }
	}
}
