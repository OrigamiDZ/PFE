using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerCanvas : MonoBehaviour {

    public Text sceneNameText;

    private void Start()
    {
        Debug.Log("scene count = " + SceneManager.sceneCountInBuildSettings);
        Debug.Log("previous scene = " + (SceneManager.GetActiveScene().buildIndex - 1) % SceneManager.sceneCountInBuildSettings);
        Debug.Log("next scene = " + (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                sceneNameText.text = "Hello AR";
                Debug.Log("Scene = " + SceneManager.GetActiveScene().buildIndex);
                break;

            case 1:
                sceneNameText.text = "Object in Front Camera";
                Debug.Log("Scene = " + SceneManager.GetActiveScene().buildIndex);
                break;

            case 2:
                sceneNameText.text = "Image recognition";
                Debug.Log("Scene = " + SceneManager.GetActiveScene().buildIndex);
                break;
        }
    }

    public void NextSceneButton()
    {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 2)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            Debug.Log("next scene = " + (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
            Debug.Log("scene count = " + SceneManager.sceneCountInBuildSettings);
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
        }
    }

    public void PreviousSceneButton()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 2);
        }
        else
        {
            Debug.Log("previous scene = " + (SceneManager.GetActiveScene().buildIndex - 1) % SceneManager.sceneCountInBuildSettings);
            Debug.Log("scene count = " + SceneManager.sceneCountInBuildSettings);
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex - 1) % SceneManager.sceneCountInBuildSettings);
        }
    }


    public void toMainApp()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
    }

    public void toTests()
    {
        SceneManager.LoadScene(0);
    }
}
