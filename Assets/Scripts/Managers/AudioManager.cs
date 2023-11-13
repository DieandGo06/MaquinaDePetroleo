using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] AudioMixer audioMixer;

    [Header("AurdioSource")]
    public AudioSource ambiente;
    public AudioSource maquina;
    public AudioSource efectosMaquina;
    public AudioSource efectosGenerales;
    public AudioSource resultadosFinales;



    [Header("Maquina")]
    [SerializeField] AudioMixerGroup maquinaGroup;
    [SerializeField] AudioClip maquinaAudio;
    public AudioClip buenTiro;
    public AudioClip malTiro;
    public AudioClip rebote;
    public AudioClip alarma;


    float volumenAmbiente = 0.6f;
    const string pitchBender_maquina = "MaquinaPitch";


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GameManager.instance.PreInicioDeTirada.AddListener(PlayAudioRodillo);
    }

    void Update()
    {
        if (volumenAmbiente <= 0.25f) volumenAmbiente = 0.25f;
        else if (volumenAmbiente > 0.6f) volumenAmbiente = 0.6f;


        if (GameManager.instance.estado == GameManager.Estados.standBy)
        {
            volumenAmbiente += Time.deltaTime/3;
            ambiente.volume = volumenAmbiente;
        }
        if (GameManager.instance.estado == GameManager.Estados.tiradaIniciada)
        {
            volumenAmbiente -= Time.deltaTime/3;
            ambiente.volume = volumenAmbiente;

            RealantizarMaquina();
        }
        if (GameManager.instance.estado == GameManager.Estados.iconoSeleccionado)
        {
            //RealantizarMaquina();
            maquina.Stop();
            if (GameManager.instance.contadorRondas == 8 && GameManager.instance.contadorTiradas == 3)
            {
                Tareas.NuevaConCooldown(0.8f, 20f, () => PlaySoundEffect(alarma),859426); 
            }
        }

    }

    void RealantizarMaquina()
    {
        //maquina.pitch = speed+0.5f;
        //audioMixer.SetFloat(pitchBender_maquina, 1f / speed+0.5f);
        //Para la entrega
        maquina.pitch = 0.60f;
        efectosMaquina.pitch = 0.60f;
        audioMixer.SetFloat(pitchBender_maquina, 1f / 0.60f);
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

    public void PlaySoundEffect(AudioClip sound)
    {
        efectosGenerales.PlayOneShot(sound);
    }
}
