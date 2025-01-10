using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    public GameObject Bush;
    public Transform Bird;
    public Transform Drum;
    private bool Raining = false;
    private bool rainStop = true;
    public ParticleSystem Rain;
    public GameObject RainHitbox;
    private float moveSpeed;
    public float RainFlow;
    private int rainTime = 5;

    private Vector3 birdMove = new Vector3(-0.13f, -2.763f, 4.33f);

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            Raining = true;
            rainStop = false;
        }


        if (Raining == true)
        {
            Rain.Play();
            RainHitbox.SetActive(true);
            RainFlow += (1 * Time.deltaTime);
        }

        if (rainStop == true)
        {
            Rain.Stop();
            RainHitbox.SetActive(false);
        }

        if (RainFlow >= rainTime)
        {
            Raining = false;
            rainStop = true;
        }

        if (Bush.activeSelf == true)
        {
            Debug.Log("Bush is active");
        }
    }
}
