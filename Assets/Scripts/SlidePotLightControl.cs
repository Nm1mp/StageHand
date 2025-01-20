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
    private bool lightsWereOff = false; 

    void Update()
    {
        if (SerialPortManager.Instance != null)
        {
            int potValue = SerialPortManager.Instance.GetPotentiometerValue("S"); 

            if (potValue > 100)
            {
                SetLightsActive(true);
                float intensity = Mathf.Clamp((potValue - 23) / 100f, 0, 100);
                SetLightsIntensity(intensity);
                UpdateLightPositions();
                lightsWereOff = false; 
            }
            else
            {
                SetLightsActive(false);
                lightsWereOff = true; 
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
        if (targetObject != null)
        {
            if (!lightsWereOff && AreLightsOn() && !hasReachedTargetPosition)
            {
                targetObject.position = Vector3.MoveTowards(targetObject.position, targetPosition, 2f * Time.deltaTime);

                if (Vector3.Distance(targetObject.position, targetPosition) < 0.1f)
                {
                    hasReachedTargetPosition = true;
                }
            }
            else if (AreLightsOn() && hasReachedTargetPosition)
            {
                float distanceToPlayer = Vector3.Distance(targetObject.position, player.position);
                if (distanceToPlayer <= playerRadius)
                {
                    targetObject.position = Vector3.MoveTowards(targetObject.position, player.position + Vector3.up * 2f, 2f * Time.deltaTime);
                }
            }
            else if (lightsWereOff)
            {
                hasReachedTargetPosition = false;
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
