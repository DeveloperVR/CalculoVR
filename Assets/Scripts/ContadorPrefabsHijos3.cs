using System.Collections.Generic;
using UnityEngine;

public class ContadorPrefabsHijos3 : MonoBehaviour
{
    [Tooltip("Lista pública que contiene los hijos del objeto")]
    public List<GameObject> children3 = new List<GameObject>();

    void Awake()
    {
        // Limpiar la lista antes de llenarla
        children3.Clear();

        // Recorrer todos los hijos y agregarlos a la lista
        foreach (Transform child in transform)
        {
            children3.Add(child.gameObject);
        }

    }
}