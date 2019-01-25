using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class AppController : MonoBehaviour {

    public static AppController control;
    public bool tutorialDone;
    public bool missionDone;
    public bool eventDone;
    public double targetPointGPS;
    public int currentObjectiveDoneEvent;

    void Awake () {
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
    }

    public void OnApplicationQuit()
    {
        Save();
    }



    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
        }
        else
        {
            file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        }

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
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData) bf.Deserialize(file);
            file.Close();

            tutorialDone = data.tutorialDone;
            missionDone = data.missionDone;
            eventDone = data.eventDone;
            currentObjectiveDoneEvent = data.currentObjectiveDoneEvent;
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
}