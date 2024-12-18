using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO.Ports;


public class LightController : MonoBehaviour
{
    SerialPort stream = new SerialPort("COM3", 9600);

    [SerializeField] private List<Light> group1Lights;
    [SerializeField] private Transform group1Target;

    [SerializeField] private List<Light> group2Lights;
    [SerializeField] private Transform group2Target;

    [SerializeField] private List<Light> group3Lights;
    [SerializeField] private Transform group3Target;

    private bool group1Active = false;
    private bool group2Active = false;
    private bool group3Active = false;

    void Start()
    {
        
        SetLightsActive(group1Lights, false);
        SetLightsActive(group2Lights, false);
        SetLightsActive(group3Lights, false);

        // Open the serial port
        stream.Open();
    }

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            group1Active = !group1Active;
            SetLightsActive(group1Lights, group1Active);
        }

        
        if (stream.IsOpen)
        {
            try
            {
                string value = stream.ReadLine(); 
                int intValue = int.Parse(value);  

                if (intValue == 1) 
                {
                    SetLightsActive(group1Lights, true);
                }
                else if (intValue == 0) 
                {
                    SetLightsActive(group1Lights, false);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Error reading serial input: " + e.Message);
            }
        }

        
        if (Input.GetKeyDown(KeyCode.H))
        {
            group2Active = !group2Active;
            SetLightsActive(group2Lights, group2Active);
        }

        
        if (Input.GetKeyDown(KeyCode.J))
        {
            group3Active = !group3Active;
            SetLightsActive(group3Lights, group3Active);
        }

      
        if (group1Active && group1Target != null)
        {
            FollowTarget(group1Lights, group1Target);
        }

        if (group2Active && group2Target != null)
        {
            FollowTarget(group2Lights, group2Target);
        }

        if (group3Active && group3Target != null)
        {
            FollowTarget(group3Lights, group3Target);
        }
    }

    private void SetLightsActive(List<Light> lights, bool isActive)
    {
        foreach (Light light in lights)
        {
            if (light != null)
            {
                light.enabled = isActive; // Toggle lights on/off
            }
        }
    }

    private void FollowTarget(List<Light> lights, Transform target)
    {
        foreach (Light light in lights)
        {
            if (light != null)
            {
                light.transform.position = target.position;
            }
        }
    }
}
