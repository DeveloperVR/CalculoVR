using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CartMovementManagerSpawnEvent : MonoBehaviour
{
    public UnityEvent OnCartMovementManagerSpawned; // Event to invoke when the CartMovementManager is spawned.
    public bool cartMovementManagerSpawned = false; // Flag to check if the CartMovementManager has been spawned.
    void Start()
    {
        StartCoroutine(WaitForCartMovementManager());
    }

   IEnumerator WaitForCartMovementManager()
   {
       while (!cartMovementManagerSpawned || RCGameManager.Instance.CartMovementManager == null)
       {
           yield return null; // Wait until the CartMovementManager is spawned.
       }
       OnCartMovementManagerSpawned.Invoke(); // Invoke the event when the CartMovementManager has been spawned.
       Debug.Log("CartMovementManager has been spawned and event invoked.");
   }
}
