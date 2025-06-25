 using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubtitlesLibrary : MonoBehaviour
{
    public string subtitles1;
    public string subtitles2;
    public string subtitles3;


    [SerializeField] private SubtitlesDB subtitleslibrary;
    [SerializeField] private SubtitlesDB subtitleslibrary2;
    [SerializeField] private SubtitlesDB subtitleslibrary3;
    [SerializeField] private TMP_Text subtitlesText;

    public string GetSubtitles(SubtitlesDB origin)
    {
        string subtitles = origin.GetSubtitles;
        if (subtitles == null || subtitles.Length == 0)
        {
            Debug.LogWarning("Subtitles are empty or null.");
            return "No subtitles available.";
        }
        return subtitles;
    }

    void Awake()
    {
        subtitles1 = GetSubtitles(subtitleslibrary);
        subtitles2 = GetSubtitles(subtitleslibrary2);
        subtitles3 = GetSubtitles(subtitleslibrary3);
    }

    public void SetSubtitlesText(int indice, float duration)
    {
        switch (indice)
        {
            case 1:
                subtitlesText.text = subtitles1;
                break;
            case 2:
                subtitlesText.text = subtitles2;
                break;
            case 3:
                subtitlesText.text = subtitles3;
                break;
            default:
                Debug.LogWarning("Invalid subtitle index.");
                break;
        }

    }

}
