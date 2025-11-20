using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasurePickUp : MonoBehaviour
{
    private GameObject TreasureManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("VRPlayer"))
        {
            // Aquí puedes agregar lógica para sumar puntos o actualizar el inventario del jugador
            TreasureManager = GameObject.Find("TreasureManager");
            TreasureManager.GetComponent<TreasureManager>().PickUpManagement();
            Debug.Log("Tesoro recogido!");

            // Destruir el objeto del tesoro después de recogerlo
            gameObject.SetActive(false);
        }
    }
}
