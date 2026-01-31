using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Player -" + damage + " can. Kalan: " + health);
        
        if (health <= 0) Debug.Log("Player Öldü!");
    }
}