using System.Collections;
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
        Button soundLocalizationButton = GameObject.Find("SoundLocalizationButton").GetComponent<Button>();
        Button pitchDiscriminationButton = GameObject.Find("PitchDiscriminationButton").GetComponent<Button>();
        
        speechInNoiseButton.onClick.AddListener(() => SwitchScene(1));
        soundLocalizationButton.onClick.AddListener(() => SwitchScene(2));
        pitchDiscriminationButton.onClick.AddListener(() => SwitchScene(3));
    }

    void SwitchScene(int gameNum)
    {
        Debug.Log("Button clicked");
        if (gameNum == 1)
        {
            SceneManager.LoadScene("SpeechInNoiseGame", LoadSceneMode.Single);
        } else if (gameNum == 2)
        {

            SceneManager.LoadScene("SoundLocalizationGame", LoadSceneMode.Single);
        } else if (gameNum == 3)
        {

            SceneManager.LoadScene("PitchDiscriminationGame", LoadSceneMode.Single);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
