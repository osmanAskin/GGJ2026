using UnityEngine;

public class MaskInteraction : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.E;

    [Header("Mask Rotation")]
    public Vector3 faceRotationEuler;
    public float rotateSpeed = 6f;

    [Header("Player")]
    public Transform player;

    [Header("Upside World Spawn Points")]
    public Transform[] upsideSpawnPoints;

    private Quaternion handRotation;
    private Quaternion faceRotation;

    private bool isRotating = false;
    private bool isInUpsideWorld = false;

    CharacterController characterController;
    Rigidbody rb;

    void Awake()
    {
        characterController = player.GetComponent<CharacterController>();
        rb = player.GetComponent<Rigidbody>();
    }

    void Start()
    {
        handRotation = transform.localRotation;
        faceRotation = Quaternion.Euler(faceRotationEuler);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey) && !isRotating)
        {
            isRotating = true;

            if (!isInUpsideWorld)
                EnterUpsideWorld();
            else
                ExitUpsideWorld();
        }

        RotateMask();
    }

    void EnterUpsideWorld()
    {
        isInUpsideWorld = true;

        Transform closestPoint = GetClosestSpawnPoint(player.position);

        if (closestPoint != null)
        {
            TeleportPlayer(closestPoint.position);
            Debug.Log("Upside World → En yakın noktaya spawnlandı");
        }
        else
        {
            Debug.LogError("Upside spawn point bulunamadı!");
        }
    }

    void ExitUpsideWorld()
    {
        isInUpsideWorld = false;
        Debug.Log("Normal World'a dönüldü");
    }

    Transform GetClosestSpawnPoint(Vector3 fromPosition)
    {
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform point in upsideSpawnPoints)
        {
            float dist = Vector3.Distance(fromPosition, point.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = point;
            }
        }

        return closest;
    }

    void TeleportPlayer(Vector3 targetPosition)
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        if (characterController != null)
            characterController.enabled = false;

        player.position = targetPosition;

        if (characterController != null)
            characterController.enabled = true;

        if (rb != null)
            rb.isKinematic = false;
    }

    void RotateMask()
    {
        Quaternion targetRotation = isInUpsideWorld ? faceRotation : handRotation;

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetRotation,
            Time.deltaTime * rotateSpeed
        );

        if (Quaternion.Angle(transform.localRotation, targetRotation) < 0.5f)
        {
            transform.localRotation = targetRotation;
            isRotating = false;
        }
    }
}
