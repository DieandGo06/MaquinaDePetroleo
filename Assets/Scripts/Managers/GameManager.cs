using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Serializable]
    private class Resultados
    {
        public string nombre;
        public VideoClip animacion;
        public Vector3 combinacion;
        public bool[] esMalo = new bool[3];
        public UnityEvent eventoAdicional;
    }


    public enum Estados { standBy, tiradaIniciada, iconoSeleccionado, consecuencias }
    public static GameManager instance;


    [Header("Contadores Generales")]
    public int contadorRondas;
    public int contadorTiradas;

    [Header("Control del Loop General")]
    public Estados estado;
    public Vector3 combinacionActual;
    [SerializeField] Resultados[] resultados;
    bool ultimaTiradaDeRonda;
    


    [Space(10)]
    [Header("Consecuencias Generales")]
    [SerializeField] RawImage lienzoFinal;
    [SerializeField] VideoPlayer videoPlayerFinal;
    [SerializeField] VideoClip explosiones;
    [SerializeField] int contadorManchas;
    [SerializeField] GameObject[] manchas;

    //Eventos
    [HideInInspector] public UnityEvent PreInicioDeTirada;
    [HideInInspector] public UnityEvent IniciarTirada;
    [HideInInspector] public UnityEvent PlayZoomOut;
    [HideInInspector] public UnityEvent PlayZoomIn;

    //privadas
    private bool ejecutarUnaVez;





    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        TrucarTiradas();
    }

    void Update()
    {
        if (estado == Estados.standBy)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Leve retraso visual para que entre antes el sonido
                SumarTirada();
                PreInicioDeTirada.Invoke();
                CambiarEstado(Estados.tiradaIniciada);
                Tareas.Nueva(0.2f, () => IniciarTirada.Invoke());
                if (contadorTiradas == 3) ultimaTiradaDeRonda = true;
            }
        }


        if (estado == Estados.tiradaIniciada)
        {
            //Giran los rodillos
        }


        if (estado == Estados.iconoSeleccionado)
        {
            if (!ejecutarUnaVez)
            {
                Tareas.Nueva(2, () =>
                {
                    ejecutarUnaVez = false;
                    CambiarEstado(Estados.consecuencias);
                    PlayZoomOut.Invoke();
                    SeleccionarFondo();
                });
                ejecutarUnaVez = true;
            }

        }


        if (estado == Estados.consecuencias)
        {
            if (!ejecutarUnaVez)
            {
                if (ultimaTiradaDeRonda)
                {
                    Tareas.Nueva(2, () =>
                    {
                        PlayConsecuencia();
                    });

                    Tareas.Nueva(6, () =>
                    {
                        PlayZoomIn.Invoke();
                        ReiniciarRonda();
                        NuevaRonda();
                        Tareas.Nueva(1, () =>
                        {
                            CambiarEstado(Estados.standBy);
                            ejecutarUnaVez = false;
                        });
                    });
                    ejecutarUnaVez = true;
                    return;
                }

                //Si no es la tercera tirada de una ronda...
                Tareas.Nueva(3, () =>
                {
                    PlayZoomIn.Invoke();
                    Tareas.Nueva(1, () =>
                    {
                        CambiarEstado(Estados.standBy);
                        ejecutarUnaVez = false;
                    });
                });
                ejecutarUnaVez = true;
            }
        }
    }




    void ReiniciarRonda()
    {
        //Reinicio
        contadorTiradas = 0;
        ultimaTiradaDeRonda = false;
        lienzoFinal.gameObject.SetActive(false);
        UIManager.instance.fondo.sprite = UIManager.instance.fondoBueno;
        foreach (Rodillo rodillo in RodillosManager.instance.rodillos) rodillo.ReiniciarRodillo();
    }

    void NuevaRonda()
    {
        contadorRondas++;
        TrucarTiradas();
    }


    public void SumarTirada()
    {
        contadorTiradas++;
    }

    public void CambiarEstado(Estados estado_)
    {
        estado = estado_;
        ejecutarUnaVez = false;
    }


    void TrucarTiradas()
    {
        combinacionActual = resultados[contadorRondas].combinacion;
        RodillosManager.instance.rodillos[0].SetIconoSeleccionado(RodillosManager.instance.rodillos[0].BuscarIconoPorID((int)combinacionActual.x));
        RodillosManager.instance.rodillos[1].SetIconoSeleccionado(RodillosManager.instance.rodillos[1].BuscarIconoPorID((int)combinacionActual.y));
        RodillosManager.instance.rodillos[2].SetIconoSeleccionado(RodillosManager.instance.rodillos[2].BuscarIconoPorID((int)combinacionActual.z));
    }

    void PlayConsecuencia()
    {
        UIManager.instance.videoFinal.clip = resultados[contadorRondas].animacion;
        UIManager.instance.lienzoVideoFinal.gameObject.SetActive(true);
        UIManager.instance.videoFinal.Play();
    }


    void SeleccionarFondo()
    {
        if (contadorTiradas == 1)
        {
            UIManager.instance.fondo.sprite = UIManager.instance.fondoBueno;
        }
        if (contadorTiradas == 2)
        {
            //Primer error
            if (combinacionActual.x == 1 && combinacionActual.y == 2)
            {
                UIManager.instance.fondo.sprite = UIManager.instance.fondoIntermdio;
            }
            if (combinacionActual.x == 3 && combinacionActual.y == 1)
            {
                UIManager.instance.fondo.sprite = UIManager.instance.fondoIntermdio;
            }
        }
        if (contadorTiradas == 3)
        {
            //Segundo Error
            if (combinacionActual.x == 1 && combinacionActual.y == 2 && combinacionActual.z == 2)
            {
                UIManager.instance.fondo.sprite = UIManager.instance.fondoMalo;
            }
            if (combinacionActual.x == 3 && combinacionActual.y == 1 && combinacionActual.z == 2)
            {
                UIManager.instance.fondo.sprite = UIManager.instance.fondoMalo;
            }
        }
    }

}
