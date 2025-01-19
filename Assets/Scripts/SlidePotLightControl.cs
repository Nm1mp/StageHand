using UnityEngine;

public class SlidePotLightControl : MonoBehaviour
{
    [SerializeField] private Light[] lights; // Lights to control
    [SerializeField] private float maxIntensity = 10f; // Maximum intensity for lights

    void Update()
    {
        if (SerialPortManager.Instance != null)
        {
            int potValue = SerialPortManager.Instance.GetPotentiometerValue(); // Get potentiometer value (0-1023)

            if (potValue > 100) // Only activate lights if the potentiometer is past a threshold
            {
                SetLightsActive(true);
                float intensity = Mathf.Clamp((potValue - 100) / 10f, 0, maxIntensity); // Map potValue to intensity
                SetLightsIntensity(intensity);
            }
            else
            {
                SetLightsActive(false);
            }
        }
        else
        {
            Debug.LogWarning("SerialPortManager instance is null.");
        }
    }

    private void SetLightsActive(bool isActive)
    {
        foreach (Light light in lights)
        {
            if (light != null)
            {
                light.enabled = isActive;

                if (!isActive)
                {
                    light.intensity = 0f;
                }
            }
        }
    }

    private void SetLightsIntensity(float intensity)
    {
        foreach (Light light in lights)
        {
            if (light != null)
            {
                light.intensity = intensity;
            }
        }
    }
}
