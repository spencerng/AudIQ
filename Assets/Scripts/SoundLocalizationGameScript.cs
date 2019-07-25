using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundLocalizationGameScript : MonoBehaviour
{
    public Image chickyTop;
    public Image chickyLeft;
    public Image chickyRight;

    public Transform chickyTopTransform;
    public Transform chickyLeftTransform;
    public Transform chickyRightTransform;

    public Button buttonLeft;
    public Button buttonRight;

    public AudioObj[] audioObjs = new AudioObj[12];

    public bool playLeftAnimation; //I just made booleans so that the Update frame plays animations, perhaps a better way is to play a method for a set # of seconds
    public bool playRightAnimation;

    // Start is called before the first frame update
    void Start()
    {
        chickyTop = GameObject.Find("chicky_T").GetComponent<Image>();
        chickyLeft = GameObject.Find("chicky_L").GetComponent<Image>();
        chickyRight = GameObject.Find("chicky_R").GetComponent<Image>();

        chickyTopTransform = GameObject.Find("chicky_T").GetComponent<Transform>();
        chickyLeftTransform = GameObject.Find("chicky_L").GetComponent<Transform>();
        chickyRightTransform = GameObject.Find("chicky_R").GetComponent<Transform>();

        buttonLeft = GameObject.Find("Button_L").GetComponent<Button>(); //Note buttons are invisible
        buttonRight = GameObject.Find("Button_R").GetComponent<Button>();

        AudioSource[] audioSources = GetComponents<AudioSource>(); //Note this will be 13; first index of 0 is "ready"

        for (int i = 0; i < audioSources.Length; i++)
        {
            audioObjs[i] = new AudioObj(audioSources[i]);
        }
    }

    public IEnumerator StartTrial(int trialNum)
    {
        //Initial pause to allow the Correct/Incorrect flash to finish
        yield return new WaitForSeconds(0.7f);

        RetrieveCorrectAnswers();

        //readyFile.GetAudioSource().PlayOneShot(readyFile.GetAudioClip()); //atm, no ready file
        //yield return new WaitForSeconds(1f);

        audioObjs[trialNum - 1].GetAudioSource().PlayOneShot(audioObjs[trialNum - 1].GetAudioClip()); //plays clip, trial 1 corresponds to index 0
        SetButtonListeners(); //Set/remove new listeners every trial to prevent registering accidental double taps

        yield return new WaitForSeconds(1f);
    }

    /* 1. Play sound
     * 2. Set Listener
     * 3. On Click, register right/wrong; remove listener
     * 4. Make chicky move & then move back
     * 5. Display correct/incorrect.
     * 6. Play next sound, and all over again
     * */

    void RetrieveCorrectAnswers()
    {
        audioObjs[0].SetCorrectAnswer("left");
        audioObjs[1].SetCorrectAnswer("right");
    }

    void SetButtonListeners()
    {
        buttonLeft.onClick.AddListener(() => OnButtonClick("left"));
        buttonRight.onClick.AddListener(() => OnButtonClick("right"));

    }

    void OnButtonClick(string left_or_right)
    {
        RemoveListeners();

        if (left_or_right == "left")
        {
            playLeftAnimation = true;
            //add in correct/incorrect animations

        } else
        {
            playRightAnimation = true;
            //add in correct/incorrect animations

        }
    }

    void RemoveListeners()
    {
        for (int i = 0; i < 2; i++)
        {
            buttonLeft.onClick.RemoveAllListeners();
            buttonRight.onClick.RemoveAllListeners();

        }
    }

    public IEnumerator FlashCorrectScreen(CanvasGroup text_cg, CanvasGroup panel_cg, Image panel, Text text, float timeBetweenFlash = 0.5f, float timeToWait = 0.75f)
    {
        bool changed = false;
        for (int i = 0; i < 2; i++)
        {
            if (!changed)
            {
                text.text = "Correct!";
                text_cg.alpha = 1;

                panel.color = UnityEngine.Color.green;
                panel_cg.alpha = 0.75f;

                changed = true;
            }
            else
            {
                text_cg.alpha = 0;
                panel_cg.alpha = 0;
            }
            yield return new WaitForSeconds(timeBetweenFlash);
        }
        yield return new WaitForSeconds(timeToWait);
    }

    public IEnumerator FlashIncorrectScreen(CanvasGroup text_cg, CanvasGroup panel_cg, Image panel, Text text, float timeBetweenFlash = 0.5f, float timeToWait = 0.75f)
    {
        bool changed = false;
        for (int i = 0; i < 2; i++)
        {
            if (!changed)
            {
                text.text = "Wrong!";
                text_cg.alpha = 1;

                panel.color = UnityEngine.Color.red;
                panel_cg.alpha = 0.75f;

                changed = true;
            }
            else
            {
                text_cg.alpha = 0;
                panel_cg.alpha = 0;
            }
            yield return new WaitForSeconds(timeBetweenFlash);
        }
        yield return new WaitForSeconds(timeToWait);
    }

    // Update is called once per frame
    void Update()
    {
        AndroidBackButtonListener();

        if (playLeftAnimation)
        {
            float temp = Time.time;
            Vector3 originalPosition = chickyLeftTransform.position;

            while (Time.time - temp < 0.5)
            {
                chickyLeftTransform.position = chickyLeftTransform.position + new Vector3(-0.1f, 0, 0);
            }

            chickyLeftTransform.position = originalPosition;

        }

        if (playRightAnimation)
        {
            float temp = Time.time;
            Vector3 originalPosition = chickyLeftTransform.position;

            while (Time.time - temp < 0.5)
            {
                chickyRightTransform.position = chickyRightTransform.position + new Vector3(0.1f, 0, 0);
            }
            chickyLeftTransform.position = originalPosition;

        }

    }

    void AndroidBackButtonListener()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("MainMenu");
            }

        }
    }
}
