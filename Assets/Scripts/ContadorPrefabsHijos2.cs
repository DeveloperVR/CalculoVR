using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContadorPrefabsHijos2 : MonoBehaviour
{
    [Tooltip("Lista pública que contiene los hijos del objeto")]
    public List<GameObject> children2 = new List<GameObject>();

    void Awake()
    {
        // Limpiar la lista antes de llenarla
        children2.Clear();

        // Recorrer todos los hijos y agregarlos a la lista
        foreach (Transform child in transform)
        {
            children2.Add(child.gameObject);
        }

        Debug.Log($"Número de hijos: {children2.Count}");
    }
}