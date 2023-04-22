using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Drawing.Printing;
using System;

public class CameraControl : MonoBehaviour
{
    private CinemachineConfiner2D confiner2D;
    public CinemachineImpulseSource impulseSource;
    public VoidEVentSO cameraShakeEvent;
    private void Awake()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }
    private void OnEnable()
    {
        cameraShakeEvent.OnEventRaised += OnCamerShakeEvent;
    }

    private void OnDisable()
    {
        cameraShakeEvent.OnEventRaised -= OnCamerShakeEvent;
    }
    private void OnCamerShakeEvent()
    {
        impulseSource.GenerateImpulse();
    }

    //TODO:场景切换后更改
    private void Start()
    {
        GetNewCameraBounds();
    }
    private void GetNewCameraBounds()
    {
        var obj = GameObject.FindGameObjectWithTag("Bounds");
        if (obj == null) 
        {
            Debug.Log("fuck");
            return;
        }
        confiner2D.m_BoundingShape2D = obj.GetComponent<Collider2D>();
        confiner2D.InvalidateCache();
    }
}
