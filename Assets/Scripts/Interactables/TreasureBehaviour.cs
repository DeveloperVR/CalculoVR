using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureBehaviour : MonoBehaviour
{
    GameObject treasure;
    private bool state;
    void Start()
    {
        treasure = gameObject;
    }

    public void ChangeState(bool newState)
    {
        state = newState;
    }

    public void PickUp()
    {
        Debug.Log("Treasure picked up!");
        treasure.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickUp();
        } 
    }

}
