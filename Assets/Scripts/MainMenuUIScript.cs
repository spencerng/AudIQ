using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIScript : MonoBehaviour
{
    private TargetBeaconPlayer player;

    public void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        AudioTest();
    }

    public void Update()
    {
        Slider sliderOffsetAngle = GameObject.Find("Slider").GetComponent<Slider>();
        Slider sliderPitch = GameObject.Find("Slider (1)").GetComponent<Slider>();

        player.SetBeaconOffsetAngle(-sliderOffsetAngle.value);
        player.SetBeaconPitch(sliderPitch.value);
        Debug.Log(player.GetAbsoluteOffsetAngleDifference() + ", " + player.GetAbsolutePitchDifference());
    }

    public void AudioTest()
    {
        player = GameObject.Find("TargetBeaconManager").GetComponent<TargetBeaconPlayer>();
        player.Play();
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

