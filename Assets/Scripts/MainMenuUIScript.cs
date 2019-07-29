using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIScript : MonoBehaviour
{
    public void StartSpeechInNoiseGame()
    {
        StartCoroutine(SwitchToLandscape());

    }

    public IEnumerator SwitchToLandscape()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        yield return new WaitForSeconds(1f);
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

