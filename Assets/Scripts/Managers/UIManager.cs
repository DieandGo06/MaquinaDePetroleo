using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Space(10)]
    [Header("Animaciones Finales")]
    public VideoPlayer videoFinal;
    public RawImage lienzoVideoFinal;

    [Space(10)]
    [Header("Fondos")]
    public SpriteRenderer fondo;
    public Sprite fondoBueno;
    public Sprite fondoIntermdio;
    public Sprite fondoMalo;

    private void Awake()
    {
        instance = this;
    }
}
