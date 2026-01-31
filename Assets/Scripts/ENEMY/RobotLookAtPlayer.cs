using UnityEngine;

public class LookAtPlayerZOnly : MonoBehaviour
{
    public Transform player;
    public float rotationSpeed = 5f;

    void Update()
    {
        Vector3 dir = player.position - transform.position;

        // Z ekseni için açý hesabý
        float angleZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(
            transform.eulerAngles.x,
            transform.eulerAngles.y,
            angleZ
        );

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
