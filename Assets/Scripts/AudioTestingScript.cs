using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTestingScript : MonoBehaviour
{
    public float[] listenerSamples = new float[128];
    public float[] sourceSamples = new float [128];
    public AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();

        CollectData();
    }

    // Update is called once per frame
    void Update()
    {
        CollectData();
    }

    public void CollectData()
    {

        source.GetSpectrumData(sourceSamples, 0, FFTWindow.Blackman);
        AudioListener.GetSpectrumData(listenerSamples, 0, FFTWindow.Blackman);

        //yield return 3f;


    }
}
