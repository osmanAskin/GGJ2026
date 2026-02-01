using UnityEngine;
using TMPro; 
using System.Collections;
using System.Collections.Generic;

public class NarrativeSystem : MonoBehaviour
{
    [Header("UI Elemanları")]
    public TextMeshProUGUI narrativeText; 

    [Header("Narrative Ayarları")]
    [TextArea(3, 10)]
    public List<string> dialogueLines; 
    public float charCooldown = 0.05f; 
    public float lineCooldown = 2f;    

    private int currentLineIndex = 0;
    private bool isPlaying = false; // Yazı şu an oynatılıyor mu?

    void Update()
    {
        // E tuşuna basıldığında ve sistem zaten çalışmıyorsa başlat
        if (Input.GetKeyDown(KeyCode.T) && !isPlaying)
        {
            StartNarrative();
        }
    }

    public void StartNarrative()
    {
        if (dialogueLines.Count > 0 && currentLineIndex < dialogueLines.Count)
        {
            StartCoroutine(PlayNarrative());
        }
    }

    IEnumerator PlayNarrative()
    {
        isPlaying = true; // Sistem kilitlendi, E tuşu tekrar basılsa da etkilemez
        
        while (currentLineIndex < dialogueLines.Count)
        {
            // Yazıyı daktilo efektiyle yazdır
            yield return StartCoroutine(TypeText(dialogueLines[currentLineIndex]));

            // Cümle bitince belirlediğin cooldown kadar bekle
            yield return new WaitForSeconds(lineCooldown);

            currentLineIndex++;
            narrativeText.text = ""; 
        }

        // Tüm liste bittiğinde sıfırla (İstersen tekrar oynatabilmek için)
        currentLineIndex = 0; 
        isPlaying = false;
        Debug.Log("Anlatı tamamen bitti.");
    }

    IEnumerator TypeText(string line)
    {
        narrativeText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            narrativeText.text += letter;
            yield return new WaitForSeconds(charCooldown);
        }
    }
}