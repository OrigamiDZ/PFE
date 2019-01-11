using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tuto_UImanager : MonoBehaviour {

    [SerializeField]
    GameObject[] tutoSlides;

    private GameObject currentSlide;
    private int currentSlideId;

    private void Start()
    {
        if(tutoSlides.Length > 0)
        {
            currentSlide = tutoSlides[0];
            currentSlideId = 0;
            currentSlide.SetActive(true);
        }
    }

    public void OnClickNextButton()
    {
        
    }
}
