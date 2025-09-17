using UnityEngine;

public class DesktopMiniMap : MonoBehaviour
{
    public GameObject MiniMap;
    private bool isPCPlayer;

    public void ActivateMiniMap()
    {
        MiniMap.SetActive(true);
    }

    public void SetPlayerType(bool isPC)
    {
        isPCPlayer = isPC;
        if (isPCPlayer)
        {
            MiniMap.SetActive(true);
        }
        else
        {
            MiniMap.SetActive(false);
        }
    }
    
}
