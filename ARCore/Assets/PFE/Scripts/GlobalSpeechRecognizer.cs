﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using AUP;
using UnityEngine.SceneManagement;


//Speech recognizer class
public class GlobalSpeechRecognizer : MonoBehaviour
{

    //Debugging tag
    private const string TAG = "[SpeechRecognizerDemo]: ";

    //Speech recognition plugin
    private SpeechPlugin speechPlugin;

    //Text canvas for speech recognition result
    public Text resultText;

    //Text canavs for speech recognition partial result
    public Text partialResultText;

    //Text canvas for speech recognition status
    public Text statusText;

    //Canvas for avatar's output
    public GameObject outputBihou;

    //Action dispatcher
    private Dispatcher dispatcher;

    //Utils plugin
    private UtilsPlugin utilsPlugin;

    //Avatar
    private GameObject Bihou;

    //Avatar's (bad) humour database
    private String[] jokes =
        { "Quelle est la différence entre un pigeon ? Il ne sait ni voler !",
         "Quelle est la différence entre un pigeon ? Il a les 2 pattes de la même longueur, surtout la gauche !",
         "Quel dinosaure était toujours rejeté par les femelles ? Le tyranosaure ex !",
         "Que dit un dinosaure lorsqu'il ne comprend rien à la conversation ? Il dit qu'il ne voit pas le raptor !",
         "C'est l'histoire d'un pingouin qui respire par les fesses. Un jour il s'asseoit et il meurt.",
         "C'est l'histoire d'un canibale. Il court, il court, et il se mange !",
         "Qu'est-ce qu'un oiseau migrateur ? Un oiseau qui se gratte que d'un seul côté !",
         "Pourquoi les canards sont toujours à l'heure ? Parce qu'ils sont dans l'étang.",
         "C'est quoi un petit pois avec une épée face à une carotte avec une épée ? Un bon duel.",
         "Pourquoi les belges prennent du pain pour aller aux toilettes ? C’est pour nourrir le canard WC !",
         "Vous avez vu les canards ? Il y en a dans le coin-coin.",
         "Deux canards discutent \"Coin!\" \"J'allais le dire!\"",
         "Qu'est ce qui fait \"nioc nioc\" ? Un canard qui parle en verlan.",
         "Un jour un bonhomme voit un canard mort au bord de la route. Il s'est dit que c'était un signe, mais en fait c'était un canard.",
         "C'est deux grains de sable qui discutent dans un désert. L'un dit à l'autre : \"Te retourne pas, je crois qu'on est suivis !\"",
         "Qu'est-ce qui fait toin toin ? Un tanard !",
         "Avec quoi ramasse-t-on la papaye ? Avec une foufourche !"
        };

    //Current joke ID
    private int jokeID;

    //Audio for the easter egg command "autobus"
    public AudioSource easterEggAutobus;

    //Audio for the easter egg command "raptor"
    public AudioSource easterEggRaptor;

    //Audio game object of the current scene music theme
    private GameObject sceneTheme = null;

    //Has the first (terrible) joke been told yet
    private bool demoTerribleJoke = false;



    //Dispatcher called on awake to assert manual commands work too
    private void Awake()
    {
        dispatcher = Dispatcher.GetInstance();
    }


    void Start()
    {
        if (AndroidRuntimePermissions.CheckPermission("android.permission.RECORD_AUDIO") != AndroidRuntimePermissions.Permission.Granted) {
            AndroidRuntimePermissions.RequestPermission("android.permission.RECORD_AUDIO");
        }
        if (AndroidRuntimePermissions.CheckPermission("android.permission.WRITE_EXTERNAL_STORAGE") != AndroidRuntimePermissions.Permission.Granted) {
            AndroidRuntimePermissions.RequestPermission("android.permission.WRITE_EXTERNAL_STORAGE");
        }
        if (GameObject.FindGameObjectsWithTag("SceneThemeAudio").Length > 0) { sceneTheme = GameObject.FindGameObjectsWithTag("SceneThemeAudio")[0]; }

        Bihou = GameObject.FindGameObjectWithTag("Bihou");

        dispatcher = Dispatcher.GetInstance();
        // for accessing audio
        utilsPlugin = UtilsPlugin.GetInstance();
        utilsPlugin.SetDebug(0);

        speechPlugin = SpeechPlugin.GetInstance();
        speechPlugin.SetDebug(0);
        speechPlugin.Init();

        // set the calling package this is optional 
        // you can use this if your app is for children or kids
        speechPlugin.SetCallingPackage("com.mycoolcompany.mygame");

        AddSpeechPluginListener();
    }



