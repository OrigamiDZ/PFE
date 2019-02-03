using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//UI manager for the tutorial scene
public class Tutorial_UImanager : MonoBehaviour {

    //Explanation slides array
    [SerializeField]
    GameObject[] tutoSlides;

    //GameObject for the testing part
    [SerializeField]
    GameObject testingPhaseGameobject;

    //Current displayed slide
    private GameObject currentSlide;

    //Current displayed slide ID
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


    //Next slide button function
    public void OnClickNextButton()
    {
        if(currentSlideId < tutoSlides.Length - 1)
        {
            currentSlide.SetActive(false);
            currentSlideId++;
            currentSlide = tutoSlides[currentSlideId];
            currentSlide.SetActive(true);
        }
    }

    //Previous slide button function
    public void OnClickPreviousButton()
    {
        if (currentSlideId > 0)
        {
            currentSlide.SetActive(false);
            currentSlideId--;
            currentSlide = tutoSlides[currentSlideId];
            currentSlide.SetActive(true);
        }
    }


    //Start the testing part button function
    public void OnClickStartTestingPhaseButton()
    {
        currentSlide.SetActive(false);
        testingPhaseGameobject.SetActive(true);
    }
}
