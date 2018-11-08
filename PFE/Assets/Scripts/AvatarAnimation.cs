using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AvatarActions
{
    /*looping,
    joke,
    fearAnimation,
    happyAnimation,*/
    joyAnimation,
    none
};
public class AvatarAnimation : MonoBehaviour {

    private static AvatarActions avatarCurrentAction = AvatarActions.none;

    public AvatarActions AvatarCurrentAction {
        get {
            return avatarCurrentAction;
        }

        set {
            avatarCurrentAction = value;
        }
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
