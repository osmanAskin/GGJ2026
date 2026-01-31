using UnityEngine;
using UnityEngine.UI;

public class StaminaSystem : MonoBehaviour
{
    [Header("Stamina Values")]
    public float maxStamina = 100f;
    public float staminaDecreaseRate = 5f; // saniyede kaç azalacak

    [Header("UI")]
    public Slider staminaSlider;

    private float currentStamina;
    private bool staminaFinished;

    void Start()
    {
        currentStamina = maxStamina;

        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }
    }

    void Update()
    {
        if (staminaFinished)
            return;

        DecreaseStamina();
        UpdateUI();
    }

    void DecreaseStamina()
    {
        currentStamina -= staminaDecreaseRate * Time.deltaTime;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        if (currentStamina <= 0f)
        {
            staminaFinished = true;
            OnStaminaEmpty();
        }
    }

    void UpdateUI()
    {
        if (staminaSlider != null)
            staminaSlider.value = currentStamina;
    }

    void OnStaminaEmpty()
    {
        Debug.Log("Stamina bitti");
        // Ýleride buraya panel açma / oyunu durdurma gelecek
    }
}
