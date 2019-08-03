using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIScript : MonoBehaviour
{

    public void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        AudioTest();
    }


    private void AudioTest()
    {
        AudioSource audio = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        AudioPlayer player = new AudioPlayer(audio);
        player.SetOffsetAngle(30.0f);
        player.Play();
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

