using UnityEngine;

public class LookAtPlayerYOnly : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 5f;

    void Update()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
