using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        Load();
        //soundOff = true;
        if (soundOff) { AudioListener.volume = 0f; } else { AudioListener.volume = 1f; }
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
        if (soundOff) { AudioListener.volume = 0f; } else { AudioListener.volume = 1f; }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Save();
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }



    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        file = File.Create(Application.persistentDataPath + "/CIRA-demo.dat");

        PlayerData data = new PlayerData();
        data.tutorialDone = tutorialDone;
        data.missionDone = missionDone;
        data.eventDone = eventDone;
        data.currentObjectiveDoneEvent = currentObjectiveDoneEvent;
        data.soundOff = soundOff;

        bf.Serialize(file, data);
        file.Close();
        
    }



    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/CIRA-demo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/CIRA-demo.dat", FileMode.Open);
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




[Serializable]
class PlayerData
{
    public bool tutorialDone;
    public bool missionDone;
    public bool eventDone;
    public int currentObjectiveDoneEvent;
    public bool soundOff;
}