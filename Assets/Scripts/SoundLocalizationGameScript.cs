using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    private bool playTopAnimation;

    private Vector3 originalChickyTopPosition;
    private Vector3 originalChickyLeftPosition;
    private Vector3 originalChickyRightPosition;

    private Text correctIncorrectText;
    private CanvasGroup correctIncorrectTextCg;
    private Image correctIncorrectPanel;
    private CanvasGroup correctIncorrectPanelCg;

    private int numTrial;

    // Start is called before the first frame update
    private void Start()
    {
        chickyTop = GameObject.Find("chicky_T").GetComponent<Image>();
        chickyLeft = GameObject.Find("chicky_L").GetComponent<Image>();
        chickyRight = GameObject.Find("chicky_R").GetComponent<Image>();

        chickyTopTransform = GameObject.Find("chicky_T").GetComponent<Transform>();
        chickyLeftTransform = GameObject.Find("chicky_L").GetComponent<Transform>();
        chickyRightTransform = GameObject.Find("chicky_R").GetComponent<Transform>();

        buttonLeft = GameObject.Find("Button_L").GetComponent<Button>(); //Note buttons are invisible
        buttonRight = GameObject.Find("Button_R").GetComponent<Button>();

        correctIncorrectText = GameObject.Find("CorrectIncorrectText").GetComponent<Text>();
        correctIncorrectTextCg = GameObject.Find("CorrectIncorrectText").GetComponent<CanvasGroup>();

        correctIncorrectPanel = GameObject.Find("CorrectIncorrectPanel").GetComponent<Image>();
        correctIncorrectPanelCg = GameObject.Find("CorrectIncorrectPanel").GetComponent<CanvasGroup>();

        AudioSource[] audioSources = GetComponents<AudioSource>();

        for (int i = 0; i < audioSources.Length; i++)
        {
            audioObjs[i] = new AudioObj(audioSources[i]);
        }

        numTrial = 1;

        originalChickyTopPosition = chickyTopTransform.position;
        playTopAnimation = true; //just an initial animation

        StartCoroutine(StartTrial(numTrial));
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

    private void RetrieveCorrectAnswers()
    {
        audioObjs[0].SetCorrectAnswer("left");
        audioObjs[1].SetCorrectAnswer("right");
    }

    private void SetButtonListeners()
    {
        buttonLeft.onClick.AddListener(() => OnButtonClick("left"));
        buttonRight.onClick.AddListener(() => OnButtonClick("right"));

    }

    private void OnButtonClick(string left_or_right)
    {
        RemoveListeners();

        int indexForTrial = numTrial - 1;

        if (numTrial <= audioObjs.Length)
        {

            if (left_or_right == "left")
            {
                originalChickyLeftPosition = chickyLeftTransform.position;
                playLeftAnimation = true;

                //add in correct/incorrect 
                if (audioObjs[indexForTrial].GetCorrectAnswer() == left_or_right)
                {
                    audioObjs[indexForTrial].SetCorrectlyAnswered(true);
                    StartCoroutine(FlashCorrectScreen(correctIncorrectTextCg, correctIncorrectPanelCg, correctIncorrectPanel, correctIncorrectText));
                }
                else
                {
                    audioObjs[indexForTrial].SetCorrectlyAnswered(false);
                    //play incorrect animation
                    StartCoroutine(FlashIncorrectScreen(correctIncorrectTextCg, correctIncorrectPanelCg, correctIncorrectPanel, correctIncorrectText));
                }

            }
            else
            {
                originalChickyRightPosition = chickyRightTransform.position;
                playRightAnimation = true;
                //add in correct/incorrect animations
                if (audioObjs[indexForTrial].GetCorrectAnswer() == left_or_right)
                {
                    audioObjs[indexForTrial].SetCorrectlyAnswered(true);
                    StartCoroutine(FlashCorrectScreen(correctIncorrectTextCg, correctIncorrectPanelCg, correctIncorrectPanel, correctIncorrectText));
                }
                else
                {
                    audioObjs[indexForTrial].SetCorrectlyAnswered(false);
                    //play incorrect animation
                    StartCoroutine(FlashIncorrectScreen(correctIncorrectTextCg, correctIncorrectPanelCg, correctIncorrectPanel, correctIncorrectText));
                }

            }

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
    private void Update()
    {
        UIHelper.OnBackButtonClickListener();

        //Top animation
        if (playTopAnimation)
        {
            chickyTopTransform.position = chickyTopTransform.position + new Vector3(0, -1f, 0);
        }
        if (originalChickyTopPosition.y - chickyTopTransform.position.y > 100)
        {
            playTopAnimation = false;
        }

        //Left animation
        if (playLeftAnimation)
        {
            chickyLeftTransform.position = chickyLeftTransform.position + new Vector3(-1f, -0, 0);
        }
        if (originalChickyLeftPosition.x - chickyLeftTransform.position.x > 90)
        {
            chickyLeftTransform.position = originalChickyLeftPosition;
            playLeftAnimation = false;
        }

        //Right Animation
        if (playRightAnimation)
        {
            chickyRightTransform.position = chickyRightTransform.position + new Vector3(1f, -0, 0);
        }
        if (chickyRightTransform.position.x - originalChickyRightPosition.x > 90) //position it moves to is > than original position
        {
            chickyRightTransform.position = originalChickyRightPosition;
            playRightAnimation = false;
        }

    }


}
