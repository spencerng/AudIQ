using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpeechInNoiseGameScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button[] wordButtons = new Button[12];
        for(int i = 0; i < 12; i++)
        {
            wordButtons[i] = GameObject.Find("SpeechGameButton" + (i+1)).GetComponent<Button>();

            int buttonNum = i;
            wordButtons[i].onClick.AddListener(() => OnSpeechButtonClick(buttonNum));
        }
            
    }

    void OnSpeechButtonClick(int buttonNum)
    {
        Debug.Log(buttonNum + " clicked!");
    }

    // Update is called once per frame
    void Update()
    {

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("MainMenu");
            }

        }
    }
}
