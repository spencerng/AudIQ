﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIScript : MonoBehaviour
{

    AudioPlayer player;

    public void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        //AudioTest();

        AudioSource audio = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        player = new AudioPlayer(audio);
        player.playAlteredPitch(1.5f);
    }


    private void AudioTest()
    {
        AudioSource audio = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        player = new AudioPlayer(audio);
        player.SetOffsetAngle(90.0f);
        player.Play();
    }

    public void Update()
    {
        Slider slider = GameObject.Find("Slider").GetComponent<Slider>();
        player.SetOffsetAngle(-slider.value);

        Slider sliderPitch = GameObject.Find("Slider (1)").GetComponent<Slider>();
        player.SetAlteredPitch(sliderPitch.value);

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

