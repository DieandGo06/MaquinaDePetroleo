using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icono : MonoBehaviour
{
    [SerializeField] Animation good, bad;
    public bool cuentaVueltas;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Limite"))
        {
            Rodillo rodillo = GetComponentInParent<Rodillo>();
            transform.position = new Vector3(transform.position.x, rodillo.ultimoIcono.transform.position.y - rodillo.distanciaEntreIconos, transform.position.z);
            rodillo.ultimoIcono = GetComponent<Rigidbody2D>();

            if (cuentaVueltas) rodillo.contadorVueltas++;
        }
    }
}


