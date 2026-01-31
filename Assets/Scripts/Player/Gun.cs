using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform muzzlePoint;
    public GameObject bulletPrefab;

    [Header("Fire")]
    public float fireRate = 0.2f;
    private float nextFireTime;

    [Header("Recoil")]
    public float recoilDistance = 0.07f;
    public float recoilReturnSpeed = 20f;
    public float shakeAmount = 0.005f;

    private Vector3 initialLocalPos;

    void Start()
    {
        initialLocalPos = transform.localPosition;
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }

        // recoil geri dönüþ
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            initialLocalPos,
            Time.deltaTime * recoilReturnSpeed
        );
    }

    void Fire()
    {
        // Mermi HER ZAMAN namlu yönünde çýkar
        Instantiate(
            bulletPrefab,
            muzzlePoint.position,
            muzzlePoint.rotation
        );

        // Geri tepme (geriye itme)
        transform.localPosition -= Vector3.forward * recoilDistance;

        // Mikro titreþim
        transform.localPosition += Random.insideUnitSphere * shakeAmount;
    }
}
