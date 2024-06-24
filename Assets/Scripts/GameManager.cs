using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int FPS = 60;
    private bool MenuLoaded = false;
    public Button confirmButton;
    public TMP_Text input;

    void Start()
    {
        DontDestroyOnLoad(this);
        Application.targetFrameRate = FPS;
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Menu" && !MenuLoaded)
        {
            confirmButton.onClick.AddListener(OnConfirmButtonClick);
            MenuLoaded = true;
        }
        else if (currentScene.name == "Menu")
        {
            confirmButton.onClick.AddListener(OnConfirmButtonClick);
        }
    }

    public void OnConfirmButtonClick()
    {
        string namePlayer = input.text;
        if (string.IsNullOrEmpty(namePlayer))
        {
            return;
        }
        else
        {
            SceneManager.LoadScene("MainScene");
            Debug.Log("Estamos en el juego");
        }
    }
}

