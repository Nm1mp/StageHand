using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;


public class SlidePotLightControl : MonoBehaviour
{
    [SerializeField] private string portName = "COM4";
    [SerializeField] private int baudRate = 9600;

    [SerializeField] private Light[] lights;
    [SerializeField] private Transform[] lightTargets;

    [SerializeField] private Transform targetObject; 
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private Transform player; 
    [SerializeField] private float playerRadius = 5f; 

    private SerialPort serialPort;
    private bool hasReachedTargetPosition = false; 

    void Start()
    {
        serialPort = new SerialPort(portName, baudRate);

        try
        {
            serialPort.Open();
            Debug.Log("Serial port opened successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error opening serial port: " + e.Message);
            serialPort = null;
        }

        SetLightsActive(false);
    }

    void Update()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                string data = serialPort.ReadLine().Trim();

                if (!string.IsNullOrEmpty(data) && IsValidPotentiometerValue(data, out int potValue))
                {
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
                }
                else
                {
                    Debug.LogWarning($"Invalid data received: \"{data}\"");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Error reading from serial port: " + e.Message);
            }
        }
        else
        {
            Debug.LogWarning("Serial port not available. Potentiometer mechanics disabled.");
        }

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

    private bool IsValidPotentiometerValue(string data, out int value)
    {
        value = 0;

        if (int.TryParse(data, out value))
        {
            return value >= 0 && value <= 1023;
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

    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}




