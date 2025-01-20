using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackUpScript : MonoBehaviour
{

    public GameObject TC;

    void Start()
    {
        TC.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restarter();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Transition();
        }
    }

    private static void Restarter()
    {
        SceneManager.LoadScene(1);
    }

    private void Transition()
    {
        TC.SetActive(true);
    }

}
