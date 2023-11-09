using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodillosManager : MonoBehaviour
{
    public static RodillosManager instance;
    public Rodillo[] rodillos; 

    [Header("Triggers para detener rodillos")]
    public GameObject triggerToReboteSuperior;
    public GameObject triggerToReboteInferior;
    public GameObject triggerToDetenerse;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    public void ReiniciarSistemaDeFrenado()
    {
        triggerToReboteSuperior.SetActive(false);
        triggerToReboteInferior.SetActive(false);
        triggerToDetenerse.SetActive(false);
    }
}
