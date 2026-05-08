using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    public Material skyboxMaterial; //skybox blend
    public float duration = 300f; //5 min

    private float _timePassed = 0f;

    private void Start()
    {
        _timePassed = 0f;
        RenderSettings.skybox = skyboxMaterial;
        skyboxMaterial.SetFloat("_Blend", 0f);
    }

    private void Update()
    {
        _timePassed += Time.deltaTime;

        var t = Mathf.Clamp01(_timePassed / duration);
        skyboxMaterial.SetFloat("_Blend", t);
    }

    public void ResetCycle()
    {
        _timePassed = 0f;
        skyboxMaterial.SetFloat("_Blend", 0f);
    }
}