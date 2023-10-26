using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icono : MonoBehaviour
{
    [SerializeField] Animation good, bad;
    public bool cuentaVueltas;
    Rigidbody2D rb;
    Rodillo rodilloPadre;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rodilloPadre = GetComponentInParent<Rodillo>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Limite"))
        {
            transform.position = new Vector3(transform.position.x, rodilloPadre.ultimoIcono.transform.position.y - rodilloPadre.distanciaEntreIconos, transform.position.z);
            rodilloPadre.ultimoIcono = GetComponent<Rigidbody2D>();

            if (cuentaVueltas) rodilloPadre.contadorVueltas++;
        }

        if (rodilloPadre.contadorVueltas >= rodilloPadre.maximoVueltas)
        {
            if (collision.CompareTag("InicioRodillo") && rodilloPadre.iconoSeleccionado == rb)
            {
                rodilloPadre.triggerReboteSuperior.SetActive(true);
                rodilloPadre.puedeDetenerse = true;
                Debug.Log("puede frenar");
            }

            if (collision.CompareTag("ReboteSuperior") && rodilloPadre.iconoSeleccionado == rb)
            {
                rodilloPadre.triggerReboteInferior.SetActive(true);
                rodilloPadre.currentSpeed = -0.9f;
                Debug.Log("rebote superior");

            }

            if (collision.CompareTag("ReboteInferior") && rodilloPadre.iconoSeleccionado == rb)
            {
                rodilloPadre.triggerDetenerse.SetActive(true);
                rodilloPadre.currentSpeed *= -1f;
                Debug.Log("rebote inferior");
            }

            if (collision.CompareTag("Detenerse") && rodilloPadre.iconoSeleccionado == rb)
            {
                rodilloPadre.currentSpeed = 0;
            }
        }
        
    }
}


