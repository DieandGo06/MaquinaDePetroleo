using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables; //Playable Director
using UnityEngine;
using Cinemachine;
using UnityEngine.Timeline;

public class AnimationCam : MonoBehaviour
{
    PlayableDirector director;
    public TimelineAsset zoomIn, zoomOut;

    void Start()
    {
        director = GetComponent<PlayableDirector>();
        GameManager.instance.camaraScript = this;

        GameManager.instance.PlayZoomIn.AddListener(ZoomIn);
        GameManager.instance.PlayZoomOut.AddListener(ZoomOut);
    }

    public void ZoomIn()
    {
        director.Play(zoomIn);
    }

    public void ZoomOut()
    {
        director.Play(zoomOut);

    }
}
