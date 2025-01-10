using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGrow : MonoBehaviour
{
    public GameObject Seed;
    public GameObject Bush;
    public float Grow;
    private int growRate = 5;

    public void Update()
    {
        if (Grow >= growRate)
        {
            Seed.SetActive(false);
            Bush.SetActive(true);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Grow += (1 * Time.deltaTime);
        Debug.Log("groooowwwww");
    }
}
