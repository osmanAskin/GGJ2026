using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public partial class EnemyHealth : MonoBehaviour
{
    [Header("Can Ayarları")]
    public float maxHealth = 200f;
    private float currentHealth;

    [Header("UI Referansı")]
    public Slider healthBar; // Inspector'dan can barını buraya sürükle

    [Header("Hit Effect (Material)")]
    public float flashDuration = 0.15f;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    
    private SkinnedMeshRenderer[] renderers;
    private MaterialPropertyBlock propBlock;

    void Start()
    {
        currentHealth = maxHealth;

        // UI Slider başlangıç ayarları
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        propBlock = new MaterialPropertyBlock();
        
        // Başlangıçta emission'ı temizle
        SetEmission(Color.black);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Tag kontrolü (B harfi büyük "Bullet")
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(10f);
            Destroy(other.gameObject); 
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // Slider'ı güncelle
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        // Parlama efektini başlat
        StopCoroutine(nameof(HitFlashEffect));
        StartCoroutine(HitFlashEffect());
        
        Debug.Log("Düşman hasar aldı! Kalan: " + currentHealth);

        if (currentHealth <= 0) Die();
    }

    IEnumerator HitFlashEffect()
    {
        SetEmission(Color.white * 5f); // Beyaz parlama
        yield return new WaitForSeconds(flashDuration);
        SetEmission(Color.black); // Sönme
    }

    void SetEmission(Color color)
    {
        if (renderers == null) return;
        
        foreach (var r in renderers)
        {
            if (r == null) continue;
            r.GetPropertyBlock(propBlock);
            propBlock.SetColor(EmissionColor, color);
            r.SetPropertyBlock(propBlock);
        }
    }

    void Die()
    {
        Debug.Log("Düşman Öldü!");
        Destroy(gameObject);
    }
}