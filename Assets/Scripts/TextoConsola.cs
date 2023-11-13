using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class TextoConsola : MonoBehaviour
{
    TextMeshProUGUI texto;
    float tiempoTranscurrido;
    float posX, posY;
    float posY0 = -1.69f;
    bool ejecitarUnaVez;


    // Start is called before the first frame update
    void Start()
    {
        texto = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.instance.estado == GameManager.Estados.standBy)
        {
            if (!ejecitarUnaVez)
            {
                ejecitarUnaVez = true;
                texto.rectTransform.position = new Vector3(-4.9f, texto.rectTransform.position.y, texto.rectTransform.position.z);
            }
            texto.text = "Tira la Palanca";
            texto.color = Color.white;
            MoverHorizontalmente();
            //Parpadear();
        }

        if (GameManager.instance.estado == GameManager.Estados.tiradaIniciada)
        {
            texto.text = "";
            texto.rectTransform.position = new Vector3(0, posY0 + 0.1f, texto.rectTransform.position.z);
            ejecitarUnaVez = false;//Permite reiniciar el texto en el standBy
            //MoverVerticalmente();
        }

        if (GameManager.instance.estado == GameManager.Estados.iconoSeleccionado)
        {
            if (GameManager.instance.contadorTiradas == 3)
            {
                Tareas.NuevaConCooldown(0, 15, () => texto.rectTransform.position = new Vector3(0, posY0, texto.rectTransform.position.z), 264854);
                Tareas.NuevaConCooldown(1f, 15f, AnimacionConsecuenciaFinal, 4855632);
            }
            else texto.rectTransform.position = new Vector3(0, posY0, texto.rectTransform.position.z);
            tiempoTranscurrido = 0;
        }

        if (GameManager.instance.estado == GameManager.Estados.consecuencias)
        {
            
        }

    }

    void Parpadear()
    {
        if (tiempoTranscurrido < 0.05f)
        {
            texto.enabled = true;
        }
        if (tiempoTranscurrido > 0.1f)
        {
            texto.enabled = false;
        }
        if (tiempoTranscurrido > 0.15f)
        {
            tiempoTranscurrido = 0;
        }

        tiempoTranscurrido += Time.deltaTime;
    }

    void MoverHorizontalmente()
    {
        posX += Time.deltaTime * 2;
        texto.rectTransform.position = new Vector3(posX, posY0, texto.rectTransform.position.z);
        if (texto.rectTransform.position.x > 4.7f)
        {
            texto.rectTransform.position = new Vector3(-4.7f, texto.rectTransform.position.y, texto.rectTransform.position.z);
            posX = -4.7f;
        }
    }

    void MoverVerticalmente()
    {
        posY += Time.deltaTime * 6;
        texto.rectTransform.position = new Vector3(0, posY, texto.rectTransform.position.z);
        if (texto.rectTransform.position.y > -1.3f)
        {
            texto.rectTransform.position = new Vector3(texto.rectTransform.position.x, posY, texto.rectTransform.position.z);
            posY = -2.5f;
        }
    }

    void AnimacionConsecuenciaFinal()
    {
        texto.rectTransform.position = new Vector3(0, posY0, texto.rectTransform.position.z);
        Tareas.NuevaConDuracion(0f, 0.28f, 20f, MoverVerticalmente, 548462);
        Tareas.NuevaConCooldown(0.30f, 20f, () =>
        {
            Color nuevoColor;
            texto.text = GameManager.instance.GetNombreResultadoFinal();
            texto.rectTransform.position = new Vector3(0, posY0, texto.rectTransform.position.z);
            if (GameManager.instance.EsMaloElResultadoFinal()) nuevoColor = UIManager.instance.colorMalIcono;
            else nuevoColor = UIManager.instance.colorBuenIcono;
            texto.color = nuevoColor;

            Tareas.Nueva(0.2f, () => { texto.enabled = false; });
            Tareas.Nueva(0.4f, () => { texto.enabled = true; });
            Tareas.Nueva(0.6f, () => { texto.enabled = false; });
            Tareas.Nueva(0.8f, () => { texto.enabled = true; });
            Tareas.Nueva(1.0f, () => { texto.enabled = false; });
            Tareas.Nueva(1.2f, () => { texto.enabled = true; });
        }, 4862351);
    }

}
