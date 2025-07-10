using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class DebugAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public void SuccesAudio()
    { 
        if (audioSource != null)
        {
            audioSource.Play();
            return;
        }
        else
        {
            Debug.LogError("AudioSource is not assigned.");
            return;
        }
    }
}
