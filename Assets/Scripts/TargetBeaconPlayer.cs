using UnityEngine;
using System.Collections;


public class TargetBeaconPlayer : MonoBehaviour
{

    AudioPlayer target, beacon;
    float clipLength;
    bool currentlyPlaying, targetPlaying;

    public AudioSource audioSource;
    public float timeDelay;

    public void Start()
    {
        target = new AudioPlayer(audioSource, Random.Range(-90.0f, 90.0f), Random.Range(0.5f, 2.0f));
        beacon = new AudioPlayer(audioSource);
        clipLength = audioSource.clip.length;
    }

    public void Reset()
    {
        beacon.Stop();
        target.Stop();
        
        target.SetPitch(Random.Range(0.5f, 2.0f));
        target.SetOffsetAngle(Random.Range(-90.0f, 90.0f));
         
        Play();
    }

    public void Play()
    {
        currentlyPlaying = true;
        StartCoroutine(PlayCycle());
    }

    IEnumerator PlayCycle()
    {
        while (currentlyPlaying)
        {
            targetPlaying = true;
            target.Play();
            yield return new WaitForSeconds(clipLength);
            targetPlaying = false;
            target.Stop();
            yield return new WaitForSeconds(timeDelay);

            
            beacon.Play();
            yield return new WaitForSeconds(clipLength);
            beacon.Stop();
            yield return new WaitForSeconds(timeDelay);
        }
        
    }

    public void Stop()
    {
        currentlyPlaying = false;
        StopAllCoroutines();
    }

    public void SetBeaconOffsetAngle(float offsetAngle)
    {
        beacon.SetOffsetAngle(offsetAngle);
    }

    public void SetBeaconPitch(float pitch)
    {
        if (!targetPlaying)
        { 
            beacon.SetPitch(pitch);
        }
    }

    public float GetAbsolutePitchDifference()
    {
        return Mathf.Abs(target.GetPitch() - beacon.GetPitch());
    }

    public float GetAbsoluteOffsetAngleDifference()
    {
        return Mathf.Abs(target.GetOffsetAngle() - beacon.GetOffsetAngle());
    }

}

