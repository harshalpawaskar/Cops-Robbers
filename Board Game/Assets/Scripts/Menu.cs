using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void PlayGame()
    {
        Application.LoadLevel(1);
    }

    public void Tutorial()
    {
        Application.LoadLevel(2);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
