using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public partial class EnemyHealth : MonoBehaviour
{
    [Header("Can Ayarları")]
    public float maxHealth = 200f;
    private float currentHealth;

    [Header("UI Referansı")]
    public Slider healthBar;

    [Header("Hit Effect (Material)")]
    public float flashDuration = 0.15f;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    private SkinnedMeshRenderer[] renderers;
    private MaterialPropertyBlock propBlock;

    [Header("Death Effect")]
    public GameObject explosionPrefab;
    public float destroyDelay = 1.5f;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        propBlock = new MaterialPropertyBlock();

        SetEmission(Color.black);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("Bullet"))
        {
            TakeDamage(10f);
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (healthBar != null)
            healthBar.value = currentHealth;

        StopCoroutine(nameof(HitFlashEffect));
        StartCoroutine(HitFlashEffect());

        if (currentHealth <= 0)
            Die();
    }

    IEnumerator HitFlashEffect()
    {
        SetEmission(Color.white * 5f);
        yield return new WaitForSeconds(flashDuration);
        SetEmission(Color.black);
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
        if (isDead) return;
        isDead = true;

        // Patlama efekti
        if (explosionPrefab != null)
        {
            Instantiate(
                explosionPrefab,
                transform.position,
                Quaternion.identity
            );
        }

        // Modeli kapat
        foreach (var r in renderers)
            r.enabled = false;

        // Collider kapat
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // UI gizle
        if (healthBar != null)
            healthBar.gameObject.SetActive(false);

        Destroy(gameObject, destroyDelay);
    }
}
