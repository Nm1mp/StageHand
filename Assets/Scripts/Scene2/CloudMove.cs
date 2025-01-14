using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    // Objects
    public GameObject Bush;
    public GameObject Seed;
    private bool bushSpawned = false;
    [Space]
    // Bird things
    public Transform Bird;
    private float moveSpeed = 3.8f;
    private float    smooth = 3.8f;
    private Vector3 birdMove = new Vector3(-0.13f, -2.763f, 4.33f);
    [Space]
    // Drum things
    public Transform Drum;
    private Vector3 drumMove = new Vector3(5.12f, -2.786f, 2.38f);
    [Space]
    // Rain things
    private bool rainStarted = false;
    public ParticleSystem Rain;
    public float RainFlow;
    private int rainTime = 3;

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
            Bird.localPosition = Vector3.MoveTowards(Bird.localPosition, birdMove, moveSpeed * Time.deltaTime);
            Drum.localPosition = Vector3.MoveTowards(Drum.localPosition, drumMove, moveSpeed * Time.deltaTime);
            Quaternion target = Quaternion.Euler(0, 0, 0);
            Drum.rotation = Quaternion.Slerp(Drum.rotation, target, Time.deltaTime * smooth);

            if (Bird.localPosition == birdMove)
            {
                Debug.Log("bird landed");
                bushSpawned = true;
            }
            Debug.Log("Bush is active");
        }
    }
}
