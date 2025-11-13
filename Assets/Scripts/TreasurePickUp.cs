using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasurePickUp : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Aquí puedes agregar lógica para sumar puntos o actualizar el inventario del jugador
            Debug.Log("Tesoro recogido!");

            // Destruir el objeto del tesoro después de recogerlo
            Destroy(gameObject);
        }
    }
}
