using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//TO-DO: NEED TO DESTROY THE SCENE IF THEY GO BACK (IE, CUSTOM BACK BUTTON SCRIPT) OR IF THEY'RE DONE
//TO-DO: NEED TO MAKE A RESULTS OBJECT THAT STORES DATA?

public class JeopardySceneScript : MonoBehaviour
{
    private Button[] optionButtons; //Could make an array of arrays but that's more lines of code, unneeded
    private RectTransform[] transforms;
    private int pressedButton;
    private CanvasGroup buttonCanvasGroup;

    public static List<int> invisibleButtons = new List<int>(); // don't want to reset in start every time, initialized here
    public static int SpeechInNoiseTrialNum; //default value is 0, public bc shared
    public static int score;

    // Start is called before the first frame update
    private void Start()
    {
        optionButtons = new Button[25];
        transforms = new RectTransform[25];
        //Screen.orientation = ScreenOrientation.LandscapeLeft;

        GameObject.Find("ScoreText").GetComponent<Text>().text = "Score: " + score.ToString();

        //Assign buttons and transforms
        for (int i = 1; i <= 25; i++)
        {
            optionButtons[i - 1] = GameObject.Find("Button" + i).GetComponent<Button>(); //Buttons 1-5 are for Cat 1, 6-10 for Cat 2, etc.
            transforms[i - 1] = GameObject.Find("Button" + i).GetComponent<RectTransform>();
        }

        //Set GUI
        for (int columnNum = 0; columnNum < 5; columnNum++)
        {
            GameObject.Find("ButtonCat" + columnNum).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height / 12);
            GameObject.Find("ButtonCat" + columnNum).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width / 5);
            GameObject.Find("ButtonCat" + columnNum).GetComponent<RectTransform>().position = new Vector3(Screen.width / 5 * columnNum, Screen.height * 11 / 24, 0);

            for (int rowNum = 0; rowNum < 5; rowNum++)
            {
                transforms[5 * columnNum + rowNum].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height / 12);
                transforms[5 * columnNum + rowNum].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width / 5);
                transforms[5 * columnNum + rowNum].position = new Vector3(Screen.width / 5 * columnNum, (Screen.height / 12 * (5 - rowNum)) - (Screen.height / 12), 0);
            }
        }


        SetOptionButtonListeners();

        //Need this loop because for some reason the buttons don't stay invisible upon reentering screen
        for (int i = 0; i < invisibleButtons.Count; i++)
        {
            Debug.Log(invisibleButtons[i]);
            buttonCanvasGroup = optionButtons[invisibleButtons[i]].GetComponent<CanvasGroup>();
            optionButtons[invisibleButtons[i]].onClick.RemoveAllListeners(); //To exclude buttons
            buttonCanvasGroup.alpha = 0;
        }
    }

    public void SetOptionButtonListeners()
    {
        for (int i = 0; i < 25; i++)
        {
            int buttonNum = i;
            optionButtons[i].onClick.AddListener(() => OnSpeechButtonClick(buttonNum));
        }
    }

    private void OnSpeechButtonClick(int buttonNum)
    {
        invisibleButtons.Add(buttonNum);

        //Button doesn't stay invisible?
        buttonCanvasGroup = optionButtons[buttonNum].GetComponent<CanvasGroup>();
        optionButtons[buttonNum].onClick.RemoveAllListeners();
        buttonCanvasGroup.alpha = 0;

        pressedButton = buttonNum;
        SpeechInNoiseTrialNum = pressedButton + 1; //ie, pressing Button1 activates Trial 1 (+1 is since Button1 is optionButton[0])

        DontDestroyOnLoad(gameObject); //NOT DESTROYING THE GAME OBJECT ALLOWS THE SPEECHINNOISEGAMESCRIPT TO USE THE VALUE FOR SpeechInNoiseTrialNum
        SceneManager.LoadScene("SpeechInNoiseGame");
    }

    // Update is called once per frame
    private void Update()
    {
        UIHelper.OnBackButtonClickListener("MainMenu");
    }
}