using UnityEngine;

public class MaskInteraction : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.E;

    [Header("Rotation")]
    public Vector3 faceRotationEuler;
    public float rotateSpeed = 6f;

    private Quaternion handRotation;
    private Quaternion faceRotation;

    private bool isEquipped = false;
    private bool isRotating = false;

    void Start()
    {
        handRotation = transform.localRotation;
        faceRotation = Quaternion.Euler(faceRotationEuler);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            isEquipped = !isEquipped;
            isRotating = true;

            Debug.Log(isEquipped ? "Maske takýlýyor" : "Maske çýkarýlýyor");
        }

        if (isRotating)
        {
            Quaternion target = isEquipped ? faceRotation : handRotation;

            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                target,
                Time.deltaTime * rotateSpeed
            );

            if (Quaternion.Angle(transform.localRotation, target) < 1f)
            {
                transform.localRotation = target;
                isRotating = false;

                Debug.Log(isEquipped ? "Maske takýldý" : "Maske çýkarýldý");
            }
        }
    }
}
