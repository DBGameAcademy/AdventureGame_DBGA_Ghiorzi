using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    [Header("Shake Settings")]
    [SerializeField]
    private float shakeDuration;
    [SerializeField]
    private float shakeAmplitude;
    [SerializeField]
    private float shakeFrequency;

    private float _shakeElapsedTime;
    private CinemachineBasicMultiChannelPerlin _virtualCameraNoise;

    public void Shake()
    {
        _shakeElapsedTime = shakeDuration;
    }

    private void Awake()
    {
        if(virtualCamera != null)
            _virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if(virtualCamera == null || _virtualCameraNoise == null)
            return;

        if(_shakeElapsedTime > 0)
        {
            _virtualCameraNoise.m_AmplitudeGain = shakeAmplitude;
            _virtualCameraNoise.m_FrequencyGain = shakeFrequency;

            _shakeElapsedTime -= Time.deltaTime;
        }
        else
        {
            // Finish - Reset
            _virtualCameraNoise.m_AmplitudeGain = 0f;
            _shakeElapsedTime = 0f;
        }
    }
}
