using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine;

public class Icono : MonoBehaviour
{
    public int iconoID;
    public VideoClip animacion;
    [SerializeField] bool cuentaVueltas;

    //Privadas
    Rigidbody2D rb;
    Rodillo rodillo;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rodillo = GetComponentInParent<Rodillo>();

        //Pasa el script y el rigidBody al rodillo
        rodillo.GetIconoComponent(this, rb);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Limite"))
        {
            transform.position = new Vector3(transform.position.x, rodillo.ultimoIcono.transform.position.y - rodillo.distanciaEntreIconos, transform.position.z);
            rodillo.ultimoIcono = GetComponent<Rigidbody2D>();
        }
        if (collision.CompareTag("InicioRodillo"))
        {
            if (cuentaVueltas) rodillo.contadorVueltas++;
        }

        if (rodillo.contadorVueltas >= rodillo.maximoVueltas)
        {
            if (collision.CompareTag("InicioRodillo") && rodillo.iconoSeleccionado == rb)
            {
                RodillosManager.instance.triggerToReboteSuperior.SetActive(true);
                rodillo.SetPuedeDetenerse(true);
                Debug.Log("puede frenar");
            }

            if (collision.CompareTag("ReboteSuperior") && rodillo.iconoSeleccionado == rb)
            {
                RodillosManager.instance.triggerToReboteInferior.SetActive(true);
                rodillo.currentSpeed = -0.75f;
                Debug.Log("rebote superior");

            }

            if (collision.CompareTag("ReboteInferior") && rodillo.iconoSeleccionado == rb)
            {
                RodillosManager.instance.triggerToDetenerse.SetActive(true);
                rodillo.currentSpeed *= -0.6f;
                Debug.Log("rebote inferior");
            }

            if (collision.CompareTag("Detenerse") && rodillo.iconoSeleccionado == rb)
            {
                if (GameManager.instance.contadorTiradas == rodillo.rodilloID)
                {
                    rodillo.currentSpeed = 0;
                    RodillosManager.instance.ReiniciarSistemaDeFrenado();
                    GameManager.instance.CambiarEstado(GameManager.Estados.iconoSeleccionado);
                    Debug.Log("detuvo");
                }
            }
        }

    }
}


