using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public InputField inputField1;
    public InputField inputField2;
    public Text text;

    private void Start()
    {
        inputField1.text = PlayerPrefs.GetString("Player1Name");
        inputField2.text = PlayerPrefs.GetString("Player2Name");
        if (inputField1.text == "" && inputField2.text == "") 
        {
            inputField1.text = "Player1";
            inputField2.text = "Player2";
            PlayerPrefs.SetString("Player1Name", inputField1.text);
            PlayerPrefs.SetString("Player2Name", inputField2.text);
        }
    }
    private void Update()
    {
    }

    public void makeChanges()
    {
        if (inputField1.text == "" || inputField2.text == "")
        {
            text.text = "Don't leave the name blank.\nNo Changes Made.";
            text.color = Color.red;
            StartCoroutine(Coroutine1());
        }
        else
        {
            PlayerPrefs.SetString("Player1Name", inputField1.text);
            PlayerPrefs.SetString("Player2Name", inputField2.text);
            text.text = "Changes Made Successfully.";
            text.color = Color.green;
            StartCoroutine(Coroutine1());
        }
    }

    IEnumerator Coroutine1()
    {
        yield return new WaitForSeconds(1f);
        text.text = "";
    }

    public void setTextValue()
    {
        inputField1.text = PlayerPrefs.GetString("Player1Name");
        inputField2.text = PlayerPrefs.GetString("Player2Name");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Tutorial()
    {
        SceneManager.LoadScene(2);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
