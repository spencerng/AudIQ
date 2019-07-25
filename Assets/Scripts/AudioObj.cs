﻿using UnityEngine;

//Public class for an object storing an audio file and a correct answer
public class AudioObj
{

    private string correctAnswer;  // This is the correct option (will be string compared to the option selected)
    private readonly AudioSource audioSource;   // Sound file
    private readonly AudioClip audioClip;
    private bool correctlyAnswered;

    public AudioObj(AudioSource audioSource)
    {
        this.audioSource = audioSource;
        audioClip = this.audioSource.clip;
    }

    public void SetCorrectAnswer(string correctAnswer)
    {
        //no idea how to do this
        this.correctAnswer = correctAnswer;

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

    public bool IsCorrectlyAnswered()
    {
        return correctlyAnswered;
    }
}
