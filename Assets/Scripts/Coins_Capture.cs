using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Coins_Capture : MonoBehaviour
{
    public int cantidadMonedas;
    public int totalMonedas = 11;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Moneda"))
        {
            Destroy(other.gameObject);
            cantidadMonedas++;
            Debug.Log("¡Has recogido una moneda!");
            int monedasRestantes = totalMonedas - cantidadMonedas;
            Debug.Log("Te quedan " + monedasRestantes + " monedas por recoger.");

            if (cantidadMonedas == totalMonedas)
            {
                Debug.Log("¡Has recolectado 11 monedas! Saliendo del juego...");
                SceneManager.LoadScene("EndGame");
            }
        }

    }
}


