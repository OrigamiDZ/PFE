using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Class managing scrollbar movements according to scrollview movements
public class ScrollviewAndScrollbar : MonoBehaviour {

    //The scrollbar to move
    public Scrollbar scrollb;

    //Starting height (scrollbar value = 1)
    public float minY;

    //Ending height (scrollbar value = 0)
    public float maxY;

    //Range used to normalize value
    private float range;


	void Start () {
        range = maxY - minY;
	}
	

	void Update () {
        scrollb.value = (GetComponent<RectTransform>().position.y - minY) / range;
        Debug.Log("y = " + GetComponent<RectTransform>().position.y.ToString());
    }
}
