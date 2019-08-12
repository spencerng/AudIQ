using System.Collections;
using UnityEngine;
using UnityEngine.UI;
/*TO DO:
 * ENABLE FUNCTIONALITY FOR N (FOR NOW 5) TRIALS; GO TO NEXT TRIAL ON CORRECT OR ON 60 SECOND TIME LIMIT
 * LOG DATA CORRECT, INCORRECT, TIME TAKEN, PITCH GUESSED, ACTUAL PITCH, ETC. (MODIFY AUDIOPLAYER? CREATE NEW OBJ? UPDATE DATABASE?)
 * */
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

    private float score;

    // Start is called before the first frame update
    private void Start()
    {
        audio = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        ResetGame();

        Button playSample = GameObject.Find("PlaySample").GetComponent<Button>();
        Button confirm = GameObject.Find("Confirm").GetComponent<Button>();

        //Set UI
        playSample.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height * (125f / 1000));
        confirm.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height * (125f / 1000));

        playSample.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * (250f / 600));
        confirm.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width * (250f / 600));

        playSample.GetComponent<RectTransform>().position = new Vector3(Screen.width * (1.5f / 6), Screen.height * 100f / 1000, 0);
        confirm.GetComponent<RectTransform>().position = new Vector3(Screen.width * 4.5f / 6, Screen.height * 100f / 1000, 0);


        locationMarkerTransform = GameObject.Find("LocationMarker").GetComponent<Transform>();
        locationMarkerSR = GameObject.Find("LocationMarker").GetComponent<SpriteRenderer>();

    }

    private void ResetGame()
    {
        player = new AudioPlayer(audio);
        sampleOffsetAngle = Random.Range(-90f, 90f);
        samplePitch = Random.Range(0.5f, 2.0f);
        samplePlayer = new AudioPlayer(audio, sampleOffsetAngle, samplePitch); //moved up here
        StartCoroutine(PlaySampleAudioRoutine());
    }

    public void PlaySampleAudio() //Attached to Play Sample Button
    {
        StartCoroutine(PlaySampleAudioRoutine());
    }
    public IEnumerator PlaySampleAudioRoutine()
    {
        lockTouch = true;
        player.Stop();
        //samplePlayer = new AudioPlayer(audio, sampleOffsetAngle, samplePitch);
        samplePlayer.Play();
        yield return new WaitForSeconds(5.0f);
        samplePlayer.Stop();
        yield return new WaitForSeconds(3.0f);
        player.Play();
        lockTouch = false;
        startTime = Time.time;
    }

    public void CheckValidity() //Attached to the Confirm Button
    {
        Debug.Log("Pressed");
        if ((Mathf.Abs(sampleOffsetAngle - localizationFactor) < 30f) && (Mathf.Abs(samplePitch - pitchFactor) < 0.15))
        {
            player.Stop();
            float timeScore;

            if (50 - (Time.time - startTime) < 0)
                timeScore = 0;
            else
                timeScore = 50 - (Time.time - startTime);

            Debug.Log("Correct");
            //Input formula here
            score += (30 - Mathf.Abs(localizationFactor - sampleOffsetAngle)) + (20 * 1 - Mathf.Abs(pitchFactor - samplePitch)) + timeScore;
            GameObject.Find("Score").GetComponent<Text>().text = "Score: " + score;
            StartCoroutine(UIHelper.FlashCorrectIncorrectScreen(true));

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
                locationMarkerSR.sprite = Resources.Load("chicky", typeof(Sprite)) as Sprite;
                locationMarkerTransform.position = latestTouch.position;


                //Question: Should people be able to change both pitch and localization at once, or one at a time? I did one at a time


                //If we want to slowly change through multiple touches:
                localizationFactor = (Screen.width /2 - latestTouch.position.x) * 90 / (Screen.width / 2); //Changes location like a slider
                if (localizationFactor > 90)
                    localizationFactor = 90;
                else if (localizationFactor < -90)
                    localizationFactor = -90;

                if (Mathf.Abs(localizationFactor) > 5)
                    player.SetOffsetAngle(-localizationFactor);



                //Question: How much should moving the finger across the screen alter the touch (ie, how big is the pitch range we're trying to test?)
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

                //1.1 is for attenuation; will probably need algorithm for proper attenuation
                if (pitchFactor < 0.5f) //song starts playing backwards
                    pitchFactor = 0.5f;
                if (pitchFactor > 2f)
                    pitchFactor = 2f;

                if (Mathf.Abs(pitchFactor) > 0.05f)
                    player.SetPitch(pitchFactor);
            }
        }        

        UIHelper.OnBackButtonClickListener("MainMenu");

    }
}