    private void OnEnable()
    {
        AddSpeechPluginListener();
    }



    private void OnDisable()
    {
        RemoveSpeechPluginListener();
    }



    private void AddSpeechPluginListener()
    {
        if (speechPlugin != null)
        {
            //add speech recognizer listener
            speechPlugin.onReadyForSpeech += onReadyForSpeech;
            speechPlugin.onBeginningOfSpeech += onBeginningOfSpeech;
            speechPlugin.onEndOfSpeech += onEndOfSpeech;
            speechPlugin.onError += onError;
            speechPlugin.onResults += onResults;
            speechPlugin.onPartialResults += onPartialResults;
        }
    }



    private void RemoveSpeechPluginListener()
    {
        if (speechPlugin != null)
        {
            //remove speech recognizer listener
            speechPlugin.onReadyForSpeech -= onReadyForSpeech;
            speechPlugin.onBeginningOfSpeech -= onBeginningOfSpeech;
            speechPlugin.onEndOfSpeech -= onEndOfSpeech;
            speechPlugin.onError -= onError;
            speechPlugin.onResults -= onResults;
            speechPlugin.onPartialResults -= onPartialResults;
        }
    }

    private void OnApplicationPause(bool val)
    {
        if (speechPlugin != null)
        {
            if (val)
            {
                RemoveSpeechPluginListener();
            }
            else
            {
                AddSpeechPluginListener();
            }
        }
    }


    //Called by the "listening" button
    public void StartListening()
    {
        outputBihou.SetActive(false);
        StopAllCoroutines();

        bool isSupported = speechPlugin.CheckSpeechRecognizerSupport();

        if (isSupported)
        {
            //number of possible results
            //Note: sometimes even you put 5 numberOfResults, there's a chance that it will be only 3 or 2
            //it is not constant.

            // unmute beep
            utilsPlugin.UnMuteBeep();

            // enable offline
            //speechPlugin.EnableOffline(true);

            // enable partial Results
            speechPlugin.EnablePartialResult(true);

            int numberOfResults = 5;
            speechPlugin.StartListening(numberOfResults);

            //by activating this, the Speech Recognizer will start and you can start Speaking or saying something 
            //speech listener will stop automatically especially when you stop speaking or when you are speaking 
            //for a long time
        }
        else
        {
            Debug.Log(TAG + "Speech Recognizer not supported by this Android device ");
        }
    }



    public void StartListeningNoBeep()
    {
        bool isSupported = speechPlugin.CheckSpeechRecognizerSupport();

        if (isSupported)
        {
            //number of possible results
            //Note: sometimes even you put 5 numberOfResults, there's a chance that it will be only 3 or 2
            //it is not constant.

            // mute beep
            utilsPlugin.MuteBeep();

            // enable offline
            //speechPlugin.EnableOffline(true);

            // enable partial Results
            speechPlugin.EnablePartialResult(true);

            int numberOfResults = 5;
            speechPlugin.StartListening(numberOfResults);
            ///speechPlugin.StartListeningNoBeep(numberOfResults,true);

            //by activating this, the Speech Recognizer will start and you can start Speaking or saying something 
            //speech listener will stop automatically especially when you stop speaking or when you are speaking 
            //for a long time
        }
        else
        {
            Debug.Log(TAG + "Speech Recognizer not supported by this Android device ");
        }
    }




    //cancel speech
    public void CancelSpeech()
    {
        if (speechPlugin != null)
        {
            bool isSupported = speechPlugin.CheckSpeechRecognizerSupport();

            if (isSupported)
            {
                speechPlugin.Cancel();
            }
        }

        Debug.Log(TAG + " call CancelSpeech..  ");
    }




    public void StopListening()
    {
        if (speechPlugin != null)
        {
            speechPlugin.StopListening();
        }
        Debug.Log(TAG + " StopListening...  ");
    }



