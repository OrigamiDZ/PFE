using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class UI_MenuManager : MonoBehaviour {

    [SerializeField]
    GameObject InGameUI;
    [SerializeField]
    GameObject MainMenuUI;
    [SerializeField]
    GameObject MissionMenuUI;
    [SerializeField]
    GameObject InfoMenuUI;
    [SerializeField]
    GameObject OptionMenuUI;

    [SerializeField]
    GameObject[] MissionPagesUI_array;
    [SerializeField]
    GameObject[] InfoPagesUI_array;


    private GameObject currentUI = null;



    private void Start()
    {
        currentUI = InGameUI;
    }


    private void Update()
    {
        if(currentUI.activeSelf == false)
        {
            currentUI.SetActive(true);
        }
    }

    public void BackToGame()
    {
        if (currentUI != null)
        {
            currentUI.SetActive(false);
        }
        currentUI = InGameUI;
    }


    public void OnClickToMainMenu()
    {
        if(currentUI != null)
        {
            currentUI.SetActive(false);
        }
        MainMenuUI.SetActive(true);
        currentUI = MainMenuUI;
    }

    public void OnClickToMissionMenu()
    {
        if (currentUI != null)
        {
            currentUI.SetActive(false);
        }
        MissionMenuUI.SetActive(true);
        currentUI = MissionMenuUI;
    }

    public void OnClickToInfoMenu()
    {
        if (currentUI != null)
        {
            currentUI.SetActive(false);
        }
        InfoMenuUI.SetActive(true);
        currentUI = InfoMenuUI;
    }

    public void OnClickToOptionMenu()
    {
        if (currentUI != null)
        {
            currentUI.SetActive(false);
        }
        OptionMenuUI.SetActive(true);
        currentUI = OptionMenuUI;
    }

    public void OnClickToMissionPage(int id_page)
    {
        Assert.AreNotEqual(MissionPagesUI_array.Length, 0);
        Assert.IsTrue(id_page < MissionPagesUI_array.Length);
        if (currentUI != null)
        {
            currentUI.SetActive(false);
        }
        MissionPagesUI_array[id_page].SetActive(true);
        currentUI = MissionPagesUI_array[id_page];
    }

    public void OnClickToInfoPage(int id_page)
    {
        Assert.AreNotEqual(InfoPagesUI_array.Length, 0);
        Assert.IsTrue(id_page < InfoPagesUI_array.Length);
        if (currentUI != null)
        {
            currentUI.SetActive(false);
        }
        InfoPagesUI_array[id_page].SetActive(true);
        currentUI = InfoPagesUI_array[id_page];
    }

}
