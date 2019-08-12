using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIScript : MonoBehaviour
{
    public void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

    }

    public void Update()
    {
        Slider slider = GameObject.Find("Slider").GetComponent<Slider>();
        Slider sliderPitch = GameObject.Find("Slider (1)").GetComponent<Slider>();
    }

    public void StartSpeechInNoiseGame()
    {
        StartCoroutine(SwitchToLandscapeThenStartGame("JeopardyGame"));

    }

    public IEnumerator SwitchToLandscapeThenStartGame(string scene)
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene);
    }

    public void StartSoundLocalizationGame()
    {
        SceneManager.LoadScene("SoundLocalizationGame");
    }

    public void StartPitchDiscriminationGame()
    {

        SceneManager.LoadScene("PitchDiscriminationGame");
    }

    public void StartTouchGame()
    {
        SceneManager.LoadScene("TouchGame");
    }

}

