using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tuto_UImanager : MonoBehaviour {

    [SerializeField]
    GameObject[] tutoSlides;
    [SerializeField]
    GameObject testingPhaseGameobject;


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
        if(currentSlideId < tutoSlides.Length - 1)
        {
            currentSlide.SetActive(false);
            currentSlideId++;
            currentSlide = tutoSlides[currentSlideId];
            currentSlide.SetActive(true);
        }
    }

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

    public void OnClickStartTestingPhaseButton()
    {
        currentSlide.SetActive(false);
        testingPhaseGameobject.SetActive(true);
    }
}
