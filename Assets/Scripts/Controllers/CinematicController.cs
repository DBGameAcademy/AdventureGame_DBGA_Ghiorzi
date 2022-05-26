using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

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
        UIController.Instance.HideInventory();
        animator.ResetTrigger("CloseCinematic");
        animator.SetTrigger("StartCinematic"); 
        IsPlaying = true;
    }

    public void EndCinematic()
    {
        animator.ResetTrigger("StartCinematic");
        animator.SetTrigger("CloseCinematic");
        IsPlaying = false;
        UIController.Instance.HideDungeonInfo();
        UIController.Instance.HideBattleUI();
    }

    public void StartBattleCinematic()
    {
        UIController.Instance.HideInventory();
        animator.ResetTrigger("CloseCinematic");
        animator.SetTrigger("StartCinematic");
        UIController.Instance.ShowBattleUI();
    }

    public void EndBattleCinematic()
    {
        UIController.Instance.HideInventory();
        animator.ResetTrigger("StartCinematic");
        animator.SetTrigger("CloseCinematic");
        UIController.Instance.HideBattleUI();
    }

    public void PlayCinematicForSeconds(float secs)
    {
        if (IsPlaying)
            return;
        StartCoroutine(COCinematicForSeconds(secs));
    }

    public void PlayCinematicForSeconds(float secs, Action OnCinematicEnd)
    {
        if (IsPlaying)
            return;
        StartCoroutine(COCinematicForSeconds(secs, OnCinematicEnd));
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

    private IEnumerator COCinematicForSeconds(float delay)
    {
        StartCinematic();
        yield return new WaitForSeconds(delay);
        EndCinematic();
    }

    private IEnumerator COCinematicForSeconds(float delay, Action OnCinematicEnd)
    {
        StartCinematic();
        yield return new WaitForSeconds(delay);
        EndCinematic();
        OnCinematicEnd?.Invoke();
    }
}
