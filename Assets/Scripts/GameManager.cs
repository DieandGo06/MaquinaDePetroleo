using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Estados { standBy, tiradaIniciada, iconoSeleccionado, consecuencias }
    public static GameManager instance;
    public Estados estado;

    [HideInInspector] public int contadorTiradas;
    public AnimationCam camaraScript;

    public UnityEvent IniciarTirada;
    public UnityEvent PlayZoomOut;
    public UnityEvent PlayZoomIn;



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
                SumarTirada();
                IniciarTirada.Invoke();
                CambiarEstado(Estados.tiradaIniciada);
            }
        }
        if (estado== Estados.tiradaIniciada)
        {

        }
        if (estado == Estados.iconoSeleccionado)
        {
            Tareas.Nueva(2, PlayZoomOut.Invoke);
            CambiarEstado(Estados.consecuencias);
            Tareas.Nueva(6, PlayZoomIn.Invoke);//Para regresar al loop (DEBERIA IR EN OTRO LUGAR)
            Tareas.Nueva(7, ()=> CambiarEstado(Estados.standBy));//Para regresar al loop (DEBERIA IR EN OTRO LUGAR)
        }
        if (estado == Estados.consecuencias)
        {

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
}
