﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("Main menu script working!");
        Button speechInNoiseButton = GameObject.Find("SpeechInNoiseButton").GetComponent<Button>();
        
        speechInNoiseButton.onClick.AddListener(() => SwitchScene(1));

    }

    void SwitchScene(int gameNum)
    {
        Debug.Log("Button clicked");
        if (gameNum == 1)
        {
            SceneManager.LoadScene("SpeechInNoiseGame", LoadSceneMode.Single);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}