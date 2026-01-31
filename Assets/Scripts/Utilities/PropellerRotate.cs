using UnityEngine;

public class PropellerRotate : MonoBehaviour
{
    public float rotationSpeed = 360f; // derece/sn
    public Vector3 rotationAxis = Vector3.forward; // genelde Z

    void Update()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}
