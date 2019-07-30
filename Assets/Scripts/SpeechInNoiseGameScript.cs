using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


internal class SpeechInNoiseGameScript : JeopardySceneScript //NOTE: I made it inherit the protected int numButton from JeopardySceneScript
{
    private readonly int numCorrect;
    private readonly int numWrong;
    private readonly int percentCorrect;
    private int numTrial;

    private readonly Text correctIncorrectText;
    private readonly CanvasGroup correctIncorrectTextCg;

    private readonly Image correctIncorrectPanel;
    private readonly CanvasGroup correctIncorrectPanelCg;

    private Button[] wordButtons;
    private AudioObj[] audioObjs;
    private AudioObj readyFile;

    //TO-DO: Will need to make method interacting with database, GetAudioFiles();

    private void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        wordButtons = new Button[12];
        audioObjs = new AudioObj[12];

        numTrial = 1; //WILL HAVE TO COMMENT THIS OUT AFTER WE HAVE 25 TRIALS
        //numTrial = SpeechInNoiseTrialNum; //SpeechInNoiseTrialNum is public static, from JeopardySceneScript

        //SpeechInNoiseTrialNum IS FROM JEOPARDYSCENE, HAS NOT BEEN DESTROYED

        //Assign buttons
        for (int i = 0; i < 12; i++)
        {
            wordButtons[i] = GameObject.Find("SpeechGameButton" + (i + 1)).GetComponent<Button>();
        }

        //TO-DO: Will need to turn this into a method at a later date, GetAudioFiles();
        // Has length of 13; element 0 is "ready"
        AudioSource[] audioSources = GetComponents<AudioSource>();
        readyFile = new AudioObj(audioSources[0]);
        for (int i = 1; i < audioSources.Length; i++)
        {
            audioObjs[i - 1] = new AudioObj(audioSources[i]);
        }

        //Done so original button text doesn't appear on the screen
        AssignButtonOptions();

        StartCoroutine(StartTrial(numTrial));

    }

    private void Update()
    {
        UIHelper.OnBackButtonClickListener("JeopardyGame");
    }

    private IEnumerator OnSpeechButtonClick(int buttonNum)
    {
        RemoveWordButtonListeners();

        if (numTrial <= audioObjs.Length)
        {
            bool isCorrect = wordButtons[buttonNum].GetComponentInChildren<Text>().text == audioObjs[numTrial - 1].GetCorrectAnswer();

            if (isCorrect)
            {
                score = score + 100 + 100*((SpeechInNoiseTrialNum - 1) % 5); //For example, pressing button5 = trial 5, 100 + 100*(4 % 5)
            }
            audioObjs[numTrial - 1].SetCorrectlyAnswered(isCorrect);

            StartCoroutine(UIHelper.FlashCorrectIncorrectScreen(isCorrect));

        }

        //numTrial++; //Had this when we played one trial after another in the original SpeechInNoiseGame


        if (numTrial <= audioObjs.Length)
        {
            DontDestroyOnLoad(this.gameObject); //Need this so that the data in the invisibleButtons List does not get deleted
            //Initial pause to allow the Correct/Incorrect flash to finish
            yield return new WaitForSeconds(0.7f);
            SceneManager.LoadScene("JeopardyGame");
            //StartCoroutine(StartTrial(numTrial)); //don't want a new trial; want to go back to jeopardy
        }

    }

    private IEnumerator StartTrial(int trialNum)
    {
        AssignButtonOptions();
        RetrieveCorrectAnswers();

        readyFile.GetAudioSource().PlayOneShot(readyFile.GetAudioClip());
        yield return new WaitForSeconds(1f);

        //plays clip, trial 1 corresponds to index 0
        audioObjs[trialNum - 1].GetAudioSource().PlayOneShot(audioObjs[trialNum - 1].GetAudioClip());

        //Set and remove new listeners every trial to prevent registering accidental double taps
        SetWordButtonListeners();

        yield return new WaitForSeconds(1f);
    }



    private void SetWordButtonListeners()
    {
        for (int i = 0; i < 12; i++)
        {
            //Additional variable needed for a "static" value reference
            int buttonNum = i;

            wordButtons[i].onClick.AddListener(() => StartCoroutine(OnSpeechButtonClick(buttonNum)));
        }
    }

    private void RemoveWordButtonListeners()
    {
        for (int i = 0; i < 12; i++)
        {
            wordButtons[i].onClick.RemoveAllListeners();
        }
    }



    private void AssignButtonOptions()
    {
        //Array of strings and list of indices that will be shuffled
        string[] buttonOptions = GetButtonOptions();
        List<int> indexlist = new List<int>();

        for (int i = 0; i < buttonOptions.Length; i++)
        {
            indexlist.Add(i);
        }

        //Probably don't need to randomize buttons, but will need to randomize files drawn from database
        List<int> randomindexlist = Algorithms.ShuffleList(indexlist);

        //Assigning words to the buttons based on the randomized list
        for (int i = 0; i < buttonOptions.Length; i++)
        {
            wordButtons[i].GetComponentInChildren<Text>().text = buttonOptions[randomindexlist[i]];
        }

    }

    private string[] GetButtonOptions()
    {
        //temporararily did this, we'll have to actually retrieve text from buttons later (ie, each audio file in a database has 12 associated options)
        string[] buttonOptions = { "beach", "boot", "touch", "boat", "juice", "bought", "food", "goose", "noise", "cheese", "what", "choose" };

        return buttonOptions;
    }

    private void RetrieveCorrectAnswers()
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

}

