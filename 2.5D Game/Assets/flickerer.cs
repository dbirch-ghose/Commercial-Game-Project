using UnityEngine;

public class flickerer : MonoBehaviour
{
    private Light light;
    public float minIntensity = 1.6f;
    public float maxIntensity = 2.0f;
    public float flickerSpeed = 2.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        light = GetComponent<Light>();

    }

    // Update is called once per frame
    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
        light.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}
