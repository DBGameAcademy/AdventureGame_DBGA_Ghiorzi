using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinematicController : Singleton<CinematicController>
{
    public bool IsPlaying { get; private set; }

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;
    [SerializeField]
    private float zoomInValue = 50.0f;
    [SerializeField]
    private float zoomSpeed = 20.0f;

    private float _defaultZoom;
    private bool _isZoomIn = false;
    private bool _isZoomReset = false;

    public void StartCinematic()
    {
        animator.ResetTrigger("CloseCinematic");
        animator.SetTrigger("StartCinematic"); 
        IsPlaying = true;
    }

    public void EndCinematic()
    {
        animator.ResetTrigger("StartCinematic");
        animator.SetTrigger("CloseCinematic");
        IsPlaying = false;
    }

    public void StartBattleCinematic()
    {
        animator.ResetTrigger("CloseCinematic");
        animator.SetTrigger("StartCinematic");
        UIController.Instance.ShowBattleUI();
    }

    public void EndBattleCinematic()
    {
        animator.ResetTrigger("StartCinematic");
        animator.SetTrigger("CloseCinematic");
        UIController.Instance.HideBattleUI();
    }

    public void ZoomIn()
    {
        _isZoomIn = true;
    }

    public void ResetZoom()
    {
        _isZoomReset = true;
    }

    protected override void Awake()
    {
        base.Awake();
        _defaultZoom = virtualCamera.m_Lens.FieldOfView;
    }

    private void Update()
    {
        if (_isZoomIn && virtualCamera.m_Lens.FieldOfView > zoomInValue)
        {
            virtualCamera.m_Lens.FieldOfView -= Time.deltaTime * zoomSpeed;
        }
        else if(_isZoomIn)
        {
            _isZoomIn = false;
            virtualCamera.m_Lens.FieldOfView = zoomInValue;
        }

        if (_isZoomReset && virtualCamera.m_Lens.FieldOfView < _defaultZoom)
        {
            virtualCamera.m_Lens.FieldOfView += Time.deltaTime * zoomSpeed;
        }
        else if(!_isZoomIn && _isZoomReset)
        {
            _isZoomReset = false;
            virtualCamera.m_Lens.FieldOfView = _defaultZoom;
        }
    }
}
