using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIScript : MonoBehaviour
{

    public void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        AudioSource audio = GetComponent<AudioSource>();
        AudioPlayer player = new AudioPlayer(audio);
        player.PlayITD(5, false);
    }

    public void StartSpeechInNoiseGame()
    {
        StartCoroutine(SwitchToLandscapeThenStartGame("JeopardyGame"));

    }

    public IEnumerator SwitchToLandscapeThenStartGame(string scene)
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene);
    }

    public void StartSoundLocalizationGame()
    {
        SceneManager.LoadScene("SoundLocalizationGame");
    }

    public void StartPitchDiscriminationGame()
    {

        SceneManager.LoadScene("PitchDiscriminationGame");
    }

}

