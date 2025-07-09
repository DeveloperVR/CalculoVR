using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContadorPrefabsHijos : MonoBehaviour
{
    [Tooltip("Lista pública que contiene los hijos del objeto")]
    public List<GameObject> children = new List<GameObject>();

    void Awake()
    {
        // Limpiar la lista antes de llenarla
        children.Clear();

        // Recorrer todos los hijos y agregarlos a la lista
        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }

    }
}