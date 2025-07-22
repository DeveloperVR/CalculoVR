using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureFunction : MonoBehaviour
{
    private string input;
    public void textedCaptured(string text)
    {
        input = text;
        Debug.Log("Function captured successfully!");
        Debug.Log("The text captured was: " + input);
        // Additional logic for when the function is captured can be added here
    }
}
