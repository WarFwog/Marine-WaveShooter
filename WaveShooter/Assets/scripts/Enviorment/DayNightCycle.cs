using System;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light _directionalLight;

    [Header("Settings")]
    [SerializeField] private float _duration = 300f;

    private float _timePassed;

    private void Update()
    {
        _timePassed += Time.deltaTime;
        var time = Mathf.Clamp01(_timePassed / _duration);
        
        var angle = Mathf.Lerp(0f, 180f, time);
        
        var lightRotation = Quaternion.Euler(angle + 90f, 0f, 0f);
        _directionalLight.transform.rotation = lightRotation;
        
        _directionalLight.intensity = Mathf.Lerp(1f, 0.1f, time);
        
        _directionalLight.color = Color.Lerp(
            new Color(1f, 0.95f, 0.8f),
            new Color(0.2f, 0.2f, 0.5f),
            time
        );
    }

    public void ResetCycle()
    {
        _timePassed = 0f;
        
        _directionalLight.intensity = 1f;
        _directionalLight.color = new Color(1f, 0.95f, 0.8f);

    }
}