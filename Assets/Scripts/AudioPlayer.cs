using UnityEngine;

//Public class for an object storing an audio file and a correct answer
public class AudioPlayer 
{

    private string correctAnswer;
    private bool correctlyAnswered;

    private readonly AudioSource audioSource;
    private AudioSource leftSource, rightSource;
    private readonly AudioClip audioClip;

    private float ild, itd;
    private float offsetAngle;

    public readonly static float MAX_ILD_DB = 15.0f;
    public readonly static float MAX_ITD_SEC = 0.00066f;
    private static float BASELINE_LINEAR_VOL;

    public AudioPlayer(AudioSource audioSource)
    {
        this.audioSource = audioSource;
        audioClip = audioSource.clip;

        // Set baseline volume so that ILD never goes out of Unity's volume range of [0.0, 1.0]
        BASELINE_LINEAR_VOL = Mathf.Pow(10, -MAX_ILD_DB / 20); 

        Reset();
    }

    public void Reset()
    {
        audioSource.volume = BASELINE_LINEAR_VOL;

        leftSource = GetLeftSource();
        rightSource = GetRightSource();
    }

    private void UpdateInterauralDifferences()
    {

        bool isTowardsLeft = offsetAngle > 0.0;

        float x = 0.5f - offsetAngle / 90 / 2.22f;
        float ild = Mathf.Abs(Mathf.Log(x / (1 - x)) * MAX_ILD_DB / 3);
        float itd = Mathf.Abs(Mathf.Log(x / (1 - x)) * MAX_ITD_SEC / 3);

        CreateILD(ild, isTowardsLeft);
        CreateITD(itd, !isTowardsLeft);
    }

    // Plays the audio clip with an interaural level difference specified by dbReduce, in decibels
    // The clip is quiet in the right ear if isLeftEarBaseline is true; otherwise, it is quieter in the left ear
    private void CreateILD(float ildDb, bool isLeftEarLouder)
    {
        AudioSource louder = isLeftEarLouder ? leftSource : rightSource;
        AudioSource softer = !isLeftEarLouder ? leftSource : rightSource;

        float gain = ildDb / 2;

        louder.volume = Mathf.Pow(10, gain / 10) * BASELINE_LINEAR_VOL;
        softer.volume = Mathf.Pow(10, -gain / 10) * BASELINE_LINEAR_VOL;
    }

    // Plays the audio clip with an interaural time difference specified by delay, in seconds. 
    // The clip reaches the right ear first if isLeftEarDelay is true; otherwise, it reaches the left ear first
    public void CreateITD(float itdSec, bool isLeftEarDelay)
    {
        if (leftSource.time < rightSource.time)
        {
            leftSource.time = rightSource.time;
        }
        else
        {
            rightSource.time = leftSource.time;
        }

        AudioSource ahead = !isLeftEarDelay ? leftSource : rightSource;

        ahead.time += itdSec;
    }

    public void Play()
    {
        leftSource.Play();
        rightSource.Play();
    }

    // A positive offset means the source is to the left relative to the midline
    // A negative offset means the source is to the right
    // Range of offset is [-90, 90]
    public void SetOffsetAngle(float offsetAngle)
    {
        this.offsetAngle = offsetAngle;
        UpdateInterauralDifferences();
    }

    public float GetOffsetAngle()
    {
        return offsetAngle;
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

}
