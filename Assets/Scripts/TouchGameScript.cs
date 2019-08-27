using System.Collections;
using UnityEngine;
using UnityEngine.UI;
/*TO DO:
 * LOG DATA CORRECT, INCORRECT, TIME TAKEN, PITCH GUESSED, ACTUAL PITCH, ETC. (MODIFY AUDIOPLAYER? CREATE NEW OBJ? UPDATE DATABASE?)
 * */
public class TouchGameScript : MonoBehaviour
{
    private readonly int numTouches;
    private AudioPlayer samplePlayer;
    private TargetBeaconPlayer player;
    private AudioSource audio;
    private float localizationFactor, pitchFactor;
    private bool lockTouch, istimeLimitReached, isFirstSamplePlaying;
    private Transform locationMarkerTransform;
    private SpriteRenderer locationMarkerSR;

    private float startTime, sampleOffsetAngle, samplePitch, score, maxTime;

    private int numTrials, maxTrials;

    // Start is called before the first frame update
    private void Start()
    {
        numTrials = 0; maxTrials = 3; maxTime = 60f;
        audio = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        //player = new AudioPlayer(audio);
        player = GameObject.Find("TargetBeaconManager").GetComponent<TargetBeaconPlayer>();

        samplePlayer = new AudioPlayer(audio, sampleOffsetAngle, samplePitch); //moved up here

        Button playSample = GameObject.Find("PlaySample").GetComponent<Button>();
        Button confirm = GameObject.Find("Confirm").GetComponent<Button>();
        Text scoreText = GameObject.Find("Score").GetComponent<Text>();
        Text timeRemaining = GameObject.Find("TimeRemaining").GetComponent<Text>();

        //Set UI
        playSample.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height * (125f / 1000));
        confirm.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height * (125f / 1000));
        scoreText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height * (125f / 1000));
        timeRemaining.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height * (125f / 1000));

        playSample.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * (250f / 600));
        confirm.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * (250f / 600));
        scoreText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * (250f / 600));
        timeRemaining.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * (250f / 600));

        playSample.GetComponent<RectTransform>().position = new Vector3(Screen.width * (1.5f / 6), Screen.height * 100f / 1000, 0);
        confirm.GetComponent<RectTransform>().position = new Vector3(Screen.width * 4.5f / 6, Screen.height * 100f / 1000, 0);
        scoreText.GetComponent<RectTransform>().position = new Vector3(Screen.width * (1.5f / 6), Screen.height * 900f / 1000, 0);
        timeRemaining.GetComponent<RectTransform>().position = new Vector3(Screen.width * (4.5f / 6), Screen.height * 900f / 1000, 0);

        locationMarkerTransform = GameObject.Find("LocationMarker").GetComponent<Transform>();
        locationMarkerSR = GameObject.Find("LocationMarker").GetComponent<SpriteRenderer>();

        locationMarkerSR.sprite = Resources.Load("chicky", typeof(Sprite)) as Sprite;
        
        RectTransform locationMarkerRectTransform = locationMarkerSR.GetComponent<RectTransform>();

        locationMarkerRectTransform.anchorMin = new Vector2(0, 0);
        locationMarkerRectTransform.anchorMax = new Vector2(0, 0);
        locationMarkerRectTransform.pivot = new Vector2(0, 0);
        
        StartNewTrial();
        
    }

    private void StartNewTrial()
    {
        if (numTrials < maxTrials)
        {
            locationMarkerTransform.position = new Vector3(291, 520);
            numTrials++;
            player.Reset();
           
            startTime = Time.time; //Time it takes for original sample to play
            istimeLimitReached = false;
            //StartCoroutine(PlaySampleAudioRoutine());
        }
    }

    public void PlaySampleAudio() //Attached to Play Sample Button
    {
        if (maxTime - (Time.time - startTime) > 8f && !isFirstSamplePlaying)
            StartCoroutine(PlaySampleAudioRoutine());
    }
    public IEnumerator PlaySampleAudioRoutine()
    {
        lockTouch = true;
        isFirstSamplePlaying = true;
        player.Stop();
        samplePlayer.Play();
        yield return new WaitForSeconds(5.0f);
        samplePlayer.Stop();
        yield return new WaitForSeconds(1.0f);
        player.Play();
        lockTouch = false;
        isFirstSamplePlaying = false;
    }

    public void CheckValidity() //Attached to the Confirm Button
    {
        if (player.GetAbsoluteOffsetAngleDifference() < 30f && player.GetAbsolutePitchDifference() < 0.15)
        {
            player.Stop();
            float timeScore;

            if (50 - (Time.time - startTime) < 0)
            {
                timeScore = 0;
            }
            else
            {
                timeScore = 50 - (Time.time - startTime);
            }

            //Input formula here
            score += (30 - player.GetAbsoluteOffsetAngleDifference()) + (20 * 1 - player.GetAbsolutePitchDifference()) + timeScore;
            GameObject.Find("Score").GetComponent<Text>().text = "Score: " + (int)score;
            StartCoroutine(UIHelper.FlashCorrectIncorrectScreen(true));

            StartNewTrial();
        }
        else
        {
            StartCoroutine(UIHelper.FlashCorrectIncorrectScreen(false));
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if ((Input.touchCount > 0 && !lockTouch) && (Input.GetTouch(Input.touchCount - 1).position.y > Screen.height * 150f / 1000) && (Input.GetTouch(Input.touchCount - 1).position.y < Screen.height * 850f / 1000))
        {
            Touch latestTouch = Input.GetTouch(Input.touchCount - 1);
            if (latestTouch.phase == TouchPhase.Moved)
            {
                Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
                locationMarkerTransform.position = camera.ScreenToWorldPoint(new Vector3(latestTouch.position.x, latestTouch.position.y, camera.nearClipPlane));

                localizationFactor = (Screen.width /2 - latestTouch.position.x) * 90 / (Screen.width / 2); //Changes location like a slider
                if (localizationFactor > 90)
                    localizationFactor = 90;
                else if (localizationFactor < -90)
                    localizationFactor = -90;

                if (Mathf.Abs(localizationFactor) > 5)
                    player.SetBeaconOffsetAngle(localizationFactor);

                float registeredScreenHeight = Screen.height * 700f / 1000;
                float modifiedYPosition = latestTouch.position.y - (Screen.height * 150f / 1000);

                if (modifiedYPosition < registeredScreenHeight / 2)
                {
                    pitchFactor = 0.5f + 0.5f * (modifiedYPosition / (registeredScreenHeight / 2f));
                }
                else
                {
                    pitchFactor = 1f + ((modifiedYPosition - registeredScreenHeight / 2f) / (registeredScreenHeight / 2f));
                }

                if (pitchFactor < 0.5f) //song starts playing backwards
                    pitchFactor = 0.5f;
                if (pitchFactor > 2f)
                    pitchFactor = 2f;

                if (Mathf.Abs(pitchFactor) > 0.05f)
                    player.SetBeaconPitch(pitchFactor);
            }
        }

        Debug.Log(locationMarkerTransform.position);

        //Update Time
        GameObject.Find("TimeRemaining").GetComponent<Text>().text = "Time Remaining: " + (int)(maxTime - (Time.time - startTime) + 1);

        //Check Time Limit
        if (Time.time - startTime > maxTime && !istimeLimitReached)
        {
            StartCoroutine(UIHelper.FlashCorrectIncorrectScreen(false));
            istimeLimitReached = true;
            StartNewTrial();
        }

        UIHelper.OnBackButtonClickListener("MainMenu");

    }
}
