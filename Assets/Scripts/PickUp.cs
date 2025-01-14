using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public GameObject drum;
    public GameObject playerDrum;
    public GameObject invisWall;

    private void OnTriggerEnter(Collider other)
    {
        if (drum.activeSelf)
        {
            playerDrum.SetActive(true);
            drum.SetActive(false);
            invisWall.SetActive(false);
        }
    }
}
