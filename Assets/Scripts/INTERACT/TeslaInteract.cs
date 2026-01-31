using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeslaInteract : MonoBehaviour
{
    [Header("UI")]
    public GameObject interactText;

    [Header("Explosion")]
    public GameObject explosionPrefab;

    [Header("Physics Objects")]
    public List<Rigidbody> rigidbodiesToActivate = new List<Rigidbody>();
    public List<BoxCollider> boxCollidersToActivate = new List<BoxCollider>();

    public float colliderSizeMultiplier = 1.5f;
    public float affectedObjectsDestroyTime = 1f;

    [Header("Win Condition")]
    public GameObject winPanel;
    public int totalTeslaToWin = 4;

    private static int destroyedTeslaCount = 0;

    private bool playerInside;
    private bool exploded;

    private HashSet<GameObject> affectedRootObjects = new HashSet<GameObject>();

    void Start()
    {
        if (interactText != null)
            interactText.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(false);
    }

    void Update()
    {
        if (!playerInside || exploded) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            ExplodeTesla();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;
        if (interactText != null)
            interactText.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        if (interactText != null)
            interactText.SetActive(false);
    }

    void ExplodeTesla()
    {
        exploded = true;

        Debug.Log("TESLA PATLADI");

        ActivateRigidbodies();
        ActivateColliders();

        if (explosionPrefab != null)
        {
            GameObject fx = Instantiate(
                explosionPrefab,
                transform.position,
                Quaternion.identity
            );
            Destroy(fx, 3f);
        }

        StartCoroutine(DestroyAffectedObjects());

        DisableTeslaVisuals();
    }

    IEnumerator DestroyAffectedObjects()
    {
        yield return new WaitForSeconds(affectedObjectsDestroyTime);

        foreach (GameObject obj in affectedRootObjects)
        {
            if (obj != null)
                Destroy(obj);
        }

        RegisterTeslaDestroyed();

        Destroy(gameObject);
    }

    void RegisterTeslaDestroyed()
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

        Time.timeScale = 0f; // oyunu durdur
    }

    void ActivateRigidbodies()
    {
        foreach (Rigidbody rb in rigidbodiesToActivate)
        {
            if (rb == null) continue;

            rb.isKinematic = false;
            rb.useGravity = true;

            rb.AddExplosionForce(
                300f,
                transform.position,
                5f,
                1f,
                ForceMode.Impulse
            );

            affectedRootObjects.Add(rb.transform.root.gameObject);
        }
    }

    void ActivateColliders()
    {
        foreach (BoxCollider col in boxCollidersToActivate)
        {
            if (col == null) continue;

            col.isTrigger = false;
            col.size *= colliderSizeMultiplier;

            affectedRootObjects.Add(col.transform.root.gameObject);
        }
    }

    void DisableTeslaVisuals()
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = false;

        foreach (Collider c in GetComponentsInChildren<Collider>())
            c.enabled = false;
    }
}
