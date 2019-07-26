using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHelper : MonoBehaviour
{
    public static void OnBackButtonClickListener(string scene)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene(scene);
            }

        }
    }

    public static IEnumerator FlashCorrectIncorrectScreen(bool isCorrect)
    {

        Text correctIncorrectText = GameObject.Find("CorrectIncorrectText").GetComponent<Text>();
        CanvasGroup correctIncorrectTextCg = GameObject.Find("CorrectIncorrectText").GetComponent<CanvasGroup>();

        Image correctIncorrectPanel = GameObject.Find("CorrectIncorrectPanel").GetComponent<Image>();
        CanvasGroup correctIncorrectPanelCg = GameObject.Find("CorrectIncorrectPanel").GetComponent<CanvasGroup>();

        Color backgroundColor;
        string textToDisplay;

        if (isCorrect)
        {
            backgroundColor = UnityEngine.Color.green;
            textToDisplay = "Correct";
        }
        else
        {
            backgroundColor = UnityEngine.Color.red;
            textToDisplay = "Incorrect";
        }

        return FlashCorrectIncorrectScreen(correctIncorrectTextCg, correctIncorrectPanelCg, correctIncorrectPanel, correctIncorrectText,
                textToDisplay, backgroundColor);
    }

    private static IEnumerator FlashCorrectIncorrectScreen(CanvasGroup textCg, CanvasGroup panelCg, Image panel, Text text,
       string textStr, Color color, float timeBetweenFlash = 0.5f, float timeToWait = 0.75f)
    {
        bool changed = false;
        for (int i = 0; i < 2; i++)
        {
            if (!changed)
            {
                text.text = textStr;
                textCg.alpha = 1;

                panel.color = color;
                panelCg.alpha = 0.75f;

                changed = true;
            }
            else
            {
                textCg.alpha = 0;
                panelCg.alpha = 0;
            }
            yield return new WaitForSeconds(timeBetweenFlash);
        }
        yield return new WaitForSeconds(timeToWait);
    }

    //Return: finalPos reached
    public static bool Translate(Transform target, Vector3 direction, Vector3 finalPos)
    {
        target.position = target.position + direction;


        return target.position.Equals(finalPos);

    }
}