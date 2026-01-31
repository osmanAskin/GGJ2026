using UnityEngine;

public class EnemyDetectAndLook : MonoBehaviour
{
    public Transform player;

    private void OnTriggerStay(Collider other)
    { 
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player enemy alan�na girdi");

            LookAtPlayer();
        }
    }

    void LookAtPlayer()
    {
        if (player == null) return;

        // Oyuncuya olan yönü hesapla
        Vector3 dir = player.position - transform.position;
    
        // Y eksenindeki farkı sıfırla (Böylece karakter yukarı/aşağı bakıp dengesini bozmaz)
        dir.y = 0; 

        if (dir != Vector3.zero)
        {
            // Yönü rotasyona çevir
            Quaternion targetRotation = Quaternion.LookRotation(dir);
        
            // Sadece Y ekseninde dönmesini sağla (Düşmeyi engeller)
            transform.rotation = targetRotation;
        }
    }
}
