using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine;

public class Rodillo : MonoBehaviour
{
    [Header("Propiedades del Rodillo")]
    public int rodilloID;
    public float distanciaEntreIconos;

    [Header("Sistema de Giro")]
    public float currentSpeed;
    public float maxSpeed;
    public float timerToDesacelerar;
    public float contadorVueltas;
    public float maximoVueltas;
    bool estaDesacelerando;
    bool puedeDetenerse;

    [Header("Sistema de Videos")]
    public VideoPlayer videoPlayer;
    public RawImage lienzo;

    [Header("Sistema de Trucadas de Iconos")]
    [SerializeField] string nombreIconoSeleccionado;
    public Rigidbody2D iconoSeleccionado;
    public Rigidbody2D ultimoIcono;
    List<Rigidbody2D> iconosRB;
    List<Icono> iconos;




    private void Awake()
    {
        iconosRB = new List<Rigidbody2D>();
        iconos = new List<Icono>();
    }

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
            { }

            if (GameManager.instance.estado == GameManager.Estados.tiradaIniciada)
            {
                Girar();
                if (estaDesacelerando) Desacelerar();
            }

            if (GameManager.instance.estado == GameManager.Estados.iconoSeleccionado)
            {
                lienzo.gameObject.SetActive(true);
                videoPlayer.Play();
            }

            if (GameManager.instance.estado == GameManager.Estados.consecuencias)
            { }
        }
    }




    void Girar()
    {
        foreach (Rigidbody2D icono in iconosRB)
        {
            float newPosY = icono.transform.position.y + (currentSpeed * Time.fixedDeltaTime);
            icono.MovePosition(new Vector3(icono.transform.position.x, newPosY, icono.transform.position.z));
        }
    }

    void Desacelerar()
    {
        //Frena tras superar el timer para desacelerar
        if (!puedeDetenerse)
        {
            if (currentSpeed > 3.5f) currentSpeed -= (Mathf.Log(currentSpeed) * 8.5f) * Time.fixedDeltaTime;
            //Desacelerar cuando aun no ha dado el primer rebote
            else currentSpeed -= (currentSpeed / 4.5f) * Time.fixedDeltaTime;
        }
        //Frena tras dar el primer rebote para detenerse
        else
        {
            if (currentSpeed != 0)
            {
                //Desacelerar cuando hace el PRIMER rebote
                if (currentSpeed > 0) currentSpeed -= (Time.fixedDeltaTime / 5f);
                //Desacelerar cuando hace el SEGUNDO rebote
                else currentSpeed += (Time.fixedDeltaTime / 5f);
            }
        }
    }



    

    //Lo ejecuta el GameManager tras las tres tiradas
    public void ReiniciarRodillo()
    {
        estaDesacelerando = false;
        puedeDetenerse = false;
        lienzo.gameObject.SetActive(false);
        iconoSeleccionado = null;
        contadorVueltas = 0;
    }

    void IniciarTirada()
    {
        if (GameManager.instance.contadorTiradas == rodilloID)
        {
            currentSpeed = maxSpeed;
            Tareas.Nueva(timerToDesacelerar, () => estaDesacelerando = true);

            if (iconoSeleccionado == null)
            {
                //SeleccionarIconoAleatorio();
                videoPlayer.clip = iconoSeleccionado.gameObject.GetComponent<Icono>().animacion;
            }
            else
            {
                videoPlayer.clip = iconoSeleccionado.gameObject.GetComponent<Icono>().animacion;
            }
        }
    }



    public Rigidbody2D BuscarIconoPorID(int id)
    {
        foreach (Icono icono in iconos)
        {
            if (icono.iconoID == id) return icono.GetComponent<Rigidbody2D>();
        }
        return null;
    }

    public void GetIconoComponent(Icono script, Rigidbody2D rigidbody)
    {
        iconos.Add(script);
        iconosRB.Add(rigidbody);
    }

    public void SetPuedeDetenerse(bool value)
    {
        puedeDetenerse = value;
    }

    public void SetIconoSeleccionado(Rigidbody2D icono)
    {
        nombreIconoSeleccionado = icono.name;
        iconoSeleccionado = icono;
    }

    void SeleccionarIconoAleatorio()
    {
        int randomNum = Random.Range(0, iconosRB.Count);
        iconoSeleccionado = iconosRB[randomNum];
    }
}
