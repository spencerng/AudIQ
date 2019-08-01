using UnityEngine;

//Public class for an object storing an audio file and a correct answer
public class AudioPlayer
{

    private string correctAnswer;  // This is the correct option (will be string compared to the option selected)
    private readonly AudioSource audioSource;   // Sound file
    private readonly AudioClip audioClip;
    private bool correctlyAnswered;

    public AudioPlayer(AudioSource audioSource)
    {
        this.audioSource = audioSource;
        audioClip = audioSource.clip;
    }

    public void Play()
    {
        audioSource.Play();
    }

    // Plays the audio clip with an interaural time difference specified by delay, in seconds. 
    // The clip reaches the right ear first if isLeftEarDelay is true; otherwise, it reaches the left ear first
    public void PlayITD(float delay, bool isLeftEarDelay)
    {
        AudioSource leftSource = AudioSource.Instantiate(audioSource);
        AudioSource rightSource = AudioSource.Instantiate(audioSource);

        leftSource.panStereo = -1.0f;
        rightSource.panStereo = 1.0f;

        leftSource.PlayDelayed(isLeftEarDelay ? delay : 0);
        rightSource.PlayDelayed(!isLeftEarDelay ? delay : 0);
    }

    // Plays the audio clip with an interaural level difference specified by dbReduce, in decibels
    // The clip is quiet in the right ear if isLeftEarBaseline is true; otherwise, it is quieter in the left ear
    public void PlayILD(float dbReduce, bool isLeftEarBaseline)
    {
        AudioSource leftSource = AudioSource.Instantiate(audioSource);
        AudioSource rightSource = AudioSource.Instantiate(audioSource);

        leftSource.panStereo = -1.0f;
        rightSource.panStereo = 1.0f;

        if (isLeftEarBaseline)
        {
            ReduceVolume(dbReduce, leftSource);
        }
        else
        {
            ReduceVolume(dbReduce, rightSource);
        }

        leftSource.Play();
        rightSource.Play();
    }

    public void PlaySoundLocalization(float offsetInDegrees)
    {

    }

    private void ReduceVolume(float db, AudioSource audioSource)
    {
        audioSource.volume = Mathf.Pow(10, -db / 10);
    }

    public void SetCorrectAnswer(string correctAnswer)
    {
        this.correctAnswer = correctAnswer;
    }

    public string GetCorrectAnswer()
    {
        return correctAnswer;
    }

    public void SetCorrectlyAnswered(bool isCorrectlyAnswered)
    {
        correctlyAnswered = isCorrectlyAnswered;
    }

    public bool IsCorrectlyAnswered()
    {
        return correctlyAnswered;
    }

    public AudioSource GetAudioSource()
    {
        return audioSource;
    }

    public AudioClip GetAudioClip()
    {
        return audioClip;
    }


}