    public void StopCancel()
    {
        if (speechPlugin != null)
        {
            speechPlugin.StopCancel();
        }
        Debug.Log(TAG + " StopCancel...  ");
    }



    private void DelayUnMute()
    {
        utilsPlugin.UnMuteBeep();
    }



    private void OnDestroy()
    {
        RemoveSpeechPluginListener();
        speechPlugin.StopListening();
    }



    private void UpdateStatus(string status)
    {
        if (statusText != null)
        {
            statusText.text = String.Format("Status: {0}", status);
        }
    }



    //SpeechRecognizer Events
    private void onReadyForSpeech(string data)
    {
        dispatcher.InvokeAction(
            () => {
                if (speechPlugin != null)
                {
                    //Disables modal
                    speechPlugin.EnableModal(false);
                }

                UpdateStatus(data.ToString());
            }
        );
    }




    private void onBeginningOfSpeech(string data)
    {
        dispatcher.InvokeAction(
            () => {
                UpdateStatus(data.ToString());
                if (resultText != null)
                {
                    //resultText.text = data.ToString(); //"Status: incoming words";
                }
            }
        );
    }




    private void onEndOfSpeech(string data)
    {
        dispatcher.InvokeAction(
            () => {
                UpdateStatus(data.ToString());
            }
        );
    }


    private void onError(int data)
    {
        dispatcher.InvokeAction(
            () => {
                // unmute beep
                CancelInvoke("DelayUnMute");
                Invoke("DelayUnMute", 0.3f);

                SpeechRecognizerError error = (SpeechRecognizerError)data;
                UpdateStatus(error.ToString());

                if (resultText != null)
                {
                    resultText.text = "Status: Unrecognized words";
                }
            }
        );
    }



