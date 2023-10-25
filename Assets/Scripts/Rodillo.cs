using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rodillo : MonoBehaviour
{
    [SerializeField] int rodilloID;
    public float maxSpeed;
    public float currentSpeed;
    public float tiempoToInciarFreno;
    public float distanciaEntreIconos;
    public List<Rigidbody2D> iconos = new List<Rigidbody2D>();
    public Rigidbody2D ultimoIcono;

    bool estaFrenando;
    public float contadorVueltas;


    private void Start()
    {
        GameManager.instance.IniciarTirada.AddListener(IniciarTirada);
    }

    private void OnDisable()
    {
        GameManager.instance.IniciarTirada.RemoveListener(IniciarTirada);
    }



    private void FixedUpdate()
    {
        if (GameManager.instance.contadorTiradas == rodilloID)
        {
            if (GameManager.instance.estado == GameManager.Estados.standBy)
            {

            }
            if (GameManager.instance.estado == GameManager.Estados.tiradaIniciada)
            {
                Girar();
                if (estaFrenando) Frenar();
            }
            if (GameManager.instance.estado == GameManager.Estados.iconoSeleccionado)
            {

            }
            if (GameManager.instance.estado == GameManager.Estados.consecuencias)
            {

            }
        }
    }

    void Girar()
    {
        foreach (Rigidbody2D icono in iconos)
        {
            //icono.velocity = Vector3.up * currentSpeed;
            float newPosY = icono.transform.position.y + (currentSpeed * Time.fixedDeltaTime);
            icono.MovePosition(new Vector3(icono.transform.position.x, newPosY, icono.transform.position.z));
        }
    }

    void Frenar()
    {
        if (currentSpeed > 5f)
        {
            currentSpeed -= (Mathf.Log(currentSpeed) * 5) * Time.fixedDeltaTime;
        }
        else
        {
            currentSpeed -= (Time.fixedDeltaTime/3) + (currentSpeed/10) * Time.fixedDeltaTime;
        }

        //currentDeceleration -= (Mathf.Log(currentDeceleration)/2) * Time.fixedDeltaTime;

        if (currentSpeed <= 0)
        {
            currentSpeed = 0;
            estaFrenando = false;
            GameManager.instance.CambiarEstado(GameManager.Estados.iconoSeleccionado);
        }
    }

    void IniciarTirada()
    {
        currentSpeed = maxSpeed;
        Tareas.Nueva(tiempoToInciarFreno, () => estaFrenando = true);
    }
}
