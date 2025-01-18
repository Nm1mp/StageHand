using UnityEngine;

public class PickUp : MonoBehaviour
{
    // Scene 2
    public GameObject drum;
    public GameObject playerDrum;

    // Scene 3
    public GameObject trumpet;
    public GameObject playerTrumpet;

    private bool drumCollected = false;
    private bool trumpetCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (drum != null && !drumCollected)
        {
            if (drum.activeSelf)
            {
                playerDrum?.SetActive(true);
                drum.SetActive(false);
                drumCollected = true;
                Debug.Log("Drum collected.");
            }
        }

        if (trumpet != null && !trumpetCollected)
        {
            if (trumpet.activeSelf)
            {
                playerTrumpet?.SetActive(true);
                trumpet.SetActive(false);
                trumpetCollected = true;
                Debug.Log("Trumpet collected.");
            }
        }
    }
}
