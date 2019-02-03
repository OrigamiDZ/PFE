using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;


//UI manager for the UI menu prefab
public class UI_MenuManager : MonoBehaviour {

    //Default in-game UI
    [SerializeField]
    GameObject InGameUI;

    //Main menu UI
    [SerializeField]
    GameObject MainMenuUI;

    //Mission menu UI
    [SerializeField]
    GameObject MissionMenuUI;

    //Info menu UI
    [SerializeField]
    GameObject InfoMenuUI;

    //Option menu UI
    [SerializeField]
    GameObject OptionMenuUI;

    //Speech recognition prefab 
    [SerializeField]
    GameObject SpeechRecoPhase;

    //Commands list UI
    [SerializeField]
    GameObject ListCommandUI;

    //Mission pages UI
    [SerializeField]
    GameObject[] MissionPagesUI_array;
    //page 0 -> mission
    //page 1 -> event

    //Info pages UI
    [SerializeField]
    GameObject[] InfoPagesUI_array;

    //Main music theme for the menu
    [SerializeField]
    AudioSource MenuMainTheme;

    //Slider controlling the sound (on/off)
    [SerializeField]
    Slider soundSlider;

    //Is the player already in the menus
    private bool alreadyInMainMenu = false;

    //Current displayed UI
    private GameObject currentUI = null;

    //Scene music theme
    private GameObject sceneTheme = null;


    private void Start()
    {
        currentUI = InGameUI;
        if (SceneManager.GetActiveScene().name != "Tutorial") { InGameUI.transform.Find("VocalButton").gameObject.SetActive(true); }
        else { InGameUI.transform.Find("VocalButton").gameObject.SetActive(false); }
        if (GameObject.FindGameObjectsWithTag("SceneThemeAudio").Length > 0) { sceneTheme = GameObject.FindGameObjectsWithTag("SceneThemeAudio")[0]; }
    }


    private void Update()
    {
        if (currentUI.activeSelf == false)
        {
            currentUI.SetActive(true);
        }

        /*
        if (AppController.control.missionDone) { MissionPagesUI_array[0].transform.Find("Done").gameObject.SetActive(true); }
        else { MissionPagesUI_array[0].transform.Find("Done").gameObject.SetActive(false); }

        if (AppController.control.eventDone) { MissionPagesUI_array[1].transform.Find("Done").gameObject.SetActive(true); }
        else { MissionPagesUI_array[1].transform.Find("Done").gameObject.SetActive(false); }
        */

    }


    //Return to main game (current scene)
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
        AppController.control.inSpeechRecoMode = false;
        if (sceneTheme != null) { sceneTheme.GetComponent<AudioSource>().Play(); }
    }


    //To main menu button function
    public void OnClickToMainMenu()
    {
        if (!alreadyInMainMenu)
        {
            alreadyInMainMenu = true;
            Time.timeScale = 0;
            MenuMainTheme.Play();
            if (sceneTheme != null) { sceneTheme.GetComponent<AudioSource>().Stop(); }
        }
        if (currentUI != null)
        {
            currentUI.SetActive(false);
        }
        MainMenuUI.SetActive(true);
        currentUI = MainMenuUI;
    }


    //To mission menu button function
    public void OnClickToMissionMenu()
    {
        if (!alreadyInMainMenu)
        {
            alreadyInMainMenu = true;
            Time.timeScale = 0;
            MenuMainTheme.Play();
            if (sceneTheme != null) { sceneTheme.GetComponent<AudioSource>().Stop(); }
        }
        if (currentUI != null)
        {
            currentUI.SetActive(false);
        }
        MissionMenuUI.SetActive(true);
        currentUI = MissionMenuUI;
    }


    //To info menu button function
    public void OnClickToInfoMenu()
    {
        if (!alreadyInMainMenu)
        {
            alreadyInMainMenu = true;
            Time.timeScale = 0;
            MenuMainTheme.Play();
            if (sceneTheme != null) { sceneTheme.GetComponent<AudioSource>().Stop(); }
        }
        if (currentUI != null)
        {
            currentUI.SetActive(false);
        }
        InfoMenuUI.SetActive(true);
        currentUI = InfoMenuUI;
    }


    //To option menu button function
    public void OnClickToOptionMenu()
    {
        if (AppController.control.soundOff) { soundSlider.value = 0; } else { soundSlider.value = 1; }
        if (!alreadyInMainMenu)
        {
            alreadyInMainMenu = true;
            Time.timeScale = 0;
            MenuMainTheme.Play();
            if (sceneTheme != null) { sceneTheme.GetComponent<AudioSource>().Stop(); }
        }
        if (currentUI != null)
        {
            currentUI.SetActive(false);
        }
        OptionMenuUI.SetActive(true);
        currentUI = OptionMenuUI;
    }


    //To mission page no [id_page] button function
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
        if (id_page == 0 && !AppController.control.missionDone)
        {
            MissionPagesUI_array[0].transform.Find("Done").gameObject.SetActive(false);
        }
        if (id_page == 1 && !AppController.control.eventDone)
        {
            MissionPagesUI_array[1].transform.Find("Done").gameObject.SetActive(false);
        }
    }


    //To mission page no [id_page] button function
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


    //To speech recognition mode button function
    public void OnClickToSpeechRecoPhase()
    {
        Time.timeScale = 1;
        if (sceneTheme != null) { sceneTheme.GetComponent<AudioSource>().Stop(); }
        if (currentUI != null)
        {
            currentUI.SetActive(false);
        }
        SpeechRecoPhase.SetActive(true);
        currentUI = SpeechRecoPhase;
        AppController.control.inSpeechRecoMode = true;
    }


    //Start mission button function
    public void StartMission()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GoToGymnase");
    }


    //Start event button function
    public void StartEvent()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GoToENSIIE");
    }


    //Start tutorial button function
    public void StartTutorial()
    {
        Time.timeScale = 1;
        AppController.control.tutorialDone = false;
        SceneManager.LoadScene("Tutorial");
    }


    //Start discovery button function
    public void StartDiscovery()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Discovery");
    }


    //Sound slider manager (on/off)
    public void SoundOptionManager(Slider slider)
    {
        if(slider.value == 1)
        {
            AppController.control.soundOff = false;
        }
        else if (slider.value == 0)
        {
            AppController.control.soundOff = true;
        }
    }
    

    //To commands list button function
    public void OnClickToListCommand()
    {
        AppController.control.inSpeechRecoMode = false;
        if (!alreadyInMainMenu)
        {
            alreadyInMainMenu = true;
            Time.timeScale = 0;
            MenuMainTheme.Play();
            if (sceneTheme != null) { sceneTheme.GetComponent<AudioSource>().Stop(); }
        }
        if (currentUI != null)
        {
            currentUI.SetActive(false);
        }
        ListCommandUI.SetActive(true);
        currentUI = ListCommandUI;
    }


    //Trigger command [str] button function
   public void OnClickButtonListCommand(string str)
    {
        OnClickToSpeechRecoPhase();
        MenuMainTheme.Stop();
        alreadyInMainMenu = false;
        SpeechRecoPhase.GetComponent<GlobalSpeechRecognizer>().onResults(str);
    }
}
