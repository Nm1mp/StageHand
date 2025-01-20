using UnityEngine;

public class objMover : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float minZ = -8f;
    [SerializeField] private float maxZ = 8f;

    [Header("Models")]
    [SerializeField] private GameObject forwardModel;
    [SerializeField] private GameObject backwardModel;
    [SerializeField] private GameObject forwardDrum;
    [SerializeField] private GameObject backwardDrum;
    [SerializeField] private GameObject forwardM;
    [SerializeField] private GameObject backwardM;

    [Header("Drum Reference")]
    [SerializeField] private GameObject drum;

    private float lastInput = 0;

    void Update()
    {
        float verticalInput = Input.GetAxis("Horizontal");

        if (SerialPortManager.Instance.IsButtonPressed(4)) verticalInput = 1;
        if (SerialPortManager.Instance.IsButtonPressed(2)) verticalInput = -1;

        if (verticalInput > 0)
        {
            ActivateModel(forwardModel, backwardModel);

            if (drum == null || !drum.activeSelf) 
            {
                ActivateDrum(forwardDrum, backwardDrum);
            }

            ActivateM(forwardM, backwardM);
        }
        else if (verticalInput < 0)
        {
            ActivateModel(backwardModel, forwardModel);

            if (drum == null || !drum.activeSelf) 
            {
                ActivateDrum(backwardDrum, forwardDrum);
            }

            ActivateM(backwardM, forwardM);
        }

        Vector3 movement = new Vector3(0, 0, verticalInput) * speed * Time.deltaTime;
        transform.Translate(movement);

        float clampedZ = Mathf.Clamp(transform.position.z, minZ, maxZ);
        transform.position = new Vector3(transform.position.x, transform.position.y, clampedZ);
    }

    private void ActivateModel(GameObject modelToActivate, GameObject modelToDeactivate)
    {
        if (!modelToActivate.activeSelf) modelToActivate.SetActive(true);
        if (modelToDeactivate.activeSelf) modelToDeactivate.SetActive(false);
    }

    private void ActivateDrum(GameObject drumToActivate, GameObject drumToDeactivate)
    {
        if (!drumToActivate.activeSelf) drumToActivate.SetActive(true);
        if (drumToDeactivate.activeSelf) drumToDeactivate.SetActive(false);
    }

    private void ActivateM(GameObject mToActivate, GameObject mToDeactivate)
    {
        if (!mToActivate.activeSelf) mToActivate.SetActive(true);
        if (mToDeactivate.activeSelf) mToDeactivate.SetActive(false);
    }

    
}
