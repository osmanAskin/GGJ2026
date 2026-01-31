using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TeslaInteract : MonoBehaviour
{
    [Header("UI")]
    public Text interactText;

    [Header("Explosion")]
    public GameObject explosionPrefab;

    [Header("Physics Objects")]
    public List<Rigidbody> rigidbodiesToActivate = new List<Rigidbody>();
    public List<BoxCollider> boxCollidersToActivate = new List<BoxCollider>();

    public float colliderSizeMultiplier = 1.5f;
    public float affectedObjectsDestroyTime = 1f;

    private bool playerInside;
    private bool exploded;

    private HashSet<GameObject> affectedRootObjects = new HashSet<GameObject>();

    void Start()
    {
        if (interactText != null)
            interactText.text = "";
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
            interactText.text = "Patlatmak için e tuþuna basýn";
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        if (interactText != null)
            interactText.text = "";
    }

    void ExplodeTesla()
    {
        exploded = true;

        Debug.Log("TESLA PATLADI – 1 SANÝYE SONRA REFERANSLI OBJELER YOK OLACAK");

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

        // Coroutine Tesla yok edilmeden baþlýyor
        StartCoroutine(DestroyAffectedObjects());

        // Tesla görselini kapat, script çalýþmaya devam etsin
        DisableTeslaVisuals();
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

    IEnumerator DestroyAffectedObjects()
    {
        yield return new WaitForSeconds(affectedObjectsDestroyTime);

        foreach (GameObject obj in affectedRootObjects)
        {
            if (obj != null)
                Destroy(obj);
        }

        // En son Tesla tamamen yok edilir
        Destroy(gameObject);
    }

    void DisableTeslaVisuals()
    {
        interactText.text = "";
        // Tesla'yý anýnda silmek yerine görünmez yapýyoruz
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = false;

        foreach (Collider c in GetComponentsInChildren<Collider>())
            c.enabled = false;
    }
}
