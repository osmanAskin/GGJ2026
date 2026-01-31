using UnityEngine;

public class TeslaManager : MonoBehaviour
{
    public static TeslaManager Instance;

    [Header("Win Condition")]
    public int totalTeslaToWin = 4;
    public GameObject winPanel;

    private int destroyedTeslaCount;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        destroyedTeslaCount = 0;

        if (winPanel != null)
            winPanel.SetActive(false);
    }

    public void RegisterTeslaDestroyed()
    {
        destroyedTeslaCount++;
        Debug.Log("YOK EDÝLEN TESLA: " + destroyedTeslaCount);

        if (destroyedTeslaCount >= totalTeslaToWin)
        {
            TriggerWin();
        }
    }

    void TriggerWin()
    {
        Debug.Log("WIN CONDITION SAÐLANDI");

        if (winPanel != null)
            winPanel.SetActive(true);

        Time.timeScale = 0f;
    }
}
