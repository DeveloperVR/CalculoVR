using UnityEngine;

public class DesktopMiniMap : MonoBehaviour
{
    public GameObject MiniMap;

    public void ActivateMiniMap()
    { 
        MiniMap.SetActive(true);
    }
}
