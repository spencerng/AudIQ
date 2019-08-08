using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TouchGameScript : MonoBehaviour
{
    private readonly int numTouches;
    private AudioPlayer player, samplePlayer;
    private AudioSource audio;
    private float localizationFactor, pitchFactor;
    private bool lockTouch;
    private Transform locationMarkerTransform;
    private SpriteRenderer locationMarkerSR;

    private float startTime;

    private float sampleOffsetAngle, samplePitch;

    // Start is called before the first frame update
    private void Start()
    {
        audio = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        ResetGame();

        Button playSample = GameObject.Find("PlaySample").GetComponent<Button>();
        Button confirm = GameObject.Find("Confirm").GetComponent<Button>();

        playSample.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height * (125f / 1053));
        confirm.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height * (125f / 1053));

        playSample.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * (250f / 592f));
        confirm.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * (250f / 592f));

        playSample.GetComponent<RectTransform>().position = new Vector3(Screen.width * (1.5f / 6), Screen.height * 100f / 1053, 0);
        confirm.GetComponent<RectTransform>().position = new Vector3(Screen.width * 4.5f / 6, Screen.height * 100f / 1053, 0);


        locationMarkerTransform = GameObject.Find("LocationMarker").GetComponent<Transform>();
        locationMarkerSR = GameObject.Find("LocationMarker").GetComponent<SpriteRenderer>();

    }

    private void ResetGame()
    {
        player = new AudioPlayer(audio);
        sampleOffsetAngle = Random.Range(-90f, 90f);
        samplePitch = Random.Range(0.5f, 2.0f);
        PlaySampleAudio();

    }

    public void ScoreSelection()
    {
        float timeDelta = Time.time - startTime;
    }

    public void PlaySampleAudio()
    {
        lockTouch = true;
        StartCoroutine(PlaySampleAudioRoutine());
    }

    public IEnumerator PlaySampleAudioRoutine()
    {
        samplePlayer = new AudioPlayer(audio, sampleOffsetAngle, samplePitch);
        samplePlayer.Play();
        
        yield return new WaitForSeconds(5.0f);
        samplePlayer.Stop();
        yield return new WaitForSeconds(3.0f);
        player.Play();
        lockTouch = false;
        startTime = Time.time;
    }

    // Update is called once per frame
    private void Update()
    {
        if ((Input.touchCount > 0 && !lockTouch) && (Input.GetTouch(Input.touchCount - 1).position.y > Screen.height * 150f / 1053))
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
