using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public VideoClip vacio;

    [Space(10)]
    [Header("Manchas")]
    public GameObject manchas1;
    public GameObject manchas2;

    [Space(10)]
    [Header("Consola")]
    public SpriteRenderer consola;
    public Sprite consolaQuemada;
    public TextMeshProUGUI nombreDeIconos;
    public TextMeshProUGUI contadorTotalTiradas;
    public SpriteRenderer fondoTexto;
    public RectTransform barraDePetroleo;
    public Color colorBuenIcono;
    public Color colorMalIcono;
    public Color colorFondoTextoOriginal;
    public Color azulConsola;
    public Color blancoRodillo;


    float tiempoTranscurrido;



    private void Awake()
    {
        instance = this;
    }


    void Update()
    {
        if (GameManager.instance.estado == GameManager.Estados.standBy)
        {

        }

        if (GameManager.instance.estado == GameManager.Estados.tiradaIniciada)
        {
            Parpadear();
        }

        if (GameManager.instance.estado == GameManager.Estados.iconoSeleccionado)
        {
            fondoTexto.color = colorFondoTextoOriginal;
        }

        if (GameManager.instance.estado == GameManager.Estados.consecuencias)
        {
            BajarBarraDePetroleo();
        }
    }

    void Parpadear()
    {
        if (tiempoTranscurrido < 0.07f)
        {
            fondoTexto.color = blancoRodillo;
        }
        if (tiempoTranscurrido > 0.14f)
        {
            //fondoTexto.color = colorBuenIcono;
            fondoTexto.color = azulConsola;
        }
        if (tiempoTranscurrido > 0.21f)
        {
            fondoTexto.color = Color.grey;
            //fondoTexto.color = colorMalIcono;
        }
        if (tiempoTranscurrido > 0.28f)
        {
            //tiempoTranscurrido = 0;
            fondoTexto.color = colorFondoTextoOriginal;
        }
        if (tiempoTranscurrido > 0.35f)
        {
            tiempoTranscurrido = 0;
        }

        tiempoTranscurrido += Time.deltaTime;
    }

    public void MantenerManchas(int idManchas)
    {
        if (idManchas == 1)
        {
            Tareas.Nueva(5, () => manchas1.SetActive(true));
        }
        if (idManchas == 2)
        {
            Tareas.Nueva(5, () => manchas2.SetActive(true));
        }
    }

    void BajarBarraDePetroleo()
    {
        Tareas.NuevaConDuracion(0.8f, 0.2f, 10f, () =>
        {
            Vector3 newPos = new Vector3(barraDePetroleo.position.x, barraDePetroleo.position.y - (Time.deltaTime / 2), barraDePetroleo.position.z);
            barraDePetroleo.position = newPos;
        }, 84526);
    }

    public void BajarMuchaBarraDePetroleo()
    {
        Tareas.NuevaConDuracion(0.8f, 1f, 10f, () =>
        {
            Vector3 newPos = new Vector3(barraDePetroleo.position.x, barraDePetroleo.position.y - Time.deltaTime, barraDePetroleo.position.z);
            barraDePetroleo.position = newPos;
        }, 759526);
    }

    public void RecargarBarraDePetroleo()
    {
        Tareas.NuevaConDuracion(0.8f, 4f, 10f, () =>
        {
            float newPosY = barraDePetroleo.position.y + (Time.deltaTime * 2);
            if (newPosY > 0.05f) newPosY = 0.05f;
            Vector3 newPos = new Vector3(barraDePetroleo.position.x, newPosY, barraDePetroleo.position.z);
            barraDePetroleo.position = newPos;
        }, 269526);
    }

    public void CambiarConsolaTrasExlosion(float timer)
    {
        Tareas.Nueva(timer, () =>
        {
            consola.sprite = consolaQuemada;
        });
    }
}
