using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIScript : MonoBehaviour
{
    private AudioPlayer player;

    public void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        player = new AudioPlayer(GameObject.Find("AudioManager").GetComponent<AudioSource>(), 0.0f, 2.0f);
        player.Play();
    }

    public void Update()
    {
        Slider slider = GameObject.Find("Slider").GetComponent<Slider>();
        player.SetOffsetAngle(-slider.value);

        Slider sliderPitch = GameObject.Find("Slider (1)").GetComponent<Slider>();
        //player.SetPitch(sliderPitch.value);

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

