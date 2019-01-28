using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class AppController : MonoBehaviour {

    public static AppController control;
    public bool tutorialDone;
    public bool missionDone;
    public bool eventDone;
    public double targetPointGPS;
    public int currentObjectiveDoneEvent;
    public bool inSpeechRecoMode;

    void Awake () {
        //1st time running app
        if(control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
            RequestAndroidPermissions();
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
	}

    public void Start()
    {
        Load();
        if (!tutorialDone)
        {
            SceneManager.LoadScene("Tutorial");
        }
        else
        {
            SceneManager.LoadScene("Discovery");
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Save();
        }
    }



    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        file = File.Create(Application.persistentDataPath + "/CIRAInfo.dat");

        PlayerData data = new PlayerData();
        data.tutorialDone = tutorialDone;
        data.missionDone = missionDone;
        data.eventDone = eventDone;
        data.currentObjectiveDoneEvent = currentObjectiveDoneEvent;

        bf.Serialize(file, data);
        file.Close();
    }



    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/CIRAInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/CIRAInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData) bf.Deserialize(file);
            file.Close();

            tutorialDone = data.tutorialDone;
            missionDone = data.missionDone;
            eventDone = data.eventDone;
            currentObjectiveDoneEvent = data.currentObjectiveDoneEvent;
        }
    }



    private void RequestAndroidPermissions()
    {

    }

}




[Serializable]
class PlayerData
{
    public bool tutorialDone;
    public bool missionDone;
    public bool eventDone;
    public int currentObjectiveDoneEvent;
}