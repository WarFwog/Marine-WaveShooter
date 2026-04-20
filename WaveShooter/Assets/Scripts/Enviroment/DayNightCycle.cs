using System;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("References")]
    public Transform _celestialPivot;
    public Light _directionalLight;
    public GameObject _sun;
    public GameObject _moon;

    [Header("Settings")]
    public float _duration = 300f;

    private float _timePassed;

    private void Update()
    {
        _timePassed += Time.deltaTime;
        var time = Mathf.Clamp01(_timePassed / _duration);
        
        var angle = Mathf.Lerp(0f, 180f, time);
        _celestialPivot.rotation = Quaternion.Euler(angle, 0f, 0f);
        
        var lightRotation = Quaternion.Euler(angle + 90f, 0f, 0f);
        _directionalLight.transform.rotation = lightRotation;
        
        _directionalLight.intensity = Mathf.Lerp(1f, 0.1f, time);
        
        _directionalLight.color = Color.Lerp(
            new Color(1f, 0.95f, 0.8f),
            new Color(0.2f, 0.2f, 0.5f),
            time
        );
        
        // 🌞 Sun disappears later
        if (_sun != null)
            _sun.SetActive(time < 0.65f);

// 🌙 Moon appears earlier
        if (_moon != null)
            _moon.SetActive(time > 0.35f);

        if (Camera.main == null) return;
        if (_sun != null)
            _sun.transform.LookAt(Camera.main.transform);

        if (_moon != null)
            _moon.transform.LookAt(Camera.main.transform);
    }

    public void ResetCycle()
    {
        _timePassed = 0f;
        
        _directionalLight.intensity = 1f;
        _directionalLight.color = new Color(1f, 0.95f, 0.8f);
        
        if (_sun != null) _sun.SetActive(true);
        if (_moon != null) _moon.SetActive(false);
    }
}