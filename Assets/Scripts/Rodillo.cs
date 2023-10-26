using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rodillo : MonoBehaviour
{
    [Header("Propeidades")]
    [SerializeField] int rodilloID;
    public float maxSpeed;
    public float currentSpeed;
    public float tiempoToInciarFreno;
    public float distanciaEntreIconos;
    public float contadorVueltas;
    public float maximoVueltas;


    [Header("Sistema de iconos")]
    public Rigidbody2D iconoSeleccionado;
    public Rigidbody2D ultimoIcono;
    public GameObject triggerReboteSuperior;
    public GameObject triggerReboteInferior;
    public GameObject triggerDetenerse;
    public List<Rigidbody2D> iconos = new List<Rigidbody2D>();


    public bool estaFrenando, puedeDetenerse;
    float tiempo;


    private void Start()
    {
        GameManager.instance.IniciarTirada.AddListener(IniciarTirada);
        float aceleration = ((2 * (8 - 0)) / 3 * Mathf.Exp(2)) - ((2 * 3) / 3);
        Debug.Log(aceleration);
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
                if (estaFrenando && !puedeDetenerse) Desacelerar();
                if (puedeDetenerse) Frenar();
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
            float newPosY = icono.transform.position.y + (currentSpeed * Time.fixedDeltaTime);
            icono.MovePosition(new Vector3(icono.transform.position.x, newPosY, icono.transform.position.z));
        }
    }

    void Desacelerar()
    {
        if (currentSpeed > 3f)
        {
            currentSpeed -= (Mathf.Log(currentSpeed) * 6) * Time.fixedDeltaTime;
        }
        else currentSpeed -= (currentSpeed /10) * Time.fixedDeltaTime;

    }

    void Frenar()
    {
        if (puedeDetenerse)
        {
            if (currentSpeed != 0)
            {
                if (currentSpeed > 0) currentSpeed -= (Time.fixedDeltaTime / 2f);
                else currentSpeed += (Time.fixedDeltaTime / 2f);
            }
            else if (currentSpeed == 0)
            {
                ReiniciarRodillo();
                GameManager.instance.CambiarEstado(GameManager.Estados.iconoSeleccionado);
            }
        }
    }

    void ReiniciarRodillo()
    {
        estaFrenando = false;
        puedeDetenerse = false;
        triggerReboteSuperior.SetActive(false);
        triggerReboteInferior.SetActive(false);
        triggerDetenerse.SetActive(false);
        iconoSeleccionado = null;
        contadorVueltas = 0;
    }

    void IniciarTirada()
    {
        if (GameManager.instance.contadorTiradas == rodilloID)
        {
            currentSpeed = maxSpeed;
            Tareas.Nueva(tiempoToInciarFreno, () => estaFrenando = true);

            if(iconoSeleccionado == null)
            {
                SeleccionarIconoAleatorio();
            }
            else
            {
                TrucarTirada();
            }
        }
    }

    void TrucarTirada()
    {
        //Falta ver con que icono se trucara
    }

    void SeleccionarIconoAleatorio()
    {
        int randomNum = Random.Range(0, iconos.Count);
        iconoSeleccionado = iconos[randomNum];
    }
}
