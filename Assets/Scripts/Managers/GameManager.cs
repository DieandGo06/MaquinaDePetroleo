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
    public class Resultados
    {
        public string nombre;
        public Vector3 combinacion;
        public VideoClip animacion;
        public RawImage lienzoResultado;
        public VideoPlayer playerResultado;
        public AudioClip sonido;
        public bool[] esMaloElIcono = new bool[3];
        public UnityEvent eventoAdicional;
    }


    public enum Estados { standBy, tiradaIniciada, iconoSeleccionado, consecuencias }
    public static GameManager instance;


    [Header("Contadores Generales")]
    public int contadorRondas;
    public int contadorTiradas;
    public int totalTiradas;

    [Header("Control del Loop General")]
    public Estados estado;
    public Resultados resultadoActual;
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
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                //Leve retraso visual para que entre antes el sonido
                SumarTirada();
                PreInicioDeTirada.Invoke();
                CambiarEstado(Estados.tiradaIniciada);
                Tareas.Nueva(0.15f, () => IniciarTirada.Invoke());
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
                if (ultimaTiradaDeRonda)
                {
                    Tareas.Nueva(4, () =>
                    {
                        ejecutarUnaVez = false;
                        CambiarEstado(Estados.consecuencias);
                        PlayZoomOut.Invoke();
                        //SeleccionarFondo();
                    });
                    ejecutarUnaVez = true;
                    return;
                }

                //Si no es la tercera tirada de una ronda...
                Tareas.Nueva(2, () =>
                {
                    ejecutarUnaVez = false;
                    CambiarEstado(Estados.consecuencias);
                    PlayZoomOut.Invoke();
                    //SeleccionarFondo();
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
                        resultadoActual.eventoAdicional.Invoke();
                    });

                    Tareas.Nueva(6, () =>
                    {
                        PlayZoomIn.Invoke();
                        Tareas.Nueva(1.3f, () =>
                        {
                            if (contadorRondas == 8) contadorRondas = 0;
                            ReiniciarRonda();
                            NuevaRonda();
                            CambiarEstado(Estados.standBy);
                            ejecutarUnaVez = false;
                        });
                    });
                    ejecutarUnaVez = true;
                    return;
                }

                //Si no es la tercera tirada de una ronda...
                Tareas.Nueva(4, () =>
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
        //lienzoFinal.gameObject.SetActive(false);
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
        totalTiradas++;
        UIManager.instance.contadorTotalTiradas.text = totalTiradas.ToString("00");
    }

    public void CambiarEstado(Estados estado_)
    {
        estado = estado_;
        ejecutarUnaVez = false;
    }


    void TrucarTiradas()
    {
        resultadoActual = resultados[contadorRondas];
        RodillosManager.instance.rodillos[0].SetIconoSeleccionado(RodillosManager.instance.rodillos[0].BuscarIconoPorID((int)resultadoActual.combinacion.x));
        RodillosManager.instance.rodillos[1].SetIconoSeleccionado(RodillosManager.instance.rodillos[1].BuscarIconoPorID((int)resultadoActual.combinacion.y));
        RodillosManager.instance.rodillos[2].SetIconoSeleccionado(RodillosManager.instance.rodillos[2].BuscarIconoPorID((int)resultadoActual.combinacion.z));

        //Indica a cada rodillo si el icono trucado que tienen es bueno o malo en la combinacion
        for (int i = 0; i < resultados[contadorRondas].esMaloElIcono.Length; i++)
        {
            RodillosManager.instance.rodillos[i].esMaloElIcono = resultados[contadorRondas].esMaloElIcono[i];
        }
    }


    void PlayConsecuencia()
    {
        resultadoActual.lienzoResultado.gameObject.SetActive(true);
        resultadoActual.playerResultado.clip = resultadoActual.animacion;
        resultadoActual.playerResultado.Play();
        AudioManager.instance.resultadosFinales.PlayOneShot(resultadoActual.sonido);
    }

    public string GetNombreResultadoFinal()
    {
        return resultados[contadorRondas].nombre;
    }

    public bool EsMaloElResultadoFinal()
    {
        foreach (bool esMaloElIcono in resultadoActual.esMaloElIcono)
        {
            if (esMaloElIcono) return true;
        }
        //for (int i = 0; i < resultados[contadorRondas].esMaloElIcono.Length; i++)
        //{
        //    if (resultados[contadorRondas].esMaloElIcono[i]) return true;
        //}
        return false;
    }

    public void ApagarVideo(float timer)
    {
        Resultados _resultado = resultadoActual; 
        Tareas.Nueva(timer, () =>
        {
            _resultado.playerResultado.clip = UIManager.instance.vacio;
            _resultado.playerResultado.Stop();
            _resultado.playerResultado.Play();
        });

    }
}
