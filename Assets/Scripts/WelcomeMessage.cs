using UnityEngine;
using UnityEngine.UI;
using System;


public class WelcomeMessage : MonoBehaviour
{
    [SerializeField]
    private float displayTime = 5f; // Tiempo que el mensaje estará visible
    [SerializeField]
    private GameObject Bienvenida; // Mensaje de bienvenida
    [SerializeField]
    private GameObject Background; // Fondo del mensaje


    void Start()
    {
        displayTime -= Time.deltaTime;

    }

    void Update()
    {
        displayTime -= Time.deltaTime;
        // Si el tiempo de visualización es menor o igual a 0, se desactiva el mensaje
        if (displayTime <= 0)
        {
            deactivateMessage();
            Console.WriteLine("Mensaje desactivado");
        }
    }

    // Método para desactivar el mensaje de bienvenida
    private void deactivateMessage()
    {
        Bienvenida.SetActive(false);
        Background.SetActive(false);
    }
}