using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIScript : MonoBehaviour
{
    public void StartSpeechInNoiseGame()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        SceneManager.LoadScene("JeopardyGame");

    }

    public void StartSoundLocalizationGame()
    {
        SceneManager.LoadScene("SoundLocalizationGame");
    }

    public void StartPitchDiscriminationGame()
    {

        SceneManager.LoadScene("PitchDiscriminationGame");
    }

    public void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }
}

