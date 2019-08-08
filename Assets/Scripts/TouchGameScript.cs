using System.Collections;
using UnityEngine;

public class TouchGameScript : MonoBehaviour
{
    private readonly int numTouches;
    private AudioPlayer player;
    private float localizationFactor, pitchFactor;
    private bool lockTouch;
    private Transform locationMarkerTransform;
    private SpriteRenderer locationMarkerSR;

    // Start is called before the first frame update
    private void Start()
    {


        PlaySampleAudio();

        locationMarkerTransform = GameObject.Find("LocationMarker").GetComponent<Transform>();
        locationMarkerSR = GameObject.Find("LocationMarker").GetComponent<SpriteRenderer>();

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
    private void Update()
    {

        if (Input.touchCount > 0 && !lockTouch)
        {
            Touch latestTouch = Input.GetTouch(Input.touchCount - 1);
            if (latestTouch.phase == TouchPhase.Moved)
            {
                locationMarkerSR.sprite = Resources.Load("chicky", typeof(Sprite)) as Sprite;
                locationMarkerTransform.position = latestTouch.position;


                //Question: Should people be able to change both pitch and localization at once, or one at a time? I did one at a time


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



                //Question: How much should moving the finger across the screen alter the touch (ie, how big is the pitch range we're trying to test?)

                pitchFactor = pitchFactor + latestTouch.deltaPosition.y / Screen.height / 1.4f;
                //1.1 is for attenuation; will probably need algorithm for proper attenuation
                if (pitchFactor < 0.5f) //song starts playing backwards
                    pitchFactor = 0.5f;
                if (pitchFactor > 2f)
                    pitchFactor = 2f;

                if (Mathf.Abs(pitchFactor) > 0.05f)
                    player.SetPitch(pitchFactor);


                Debug.Log(latestTouch.position);

            }
        }

        UIHelper.OnBackButtonClickListener("MainMenu");

    }
}
