using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchGameScript : MonoBehaviour
{
    private int numTouches;
    private AudioPlayer player;
    private float localizationFactor, pitchFactor;
    private bool lockTouch;

    // Start is called before the first frame update
    void Start()
    {
        
        PlaySampleAudio();

        localizationFactor = 0f;
        pitchFactor = 1f;
    }

    public void ScoreSelection()
    {

    }

    public void PlaySampleAudio()
    {
        lockTouch = true;
        StartCoroutine(PlaySampleAudioRoutine());
    }

    public IEnumerator PlaySampleAudioRoutine()
    {
        AudioSource audio = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        player = new AudioPlayer(audio);
        player.Play();
        yield return new WaitForSeconds(5.0f);
        player.Stop();
        yield return new WaitForSeconds(3.0f);
        player.Play();
        lockTouch = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0 && !lockTouch)
        {
            Touch latestTouch = Input.GetTouch(Input.touchCount - 1);
            if (latestTouch.phase == TouchPhase.Moved)
            {
                //Question: Should people be able to change both pitch and localization at once, or one at a time? I did one at a time

                if (Mathf.Abs(latestTouch.deltaPosition.x) > Mathf.Abs(latestTouch.deltaPosition.y))
                {

                    //If we want to slowly change through multiple touches:
                    localizationFactor = localizationFactor + latestTouch.deltaPosition.x / Screen.width * 90; //Changes location like a slider
                    if (localizationFactor > 90)
                        localizationFactor = 90;
                    else if (localizationFactor < -90)
                        localizationFactor = -90;

                    //Question: Do we care about the angle, or is a binary 90/-90 for the angle OK?
                    /*
                    float localizationFactor = latestTouch.deltaPosition.x;
                    if (localizationFactor > 0)
                        localizationFactor = 90;
                    else if (localizationFactor == 0)
                        localizationFactor = 0;
                    else if (localizationFactor < 0)
                        localizationFactor = -90;
                    */

                    if (Mathf.Abs(localizationFactor) > 5)
                        player.SetOffsetAngle(-localizationFactor);


                }
                else
                {
                    //Question: How much should moving the finger across the screen alter the touch (ie, how big is the pitch range we're trying to test?)

                    pitchFactor = pitchFactor + latestTouch.deltaPosition.y / Screen.height / 1.4f; 
                    //1.1 is for attenuation; will probably need algorithm for proper attenuation
                    if (pitchFactor < 0) //song starts playing backwards
                        pitchFactor = 0.05f;

                    if (Mathf.Abs(pitchFactor) > 0.05f)
                        player.SetPitch(pitchFactor);
                }

                Debug.Log(pitchFactor);

            }
        }

        UIHelper.OnBackButtonClickListener("MainMenu");

    }
}
