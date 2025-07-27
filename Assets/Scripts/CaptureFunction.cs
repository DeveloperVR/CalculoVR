using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureFunction : MonoBehaviour
{
    public string input { get; private set; }

    public void textedCaptured(string text)
    {
        input = text;
        Debug.Log("Function captured successfully!");
        Debug.Log("The text captured was: " + input);
        // Additional logic for when the function is captured can be added here
    }
}
