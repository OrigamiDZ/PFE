using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerCanvas : MonoBehaviour {

    public void NextSceneButton()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCount);
    }

    public void PreviousSceneButton()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex - 1) % SceneManager.sceneCount);
    }

}
