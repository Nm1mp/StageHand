using UnityEngine;

public class SlidePotLightControl : MonoBehaviour
{
    [SerializeField] private Light[] lights;
    [SerializeField] private Transform[] lightTargets;
    [SerializeField] private Transform targetObject;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Transform player;
    [SerializeField] private float playerRadius = 5f;

    private bool hasReachedTargetPosition = false;

    void Update()
    {
        if (SerialPortManager.Instance != null)
        {
            // Get potentiometer value
            int potValue = SerialPortManager.Instance.GetPotentiometerValue(); // Fetch potentiometer value from manager

            if (potValue > 100)
            {
                SetLightsActive(true);
                float intensity = Mathf.Clamp((potValue - 100) / 18f, 0, 50);
                SetLightsIntensity(intensity);
                UpdateLightPositions();
            }
            else
            {
                SetLightsActive(false);
            }

            HandleTargetObjectMovement();
        }
        else
        {
            Debug.LogWarning("SerialPortManager instance is null.");
        }
    }

    private void HandleTargetObjectMovement()
    {
        if (AreLightsOn() && targetObject != null)
        {
            if (!hasReachedTargetPosition)
            {
                targetObject.position = Vector3.MoveTowards(targetObject.position, targetPosition, 2f * Time.deltaTime);

                if (Vector3.Distance(targetObject.position, targetPosition) < 0.1f)
                {
                    hasReachedTargetPosition = true;
                }
            }
            else
            {
                float distanceToPlayer = Vector3.Distance(targetObject.position, player.position);
                if (distanceToPlayer <= playerRadius)
                {
                    targetObject.position = Vector3.MoveTowards(targetObject.position, player.position + Vector3.up * 2f, 2f * Time.deltaTime);
                }
            }
        }
    }

    private bool AreLightsOn()
    {
        foreach (Light light in lights)
        {
            if (light != null && light.enabled)
            {
                return true;
            }
        }
        return false;
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

    private void UpdateLightPositions()
    {
        int minCount = Mathf.Min(lights.Length, lightTargets.Length);
        for (int i = 0; i < minCount; i++)
        {
            if (lights[i] != null && lightTargets[i] != null)
            {
                lights[i].transform.position = lightTargets[i].position;
            }
        }
    }
}