    public void onResults(string data)
    {
        dispatcher.InvokeAction(
            () => {
                if (resultText != null)
                {
                    // unmute beep
                    CancelInvoke("DelayUnMute");
                    Invoke("DelayUnMute", 0.3f);

                    string[] results = data.Split(',');
                    Debug.Log(TAG + " result length " + results.Length);

                    //when you set morethan 1 results index zero is always the closest to the words the you said
                    //but it's not always the case so if you are not happy with index zero result you can always 
                    //check the other index

                    //sample on checking other results
                    foreach (string possibleResults in results)
                    {
                        Debug.Log(TAG + " possibleResults " + possibleResults);
                    }

                    //sample showing the nearest result
                    string whatToSay = results.GetValue(0).ToString();


                    //List of commands recognized
                    // -> au revoir, autobus (easter egg), bonjour, blague, événement, info, mode libre, looping, mission, option, raptor (easter egg), retour, tutoriel

                    //Avatar says hello back
                    if (whatToSay.Contains("bonjour") || whatToSay.Contains("Bonjour"))
                    {
                        outputBihou.SetActive(true);
                        outputBihou.GetComponentInChildren<Text>().text = "Coucou !";
                        StartCoroutine("waiterBihouOutput");
                    }

                    //Avatar does a looping
                    else if(whatToSay.Contains("looping") || whatToSay.Contains("Looping"))
                    {
                        Bihou.GetComponent<AnimatorScript>().looping = true;
                    }

                    //Avatar tells a joke starting by a terrible one
                    else if (whatToSay.Contains("blague") || whatToSay.Contains("Blague"))
                    {
                        if (!demoTerribleJoke)
                        {
                            demoTerribleJoke = true;
                            outputBihou.SetActive(true);
                            outputBihou.GetComponentInChildren<Text>().text = "Je n'ai pas compris votre question. Nan je rigole, c'était une blague !";
                            StartCoroutine("waiterBihouOutput");
                        }
                        else
                        {
                            int prevJokeID = jokeID;
                            jokeID = UnityEngine.Random.Range(0, jokes.Length);
                            while (prevJokeID == jokeID)
                            {
                                jokeID = UnityEngine.Random.Range(0, jokes.Length);
                            }
                            outputBihou.SetActive(true);
                            String str = jokes[jokeID];
                            outputBihou.GetComponentInChildren<Text>().text = str;
                            StartCoroutine("waiterBihouOutput");
                        }
                    } 

                    //Shows the missions and events menu
                    else if (whatToSay.Contains("mission") || whatToSay.Contains("Mission"))
                    {
                        transform.GetComponentInParent<UI_MenuManager>().OnClickToMissionMenu();
                        AppController.control.inSpeechRecoMode = false;
                    }

                    //Shows the missions and events menu
                    else if (whatToSay.Contains("événement") || whatToSay.Contains("Événement"))
                    {
                        transform.GetComponentInParent<UI_MenuManager>().OnClickToMissionMenu();
                        AppController.control.inSpeechRecoMode = false;
                    }

                    //Gets back to the current scene
                    else if (whatToSay.Contains("retour") || whatToSay.Contains("Retour"))
                    {
                        transform.GetComponentInParent<UI_MenuManager>().BackToGame();
                        AppController.control.inSpeechRecoMode = false;
                    }

                    //Shows the option menu
                    else if (whatToSay.Contains("option") || whatToSay.Contains("Option"))
                    {
                        transform.GetComponentInParent<UI_MenuManager>().OnClickToOptionMenu();
                        AppController.control.inSpeechRecoMode = false;
                    }

                    //Shows the info menu
                    else if (whatToSay.Contains("info") || whatToSay.Contains("Info"))
                    {
                        transform.GetComponentInParent<UI_MenuManager>().OnClickToInfoMenu();
                        AppController.control.inSpeechRecoMode = false;
                    }

                    //Best M.Simatic's interpretation of an autobus (easter egg)
                    else if (whatToSay.Contains("autobus") || whatToSay.Contains("Autobus"))
                    {
                        if (AppController.control.soundOff)
                        {
                            AppController.control.soundOff = false;
                            easterEggAutobus.Play();
                            StartCoroutine("waiterBihouOutputSound");
                        }
                        else
                        {
                            easterEggAutobus.Play();
                        }
                    }

                    //Best interpretation of a raptor (possibly a trex tho) roar (easter egg)
                    else if (whatToSay.Contains("raptor") || whatToSay.Contains("Raptor"))
                    {
                        if (AppController.control.soundOff)
                        {
                            AppController.control.soundOff = false;
                            easterEggRaptor.Play();
                            StartCoroutine("waiterBihouOutputSound");
                        }
                        else
                        {
                            easterEggRaptor.Play();
                        }
                    }

                    //Switch to discovery mode
                    else if (whatToSay.Contains("libre") || whatToSay.Contains("Libre"))
                    {
                        AppController.control.inSpeechRecoMode = false;
                        SceneManager.LoadScene("Discovery");
                    }

                    //Quits the application
                    else if (whatToSay.Contains("revoir") || whatToSay.Contains("Revoir"))
                    {
                        AppController.control.inSpeechRecoMode = false;
                        Application.Quit();
                    }

                    //Restarts the tutoriel
                    else if (whatToSay.Contains("tutoriel") || whatToSay.Contains("Tutoriel"))
                    {
                        AppController.control.inSpeechRecoMode = false;
                        AppController.control.tutorialDone = false;
                        SceneManager.LoadScene("Tutorial");
                    }

                    //None of the above
                    else
                    {
                        resultText.text = string.Format("Result: {0}", whatToSay);
                        Bihou.GetComponent<AnimatorScript>().confused = true;
                    }
                }
            }
        );
    }

    //Waiter for avatar's written output
    IEnumerator waiterBihouOutput()
    {
        yield return new WaitForSeconds(8);
        outputBihou.SetActive(false);
    }

    //Waiter for avatar's audio output
    IEnumerator waiterBihouOutputSound()
    {
        yield return new WaitForSeconds(8);
        AppController.control.soundOff = true;
    }




    private void onPartialResults(string data)
    {
        dispatcher.InvokeAction(
            () => {
                if (partialResultText != null)
                {
                    string[] results = data.Split(',');
                    Debug.Log(TAG + " partial result length " + results.Length);

                    //when you set morethan 1 results index zero is always the closest to the words the you said
                    //but it's not always the case so if you are not happy with index zero result you can always 
                    //check the other index

                    //sample on checking other results
                    foreach (string possibleResults in results)
                    {
                        Debug.Log(TAG + "partial possibleResults " + possibleResults);
                    }

                    //sample showing the nearest result
                    string whatToSay = results.GetValue(0).ToString();
                    partialResultText.text = whatToSay;
                }
            }
        );
    }
}