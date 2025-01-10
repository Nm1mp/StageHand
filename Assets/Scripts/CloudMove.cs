using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    // Objects
    public GameObject Bush;
    public GameObject Seed;
    private bool bushSpawned = false;

    // Bird things
    public Transform Bird;
    private float moveSpeed;
    private Vector3 birdMove = new Vector3(-0.13f, -2.763f, 4.33f);

    // Drum things
    public Transform Drum;

    // Rain things
    private bool rainStarted = false;
    public ParticleSystem Rain;
    public float RainFlow;
    private int rainTime = 5;

    void Update()
    {        
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (!rainStarted)
            {
                Debug.Log("Rain started");

                Rain.Play();
                rainStarted = true;
            }
        }

        if (rainStarted)
        {
            RainFlow += (1 * Time.deltaTime);
        }

        if (RainFlow >= rainTime && rainStarted)
        {
            Debug.Log("Rain stop");

            Rain.Stop();
            rainStarted = false;

            Bush.SetActive(true);
            Seed.SetActive(false);
        }

        if (Bush.activeSelf && !bushSpawned)
        {
            bushSpawned = true;
            Debug.Log("Bush is active");
        }
    }
}
