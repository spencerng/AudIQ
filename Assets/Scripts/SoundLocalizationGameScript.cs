using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SoundLocalizationGameScript : MonoBehaviour
{

    private Transform chickyTopTransform;
    private Transform chickyLeftTransform;
    private Transform chickyRightTransform;

    private Button buttonLeft;
    private Button buttonRight;

    //for now we have two trials, change later
    private AudioObj[] audioObjs;

    //I just made booleans so that the Update frame plays animations, perhaps a better way is to play a method for a set # of seconds
    private bool playLeftAnimation;
    private bool playRightAnimation;
    private bool playTopAnimation;

    private Vector3 originalChickyTopPosition;
    private Vector3 originalChickyLeftPosition;
    private Vector3 originalChickyRightPosition;


    private int numTrial;

    // Start is called before the first frame update
    private void Start()
    {
        audioObjs = new AudioObj[2];
        numTrial = 1;

        chickyTopTransform = GameObject.Find("chicky_T").GetComponent<Transform>();
        chickyLeftTransform = GameObject.Find("chicky_L").GetComponent<Transform>();
        chickyRightTransform = GameObject.Find("chicky_R").GetComponent<Transform>();

        buttonLeft = GameObject.Find("Button_L").GetComponent<Button>(); //Note buttons are invisible
        buttonRight = GameObject.Find("Button_R").GetComponent<Button>();

        originalChickyLeftPosition = chickyLeftTransform.position;
        originalChickyRightPosition = chickyRightTransform.position;
        originalChickyTopPosition = chickyTopTransform.position;

        AudioSource[] audioSources = GetComponents<AudioSource>();

        for (int i = 0; i < audioSources.Length; i++)
        {
            audioObjs[i] = new AudioObj(audioSources[i]);
        }

       

        playTopAnimation = true;
    
        StartCoroutine(StartTrial(numTrial));
    }

    private IEnumerator StartTrial(int trialNum)
    {
        //Initial pause to allow the Correct/Incorrect flash to finish
        yield return new WaitForSeconds(0.7f);

        RetrieveCorrectAnswers();

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

    private void RetrieveCorrectAnswers()
    {
        audioObjs[0].SetCorrectAnswer("left");
        audioObjs[1].SetCorrectAnswer("right");
    }

    private void SetButtonListeners()
    {
        buttonLeft.onClick.AddListener(() => OnButtonClick(true));
        buttonRight.onClick.AddListener(() => OnButtonClick(false));

    }

    private void OnButtonClick(bool isLeftButton)
    {
        RemoveListeners();

        int indexForTrial = numTrial - 1;

        if (numTrial <= audioObjs.Length)
        {

            playLeftAnimation = isLeftButton;

            bool correctlyAnswered = audioObjs[indexForTrial].GetCorrectAnswer() == (isLeftButton ? "left" : "right");

            audioObjs[indexForTrial].SetCorrectlyAnswered(correctlyAnswered);
            StartCoroutine(UIHelper.FlashCorrectIncorrectScreen(correctlyAnswered));

        }
        numTrial++;


        if (numTrial <= audioObjs.Length)
        {
            StartCoroutine(StartTrial(numTrial));
        }
    }

    private void RemoveListeners()
    {
        for (int i = 0; i < 2; i++)
        {
            buttonLeft.onClick.RemoveAllListeners();
            buttonRight.onClick.RemoveAllListeners();

        }
    }

    private void Update()
    {
        UIHelper.OnBackButtonClickListener("MainMenu");

        //Top animation
        if (playTopAnimation && UIHelper.Translate(chickyTopTransform, new Vector3(0, -1f), originalChickyTopPosition + new Vector3(0, -150f)))
        {
            playTopAnimation = false;
            chickyTopTransform.position = originalChickyTopPosition;
        }

        //Left animation
        if (playLeftAnimation && UIHelper.Translate(chickyLeftTransform, new Vector3(-1f, 0), originalChickyLeftPosition + new Vector3(-90f, 0)))
        {
            playLeftAnimation = false;
            chickyLeftTransform.position = originalChickyLeftPosition;
        }

        //Right Animation 

        if (playRightAnimation && UIHelper.Translate(chickyRightTransform, new Vector3(1f, 0), originalChickyRightPosition + new Vector3(90f, 0)))
        {
            playRightAnimation = false;
            chickyRightTransform.position = originalChickyRightPosition;
        }

    }


}
