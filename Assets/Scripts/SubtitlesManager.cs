using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubtitlesLibrary : MonoBehaviour
{
    private string subtitles1;
    private string subtitles2;
    private string subtitles3;


    [SerializeField] private SubtitlesDB subtitleslibrary;
    [SerializeField] private SubtitlesDB subtitleslibrary2;
    [SerializeField] private SubtitlesDB subtitleslibrary3;
    [SerializeField] private TMP_Text subtitlesText;
    [SerializeField] private Canvas subtitlescanvas;
    [SerializeField] private Transform cam;
    private bool active;
    private int duration;

    void LateUpdate()
    {
        if (enabled == true)
        {
            Vector3 forward = cam.forward;
            Vector3 canvasposition = cam.position + forward * 2;
            canvasposition.y = cam.position.y - 2;
            subtitlescanvas.transform.position = canvasposition;
            subtitlescanvas.transform.LookAt(cam);
            subtitlescanvas.transform.Rotate(0, 180, 0); // Rotate to face the camera
        }
    }


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

    public int GetTimeOfDisplay(SubtitlesDB origin)
    {
        int time = origin.GetTimeOfDisplay;
        if (time <= 0)
        {
            Debug.LogWarning("Invalid display time for subtitles.");
            return 5; // Default display time if invalid
        }
        return time;
    }

    void Awake()
    {
        subtitles1 = GetSubtitles(subtitleslibrary);
        subtitles2 = GetSubtitles(subtitleslibrary2);
        subtitles3 = GetSubtitles(subtitleslibrary3);
    }

    public void SetSubtitlesText(int indice)
    {
        switch (indice)
        {
            case 0:
                subtitlesText.text = subtitles1;
                duration = GetTimeOfDisplay(subtitleslibrary);
                enablecanvas(subtitlescanvas);
                break;
            case 1:
                subtitlesText.text = subtitles2;
                duration = GetTimeOfDisplay(subtitleslibrary2);
                enablecanvas(subtitlescanvas);
                break;
            case 2:
                subtitlesText.text = subtitles3;
                duration = GetTimeOfDisplay(subtitleslibrary3);
                enablecanvas(subtitlescanvas);
                break;
            default:
                Debug.LogWarning("Invalid subtitle index.");
                subtitlesText = null;
                break;
        }
        if (subtitlesText == null)
        {
            Debug.Log("No hay texto valido para mostrar");
            disablecanvas(subtitlescanvas);
            active = false;
            return;
        }
        else
        {
            enablecanvas(subtitlescanvas);
            float timecounter = 0;
            while (timecounter < duration)
            {
                timecounter += Time.deltaTime;
                //Debug.Log("Tiempo de espera: " + timecounter);
                active = true;
            }
            active = false;
        }

    }

    public void disablecanvas(Canvas canvas)
    {
        canvas.gameObject.SetActive(false);
    }

    void enablecanvas(Canvas canvas)
    {
        canvas.gameObject.SetActive(true);
    }

}