using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]  // Save and store information (?); LED TO INDEX ERROR

public class AudioObj 
{
    //Public class for an object storing an audio file and a correct answer

    private string correctAnswer;  // This is the correct option (will be string compared to the option selected)
    private AudioSource audioSource;   // Sound file
    private AudioClip audioClip;
    private bool correctlyAnswered;

    public AudioObj(AudioSource inputSource)
    {
        audioSource = inputSource;
        audioClip = audioSource.clip;
    }

    public void SetCorrectAnswer(string input)
    {
        //no idea how to do this
        correctAnswer = input;
        
    }

    public void AnsweredCorrectly()
    {
        correctlyAnswered = true;
    }

    public void DidNotAnswerCorrectly()
    {
        correctlyAnswered = false;
    }

    public string GetCorrectAnswer()
    {
        return correctAnswer;
    }

    public AudioSource GetAudioSource()
    {
        return audioSource;
    }

    public AudioClip GetAudioClip()
    {
        return audioClip;
    }

    public bool GetCorrectlyAnswered()
    {
        return correctlyAnswered;
    }
}
