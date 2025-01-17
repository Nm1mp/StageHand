using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;



public class PostProcessingDarkEffect : MonoBehaviour
{
    [SerializeField] private Volume postProcessingVolume;
    [SerializeField] private List<Light> lights;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject cat;
    private ColorAdjustments colorAdjustments;
    private MonoBehaviour playerMovementScript;
    private MonoBehaviour catMovementScript;

    void Start()
    {
        if (postProcessingVolume.profile.TryGet<ColorAdjustments>(out var adjustments))
        {
            colorAdjustments = adjustments;
        }

        playerMovementScript = player.GetComponent<MonoBehaviour>();
    }

    void Update()
    {
        bool anyLightOn = false;
        foreach (Light light in lights)
        {
            if (light.enabled)
            {
                anyLightOn = true;
                break;
            }
        }

        if (anyLightOn && colorAdjustments.postExposure.value < 0)
        {
            StartCoroutine(FadeOutPostExposure());
            EnablePlayerMovement(true);
        }
        else if (!anyLightOn)
        {
            EnablePlayerMovement(false);
        }
    }

    private IEnumerator FadeOutPostExposure()
    {
        float duration = 3f;
        float startExposure = colorAdjustments.postExposure.value;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            colorAdjustments.postExposure.value = Mathf.Lerp(startExposure, 0f, elapsedTime / duration);
            yield return null;
        }

        colorAdjustments.postExposure.value = 0f;
    }

    private void EnablePlayerMovement(bool enable)
    {
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = enable;
        }
        if (catMovementScript != null  )
        {
            catMovementScript.enabled = enable;
        }
    }
}


