using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JeopardySceneScript : MonoBehaviour
{
    private Button[] optionButtons; //Could make an array of arrays but that's more lines of code, unneeded
    protected int SpeechInNoiseTrialNum; //default value is 0
    public int pressedButton;

    // Start is called before the first frame update
    void Start()
    {
        optionButtons = new Button[25];
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        for (int i = 1; i <= 25; i++)
        {
            optionButtons[i - 1 ] = GameObject.Find("Button" + i).GetComponent<Button>(); //Buttons 1-5 are for Cat 1, 6-10 for Cat 2, etc.
        }


        SetOptionButtonListeners();
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
        CanvasGroup buttonCanvasGroup = GameObject.Find("Button" + buttonNum).GetComponent<CanvasGroup>();
        optionButtons[buttonNum].onClick.RemoveAllListeners();
        buttonCanvasGroup.alpha = 0;

        pressedButton = buttonNum;

        SetSpeechInNoiseTrialNum();
        Debug.Log(SpeechInNoiseTrialNum);
        SceneManager.LoadScene("SpeechInNoiseGame");



    }

    protected void SetSpeechInNoiseTrialNum()
    {
        SpeechInNoiseTrialNum = pressedButton - 1; //ie, pressing Button1 activates Trial 0 (since Button1 is optionButton[0])
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