using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

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
    //page 0 -> mission
    //page 1 -> event
    [SerializeField]
    GameObject[] InfoPagesUI_array;

    [SerializeField]
    AudioSource MenuMainTheme;

    private bool alreadyInMainMenu = false;
    private GameObject currentUI = null;
    private GameObject sceneTheme;


    private void Start()
    {
        currentUI = InGameUI;
        if (SceneManager.GetActiveScene().name == "Discovery") { InGameUI.transform.Find("VocalButton").gameObject.SetActive(true); }
        else { InGameUI.transform.Find("VocalButton").gameObject.SetActive(false); }
        if (GameObject.FindGameObjectsWithTag("SceneThemeAudio").Length > 0) { sceneTheme = GameObject.FindGameObjectsWithTag("SceneThemeAudio")[0]; }
    }


    private void Update()
    {
        if (currentUI.activeSelf == false)
        {
            currentUI.SetActive(true);
        }

        if (AppController.control.missionDone) { MissionPagesUI_array[0].transform.Find("Done").gameObject.SetActive(true); }
        else { MissionPagesUI_array[0].transform.Find("Done").gameObject.SetActive(false); }

        if (AppController.control.eventDone) { MissionPagesUI_array[1].transform.Find("Done").gameObject.SetActive(true); }
        else { MissionPagesUI_array[1].transform.Find("Done").gameObject.SetActive(false); }
    }

    public void BackToGame()
    {
        MenuMainTheme.Stop();
        alreadyInMainMenu = false;
        if (currentUI != null)
        {
            currentUI.SetActive(false);
        }
        currentUI = InGameUI;
        Time.timeScale = 1;
        sceneTheme.GetComponent<AudioSource>().Play();
    }


    public void OnClickToMainMenu()
    {
        if (!alreadyInMainMenu)
        {
            alreadyInMainMenu = true;
            Time.timeScale = 0;
            MenuMainTheme.Play();
            sceneTheme.GetComponent<AudioSource>().Stop();
        }
        if (currentUI != null)
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
