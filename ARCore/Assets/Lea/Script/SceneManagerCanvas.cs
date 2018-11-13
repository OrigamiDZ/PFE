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
        Debug.Log("next scene = " + (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
        Debug.Log("scene count = " + SceneManager.sceneCountInBuildSettings);
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }

    public void PreviousSceneButton()
    {
        Debug.Log("previous scene = " + (SceneManager.GetActiveScene().buildIndex - 1) % SceneManager.sceneCountInBuildSettings);
        Debug.Log("scene count = " + SceneManager.sceneCountInBuildSettings);
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex - 1) % SceneManager.sceneCountInBuildSettings);
    }

}
