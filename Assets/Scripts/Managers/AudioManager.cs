using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    [Header("AurdioSource")]
    [SerializeField] AudioSource maquina;

    [Header("Maquina")]
    [SerializeField] AudioMixerGroup maquinaGroup;
    [SerializeField] AudioClip maquinaAudio;
    [SerializeField] Rodillo rodilloIzq;
    [SerializeField] Rodillo rodilloCentro;
    [SerializeField] Rodillo rodilloDer;


    const string pitchBender_maquina = "MaquinaPitch";


    void Start()
    {
        GameManager.instance.PreInicioDeTirada.AddListener(PlayAudioRodillo);

    }

    void Update()
    {
        if (GameManager.instance.estado == GameManager.Estados.tiradaIniciada)
        {
            RealantizarMaquina();
        }
        if (GameManager.instance.estado == GameManager.Estados.iconoSeleccionado)
        {
            maquina.Stop();
        }

    }

    void RealantizarMaquina()
    {
        //maquina.pitch = speed+0.5f;
        //audioMixer.SetFloat(pitchBender_maquina, 1f / speed+0.5f);
        //Para la entrega
        maquina.pitch = 0.55f;
        audioMixer.SetFloat(pitchBender_maquina, 1f / 0.55f);
    }

    public float Remap(float value, float maximoOriginal, float maximoFinal, float minimoOriginal, float minimoFinal)
    {
        return (value - maximoOriginal) / (maximoFinal - maximoOriginal) * (minimoFinal - minimoOriginal) + minimoOriginal;
    }

    public float NormalizeFloat(float rawValue, float min, float max)
    {
        return (rawValue - min) / (max - min);
    }

    void PlayAudioRodillo()
    {
        maquina.PlayOneShot(maquinaAudio);
    }
}
