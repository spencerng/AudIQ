using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//TO DO: NEED TO DESTROY THE SCENE IF THEY GO BACK (IE, CUSTOM BACK BUTTON SCRIPT) OR IF THEY'RE DONE
//CREATE TEXT KEEPING TRACK OF POINTS
//MAKING BUTTONS DISSAPPEAR AND REMOVING LISTENERS AFTER TRIAL

public class JeopardySceneScript : MonoBehaviour
{
    private Button[] optionButtons; //Could make an array of arrays but that's more lines of code, unneeded
    private RectTransform[] transforms;
    public static int SpeechInNoiseTrialNum; //default value is 0, public bc shared
    private int pressedButton;
    private CanvasGroup buttonCanvasGroup;
    public static List<int> invisibleButtons = new List<int>(); // don't want to reset in start every time
    public static int score;

    // Start is called before the first frame update
    void Start()
    {
        optionButtons = new Button[25];
        transforms = new RectTransform[25];
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        GameObject.Find("ScoreText").GetComponent<Text>().text = "Score: " + score.ToString();

        for (int i = 1; i <= 25; i++)
        {
            optionButtons[i - 1 ] = GameObject.Find("Button" + i).GetComponent<Button>(); //Buttons 1-5 are for Cat 1, 6-10 for Cat 2, etc.
            transforms[i - 1] = GameObject.Find("Button" + i).GetComponent<RectTransform>();
        }

        int height, width;

        //glitches when entering landscape mode; this forces width to be width and height to be height
        if (Screen.width > Screen.height)
        {
            height = Screen.height;
            width = Screen.width;
        } else
        {
            Debug.Log("E");
            height = Screen.width;
            width = Screen.height;
        }


        //Set GUI
        for (int columnNum = 0; columnNum < 5; columnNum++) {
            for (int rowNum = 0; rowNum < 5; rowNum++)
            {
                transforms[5 * columnNum + rowNum].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height / 12);
                transforms[5 * columnNum + rowNum].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width / 5);
                transforms[5*columnNum + rowNum].position = new Vector3(width / 5 * columnNum, (height/12 * (5 - rowNum)) - (height/12), 0);
            }
        }
        

        SetOptionButtonListeners();

        //Need this loop because for some reason the buttons don't stay invisible
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
            //Additional variable needed for a "static" value reference
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
        SpeechInNoiseTrialNum = pressedButton; //ie, pressing Button1 activates Trial 0 (since Button1 is optionButton[0])


        DontDestroyOnLoad(this.gameObject); //NOT DESTROYING THE GAME OBJECT ALLOWS THE SPEECHINNOISEGAMESCRIPT TO USE THE VALUE FOR SpeechInNoiseTrialNum
        SceneManager.LoadScene("SpeechInNoiseGame");



    }

    // Update is called once per frame
    void Update()
    {
        UIHelper.OnBackButtonClickListener("MainMenu");
    }
}
//need to log all the selected tiles: turn invisible, turn off listener.
//On click: load speechinnoise scene, play ONE trial (associated with the button_num), & based on if correct, update score
//Then, reload this scene. 