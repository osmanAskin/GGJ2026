using UnityEngine;
using System.Collections;

public class SwordSwing : MonoBehaviour
{
    [Header("Swing Settings")]
    public float swingAngle = 120f;      // sola savurma açýsý
    public float swingDuration = 0.08f;  // savurma süresi (ýþýn kýlýcý hissi için kýsa)
    public float returnDuration = 0.12f; // geri dönüþ

    private Quaternion startRotation;
    private bool isSwinging;

    void Start()
    {
        startRotation = transform.localRotation;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            StartCoroutine(Swing());
        }
    }

    IEnumerator Swing()
    {
        isSwinging = true;

        // Hedef rotasyon (sola savurma)
        Quaternion targetRotation =
            startRotation * Quaternion.Euler(0f, 0f, swingAngle);

        float t = 0f;

        // Savurma
        while (t < 1f)
        {
            t += Time.deltaTime / swingDuration;
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        t = 0f;

        // Geri dönüþ
        while (t < 1f)
        {
            t += Time.deltaTime / returnDuration;
            transform.localRotation = Quaternion.Slerp(targetRotation, startRotation, t);
            yield return null;
        }

        transform.localRotation = startRotation;
        isSwinging = false;
    }
}
