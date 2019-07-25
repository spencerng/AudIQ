using UnityEngine;
using UnityEngine.SceneManagement;

public class UIHelper : ScriptableObject
{
    public static void OnBackButtonClickListener()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneManager.LoadScene("MainMenu");
            }

        }
    }

    public static void FlashCorrectIncorrectScreen(bool isCorrect)
    {

    }
}