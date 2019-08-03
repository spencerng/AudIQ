using UnityEngine;

//Public class for an object storing an audio file and a correct answer
public class AudioPlayer
{

    private string correctAnswer;  // This is the correct option (will be string compared to the option selected)
    private readonly AudioSource audioSource;   // Sound file
    private readonly AudioClip audioClip;
    private bool correctlyAnswered;

    public readonly static float MAX_ILD_DB = 0.5f;
    public readonly static float MAX_ITD_SEC = 0.00066f;

    public AudioPlayer(AudioSource audioSource)
    {
        this.audioSource = audioSource;
        audioClip = audioSource.clip;

        // Set baseline volume so that ILD never goes out of Unity's volume range of [0.0, 1.0]
        this.audioSource.volume = Mathf.Pow(10, -MAX_ILD_DB / 20);
    }

    public void Play()
    {
        audioSource.Play();
    }

    // A positive offset means the source is to the left relative to the midline
    // A negative offset means the source is to the right
    // Range of offset is [-90, 90]
    public void PlaySoundLocalization(float offsetInDegrees)
    {
        AudioSource leftSource = GetLeftSource();
        AudioSource rightSource = GetRightSource();

        bool isTowardsLeft = offsetInDegrees > 0.0;

        float x = 0.5f - offsetInDegrees / 90 / 2.22f;
        float ild = Mathf.Abs(Mathf.Log(x/ (1 - x)) * MAX_ILD_DB / 3);
        float itd = Mathf.Abs(Mathf.Log(x / (1 - x)) * MAX_ITD_SEC / 3);
        
        CreateILD(ild, isTowardsLeft, leftSource, rightSource);
        PlayITD(itd, !isTowardsLeft, leftSource, rightSource);
    }


    // Plays the audio clip with an interaural time difference specified by delay, in seconds. 
    // The clip reaches the right ear first if isLeftEarDelay is true; otherwise, it reaches the left ear first
    public void PlayITD(float delay, bool isLeftEarDelay)
    {
        PlayITD(delay, isLeftEarDelay, GetLeftSource(), GetRightSource());
    }

    private void PlayITD(float delay, bool isLeftEarDelay, AudioSource leftSource, AudioSource rightSource)
    {
        leftSource.PlayDelayed(isLeftEarDelay ? delay : 0);
        rightSource.PlayDelayed(!isLeftEarDelay ? delay : 0);
    }

    // Plays the audio clip with an interaural level difference specified by dbReduce, in decibels
    // The clip is quiet in the right ear if isLeftEarBaseline is true; otherwise, it is quieter in the left ear
    public void PlayILD(float dbDifference, bool isLeftEarLouder)
    {
        AudioSource leftSource = GetLeftSource();
        AudioSource rightSource = GetRightSource();

        CreateILD(dbDifference, isLeftEarLouder, leftSource, rightSource);

        leftSource.Play();
        rightSource.Play();
    }

    private void CreateILD(float dbDifference, bool isLeftEarLouder, AudioSource leftSource, AudioSource rightSource)
    {
        AudioSource louder = isLeftEarLouder ? leftSource : rightSource;
        AudioSource softer = !isLeftEarLouder ? leftSource : rightSource;

        float gain = dbDifference / 2;

        louder.volume *= Mathf.Pow(10, gain / 10);
        softer.volume *= Mathf.Pow(10, -gain / 10);
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

    private AudioSource GetLeftSource()
    {
        AudioSource leftSource = AudioSource.Instantiate(audioSource);
        leftSource.panStereo = -1.0f;

        return leftSource;
    }

    private AudioSource GetRightSource()
    {
        AudioSource rightSource = AudioSource.Instantiate(audioSource);
        rightSource.panStereo = 1.0f;

        return rightSource;
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
