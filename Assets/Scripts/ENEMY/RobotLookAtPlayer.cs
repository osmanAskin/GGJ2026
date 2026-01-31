using UnityEngine;

public class EnemyDetectAndLook : MonoBehaviour
{
    public Transform player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player enemy alanýna girdi");

            LookAtPlayer();
        }
    }

    void LookAtPlayer()
    {
        if (player == null) return;

        Vector3 dir = player.position - transform.position;

        float angleZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(
            transform.eulerAngles.x,
            transform.eulerAngles.y,
            angleZ
        );
    }
}
