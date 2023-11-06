using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Estados { standBy, tiradaIniciada, iconoSeleccionado, consecuencias }
    public static GameManager instance;

    public AnimationCam camaraScript;

    [Header("Control del Loop General")]
    public Estados estado;
    public Vector3 combinacionIconos;
    [HideInInspector] public int contadorTiradas;
    [SerializeField] bool llegoFinal;
    public Rodillo[] rodillos;

    [Space(10)]
    [Header("Consecuencias Generales")]
    [SerializeField] RawImage lienzoFinal;
    [SerializeField] VideoPlayer videoPlayerFinal;
    [SerializeField] VideoClip explosiones;
    [SerializeField] int contadorManchas;
    [SerializeField] GameObject[] manchas;

    [Space(10)]
    [Header("Fondos")]
    [SerializeField] SpriteRenderer fondo;
    [SerializeField] Sprite fondoBueno;
    [SerializeField] Sprite fondoIntermdio;
    [SerializeField] Sprite fondoMalo;


    [HideInInspector] public UnityEvent PreInicioDeTirada;
    [HideInInspector] public UnityEvent IniciarTirada;
    [HideInInspector] public UnityEvent PlayZoomOut;
    [HideInInspector] public UnityEvent PlayZoomIn;



    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (estado == Estados.standBy)
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (contadorTiradas == 3)
                {
                    llegoFinal = true;
                    contadorTiradas = 0;
                    foreach (Rodillo rodillo in rodillos)
                    {
                        fondo.sprite = fondoBueno;
                        rodillo.ReiniciarRodillo();
                        lienzoFinal.gameObject.SetActive(false);
                    }
                }

                //Leve retraso visual para que entre antes el sonido
                SumarTirada();
                CambiarEstado(Estados.tiradaIniciada);
                PreInicioDeTirada.Invoke();
                Tareas.Nueva(0.2f, () => IniciarTirada.Invoke());

            }
        }


        if (estado == Estados.tiradaIniciada)
        {

        }


        if (estado == Estados.iconoSeleccionado)
        {
            CambiarEstado(Estados.consecuencias);
            Tareas.Nueva(1, PlayZoomOut.Invoke);
            Tareas.Nueva(5, PlayZoomIn.Invoke);//Para regresar al loop (DEBERIA IR EN OTRO LUGAR)
            Tareas.Nueva(6, () => CambiarEstado(Estados.standBy));//Para regresar al loop (DEBERIA IR EN OTRO LUGAR)   
            SeleccionarFondo();
        }


        if (estado == Estados.consecuencias)
        {
            if (contadorTiradas == 3 && !llegoFinal)
            {
                llegoFinal = true;
                SeleccionarVideoFinal();

                if(esFinalMalo())
                {
                    if (contadorManchas < manchas.Length)
                    {
                        contadorManchas++;
                        manchas[contadorManchas].SetActive(true);
                    }
                }
                
                Tareas.Nueva(1.5f, () =>
                {
                    lienzoFinal.gameObject.SetActive(true);
                    videoPlayerFinal.Play();
                });
            }
        }
    }

    public void SumarTirada()
    {
        contadorTiradas++;
    }

    public void CambiarEstado(Estados estado_)
    {
        estado = estado_;
    }

    public void GuardarID_iconosSeleccionado(int iconoID)
    {
        if (contadorTiradas == 1) combinacionIconos.x = iconoID;
        else if (contadorTiradas == 2) combinacionIconos.y = iconoID;
        else if (contadorTiradas == 3) combinacionIconos.z = iconoID;
    }

    void SeleccionarVideoFinal()
    {
        //Explosion
        if (combinacionIconos.x == 1 && combinacionIconos.y == 2 && combinacionIconos.z == 2)
        {
            videoPlayerFinal.clip = explosiones;
        }
        if (combinacionIconos.x == 3 && combinacionIconos.y == 1 && combinacionIconos.z == 2)
        {
            videoPlayerFinal.clip = explosiones;
        }
    }

    bool esFinalMalo()
    {
        //Explosion
        if (combinacionIconos.x == 1 && combinacionIconos.y == 2 && combinacionIconos.z == 2) return true;
        if (combinacionIconos.x == 3 && combinacionIconos.y == 1 && combinacionIconos.z == 2) return true;
        else return false;
    }

    void SeleccionarFondo()
    {
        if (contadorTiradas == 1)
        {
            fondo.sprite = fondoBueno;
        }
        if (contadorTiradas == 2)
        {
            //Primer error
            if (combinacionIconos.x == 1 && combinacionIconos.y == 2)
            {
                fondo.sprite = fondoIntermdio;
            }
            if (combinacionIconos.x == 3 && combinacionIconos.y == 1)
            {
                fondo.sprite = fondoIntermdio;
            }
        }
        if (contadorTiradas == 3)
        {
            //Segundo Error
            if (combinacionIconos.x == 1 && combinacionIconos.y == 2 && combinacionIconos.z == 2)
            {
                fondo.sprite = fondoMalo;
            }
            if (combinacionIconos.x == 3 && combinacionIconos.y == 1 && combinacionIconos.z == 2)
            {
                fondo.sprite = fondoMalo;
            }
        }

    }
}
