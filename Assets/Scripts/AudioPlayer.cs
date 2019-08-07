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
    private float pitch;

    public static readonly float MAX_ILD_DB = 15.0f;
    public static readonly float MAX_ITD_SEC = 0.0007f;
    private static float BASELINE_LINEAR_VOL;

    

    public AudioPlayer(AudioSource audioSource, float offsetAngle = 0.0f, float pitch = 1.0f)
    {
        this.audioSource = audioSource;
        audioClip = audioSource.clip;

        // Set baseline volume so that ILD never goes out of Unity's volume range of [0.0, 1.0]
        BASELINE_LINEAR_VOL = Mathf.Pow(10, -MAX_ILD_DB / 20);


        Reset();
        //SetOffsetAngle(offsetAngle);
       // SetPitch(pitch);
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

        float angleMap = 0.5f - offsetAngle / 90 / 2.22f;
        ild = Mathf.Abs(Mathf.Log(angleMap / (1 - angleMap)) * MAX_ILD_DB / 3);
        itd = Mathf.Abs(Mathf.Log(angleMap / (1 - angleMap)) * MAX_ITD_SEC / 3);

        CreateILD(ild, isTowardsLeft);
        CreateITD(itd, !isTowardsLeft);
    }

    // Plays the audio clip with an interaural level difference specified by ildDb, in decibels
    // The clip is quiet in the right ear if isLeftEarBaseline is true; otherwise, it is quieter in the left ear
    private void CreateILD(float ildDb, bool isLeftEarLouder)
    {
        AudioSource louder = isLeftEarLouder ? leftSource : rightSource;
        AudioSource softer = !isLeftEarLouder ? leftSource : rightSource;

        float gain = ildDb / 2;

        louder.volume = Mathf.Pow(10, gain / 10) * BASELINE_LINEAR_VOL;
        softer.volume = Mathf.Pow(10, -gain / 10) * BASELINE_LINEAR_VOL;

        float ratio = softer.volume / (louder.volume + softer.volume);

        //louder.volume = (1 - ratio) * BASELINE_LINEAR_VOL;
        //softer.volume = ratio * BASELINE_LINEAR_VOL;
    }

    // Plays the audio clip with an interaural time difference specified by itdSec, in seconds. 
    // The clip reaches the right ear first if isLeftEarDelay is true; otherwise, it reaches the left ear first
    public void CreateITD(float itdSec, bool isLeftEarDelay)
    {
        float currentITD = Mathf.Abs(leftSource.time - rightSource.time);

        // Resets sources so ITD = 0 again
        if (leftSource.time < rightSource.time)
        {
            leftSource.time += currentITD / 2;
            rightSource.time -= currentITD / 2;
        }
        else
        {
            leftSource.time -= currentITD / 2;
            rightSource.time += currentITD / 2;
        }

        // Splits ITD between both channels to fix high-frequency popping
        AudioSource ahead = !isLeftEarDelay ? leftSource : rightSource;
        AudioSource behind = isLeftEarDelay ? leftSource : rightSource;

        // Check so that time is not assigned a negative value
        float newBehindTime = Mathf.Max(0.0f, behind.time - itdSec / 2);

        ahead.time += itdSec - (behind.time - newBehindTime);
        behind.time = newBehindTime;
    }

    public void Play()
    {
        leftSource.Play();
        rightSource.Play();
    }

    // Sets the pitch based on Unity's AudioMixer pitch shifter effect
    // 0.5 lowers the clip by one octave, 1.0 is the baseline, while 2.0 increases the clip by one octave
    public void SetPitch(float pitch)
    {
        this.pitch = pitch;
        leftSource.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", pitch);
        rightSource.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", pitch);
    }

    public float GetPitch()
    {
        return pitch;
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
