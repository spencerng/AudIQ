using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpeechInNoiseGameScript : MonoBehaviour
{
    public int numCorrect, numWrong, percentCorrect, numtrial;
    public bool isClicked;

    public Text CorrectIncorrectText;
    public CanvasGroup CorrectIncorrectTextCg;
    public Image CorrectIncorrectPanel;
    public CanvasGroup CorrectIncorrectPanelCg;

    public Button[] wordButtons = new Button[12];
    public AudioObj[] audioObjs = new AudioObj[12]; //One object has audioSource, audioClip, correctAnswer, and bool correctlyAnswered

    public AudioObj readyFile;

    // Start is called before the first frame update
    void Start()
    {
        CorrectIncorrectText = GameObject.Find("CorrectIncorrectText").GetComponent<Text>();
        CorrectIncorrectTextCg = GameObject.Find("CorrectIncorrectText").GetComponent<CanvasGroup>();

        CorrectIncorrectPanel = GameObject.Find("CorrectIncorrectPanel").GetComponent<Image>();
        CorrectIncorrectPanelCg = GameObject.Find("CorrectIncorrectPanel").GetComponent<CanvasGroup>();

        numtrial = 1; //initialized here because doing it before the first method glitched, was set to 0

        //Array of buttons
        for (int i = 0; i < 12; i++)
        {
            wordButtons[i] = GameObject.Find("SpeechGameButton" + (i + 1)).GetComponent<Button>(); //initializes button array
            //moved adding listeners to AssignButtonOptions()
        }
        
        AudioSource[] audioSources = GetComponents<AudioSource>(); //Note this will be 13; first index of 0 is "ready"
        readyFile = new AudioObj(audioSources[0]);

        for (int i = 1; i < audioSources.Length; i++)
        {
            AudioObj a1 = new AudioObj(audioSources[i]);
            audioObjs[i - 1] = a1;
        }
        
        SetButtonListeners();
        StartCoroutine(StartTrial(numtrial));
    }

    
    void SetButtonListeners()
    {
        for (int i = 0; i < 12; i++)
        {
            int buttonNum = i;
            //Add listener here?
            wordButtons[i].onClick.AddListener(() => OnSpeechButtonClick(buttonNum));
        }
    }

    public IEnumerator StartTrial(int trial_number)
    {
        //Initial pause to allow the Correct/Incorrect flash to finish
        yield return new WaitForSeconds(0.7f);

        RetrieveCorrectAnswers();
        AssignButtonOptions();

        readyFile.GetAudioSource().PlayOneShot(readyFile.GetAudioClip());
        yield return new WaitForSeconds(1f);

        audioObjs[trial_number - 1].GetAudioSource().PlayOneShot(audioObjs[trial_number - 1].GetAudioClip()); //plays clip, trial 1 corresponds to index 0

        yield return new WaitForSeconds(1f);
    }

    void RetrieveCorrectAnswers()
    {
        //Will do later
        //Possible options: Resource.Load, making 12 GameObjects and attaching each file individually, then AudioSource.name
        //Note: AudioSource.name returns name of the GameObject it's attached to, not the file :/
        audioObjs[0].SetCorrectAnswer("goose");
        audioObjs[1].SetCorrectAnswer("what");
        audioObjs[2].SetCorrectAnswer("touch");
        audioObjs[3].SetCorrectAnswer("noise");
        audioObjs[4].SetCorrectAnswer("juice");
        audioObjs[5].SetCorrectAnswer("food");
        audioObjs[6].SetCorrectAnswer("choose");
        audioObjs[7].SetCorrectAnswer("cheese");
        audioObjs[8].SetCorrectAnswer("boot");
        audioObjs[9].SetCorrectAnswer("boat");
        audioObjs[10].SetCorrectAnswer("beach");
        audioObjs[11].SetCorrectAnswer("bought");
    }



    void AssignButtonOptions()
    {
        //Array of strings and list of indices that will be shuffled
        string[] buttonOptions = GetButtonOptions();
        List<int> indexlist = new List<int>();

        for (int i = 0; i < buttonOptions.Length; i++)
        {
            indexlist.Add(i);
        }

        List<int> randomindexlist = Shuffle(indexlist);

        //Assigning words to the buttons based on the randomized list
        for (int i = 0; i < buttonOptions.Length; i++)
        {
            wordButtons[i].GetComponentInChildren<Text>().text = buttonOptions[randomindexlist[i]];
        }

    }

    private string[] GetButtonOptions()
    {
        //string[] buttonoptions = new string[12];

        //temporararily did this, we'll have to actually retrieve text from buttons later
        string[] buttonOptions = { "beach", "boot", "touch", "boat", "juice", "bought", "food", "goose", "noise", "cheese", "what", "choose" };

        return buttonOptions;
    }

    //Algorithm: Fisher-Yates Shuffle, taken from: https://stackoverflow.com/questions/273313/randomize-a-listt
    private List<int> Shuffle(List<int> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }

        return list;
    }

    void OnSpeechButtonClick(int buttonNum)
    {
        isClicked = true;

        int index_for_numtrial = numtrial - 1;
        //Should probably add conditional as a defensive programming measure -- if people do the 12 trials and press another button, shouldn't count it

        if (numtrial <= audioObjs.Length)
        {
            if (wordButtons[buttonNum].GetComponentInChildren<Text>().text == audioObjs[index_for_numtrial].GetCorrectAnswer())
            {
                audioObjs[index_for_numtrial].AnsweredCorrectly();
                //play correct animation
                StartCoroutine(FlashCorrectScreen(CorrectIncorrectTextCg, CorrectIncorrectPanelCg, CorrectIncorrectPanel, CorrectIncorrectText));

            }
            else
            {
                audioObjs[index_for_numtrial].DidNotAnswerCorrectly();
                //play incorrect animation
                StartCoroutine(FlashInCorrectScreen(CorrectIncorrectTextCg, CorrectIncorrectPanelCg, CorrectIncorrectPanel, CorrectIncorrectText));

            }
        }

        numtrial++;

        if (numtrial <= audioObjs.Length)
        {
            StartCoroutine(StartTrial(numtrial));
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

    public IEnumerator FlashInCorrectScreen(CanvasGroup text_cg, CanvasGroup panel_cg, Image panel, Text text, float timeBetweenFlash = 0.5f, float timeToWait = 0.75f)
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
        AndroidBackButton();

    }

    void AndroidBackButton()
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

/*
 * Added the flash correct/incorrect methods
 * made the start trial a coroutine 
 * 
 * */
