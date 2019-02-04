using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/* The controller for the app.
 * Only one instance of it.
 * Used for data persistence, states, audio.
 */
public class AppController : MonoBehaviour {

    public static AppController control;
    public bool tutorialDone;
    public bool missionDone;
    public bool eventDone;
    public double targetPointGPS;
    public int currentObjectiveDoneEvent;
    public bool inSpeechRecoMode;
    public bool soundOff;

    void Awake () {
        //1st time running app
        //Provides only one instance of the AppController gameObject
        if(control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
	}

    public void Start()
    {
        //Load game data
        Load();

        //Mute sound if needed
        if (soundOff) { AudioListener.volume = 0f; } else { AudioListener.volume = 1f; }

        //State of the event
        if(currentObjectiveDoneEvent == 0) { eventDone = false; }

        //State of the tutorial
        if (!tutorialDone)
        {
            SceneManager.LoadScene("Tutorial");
        }
        else
        {
            SceneManager.LoadScene("Discovery");
        }
    }

    private void Update()
    {
        //Mute sound during the game
        if (soundOff) { AudioListener.volume = 0f; } else { AudioListener.volume = 1f; }
    }


    //On Android device, OnApplicationQuit isn't always called. It's advised to use OnApplicationPause instead.
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Save();
        }
    }


    //Used for the vocal command "au revoir"
    private void OnApplicationQuit()
    {
        Save();
    }



    //Save game data in CIRA.dat
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        file = File.Create(Application.persistentDataPath + "/CIRA.dat");

        PlayerData data = new PlayerData();
        data.tutorialDone = tutorialDone;
        data.missionDone = missionDone;
        data.eventDone = eventDone;
        data.currentObjectiveDoneEvent = currentObjectiveDoneEvent;
        data.soundOff = soundOff;

        bf.Serialize(file, data);
        file.Close();
        
    }



    //Load game data from CIRA.dat
    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/CIRA.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/CIRA.dat", FileMode.Open);
            PlayerData data = (PlayerData) bf.Deserialize(file);

            tutorialDone = data.tutorialDone;
            missionDone = data.missionDone;
            eventDone = data.eventDone;
            currentObjectiveDoneEvent = data.currentObjectiveDoneEvent;
            soundOff = data.soundOff;

            file.Close();
        }
    }

}



//The serializable class for data serialization
[Serializable]
class PlayerData
{
    public bool tutorialDone;
    public bool missionDone;
    public bool eventDone;
    public int currentObjectiveDoneEvent;
    public bool soundOff;
}