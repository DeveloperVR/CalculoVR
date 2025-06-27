using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] audios;
    private AudioSource controlAudio;
    private bool isPlaying = false;
    [SerializeField] SubtitlesLibrary subtitles; 

    private void Awake()
    {
        controlAudio = GetComponent<AudioSource>();
    }

    public void SeleccionAudio(int indice, float volume, bool loop)
    {
        if (isPlaying)
        {
            return;
        }
        isPlaying = true;
        controlAudio.clip = audios[indice];
        controlAudio.volume = volume;
        controlAudio.loop = loop;
        controlAudio.Play();
        StartCoroutine(WaitForAudioToFinish());
        subtitles.SetSubtitlesText(indice);

    }
    private IEnumerator WaitForAudioToFinish()
    {
        yield return new WaitWhile(() => controlAudio.isPlaying);
        isPlaying = false;
    }

    public void StopAudio()
    {
        if (controlAudio.isPlaying)
        {
            controlAudio.Stop();
            isPlaying = false;
        }
    }
}
