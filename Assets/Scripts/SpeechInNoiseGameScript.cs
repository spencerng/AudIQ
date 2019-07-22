using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpeechInNoiseGameScript : MonoBehaviour
{
    public int numCorrect, numWrong, percentCorrect, numtrial;
    public bool isClicked;

    
    public Button[] wordButtons = new Button[12];
    public AudioObj[] audioObjs = new AudioObj[12]; //One object has audioSource, audioClip, correctAnswer, and bool correctlyAnswered
    

    // Start is called before the first frame update
    void Start()
    {


        isClicked = false;
        numtrial = 1; //initialized here because doing it before the first method glitched, was set to 0

        //Array of buttons
        for (int i = 0; i < 12; i++)
        {
            wordButtons[i] = GameObject.Find("SpeechGameButton" + (i + 1)).GetComponent<Button>(); //initializes button array
            //moved adding listeners to AssignButtonOptions()
        }

        AudioSource[] audioSources = GetComponents<AudioSource>();

        for (int i = 0; i < audioSources.Length; i++)
        {
            AudioObj a1 = new AudioObj(audioSources[i]);
            audioObjs[i] = a1;
        }

        
        //just testing 
        SetButtonListeners();
        StartTrial(numtrial);

        //What I want to do: Start trial, assign buttons, read prompt, wait for user button press, then start new trial
        


    

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

    void StartTrial(int trial_number)
    {
        RetrieveCorrectAnswers();
        AssignButtonOptions();
        //play prompt, ready or something
        audioObjs[trial_number - 1].GetAudioSource().PlayOneShot(audioObjs[trial_number - 1].GetAudioClip()); //plays clip, trial 1 corresponds to index 0

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
            }
            else
            {
                audioObjs[index_for_numtrial].DidNotAnswerCorrectly();
                //play incorrect animation
            }
        }

        numtrial++;

        if (numtrial <= audioObjs.Length)
        {
            StartTrial(numtrial);
        }
        
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
