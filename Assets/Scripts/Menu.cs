using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Newgame()
    {
        SceneManager.LoadScene(1);
    }

    public void BackMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Exitgame()
    {
        Application.Quit();
    }
}
