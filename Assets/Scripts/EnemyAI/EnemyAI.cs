using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Mesafe Ayarları")]
    public float detectionRange = 10f;
    public float attackRange = 2f;

    [Header("Hareket Ayarları")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 5f;
    public float gravity = 9.81f;

    [Header("Saldırı Ayarları")]
    public int attackDamage = 10;
    public float attackSpeed = 1.5f;
    private float nextAttackTime = 0f;

    [Header("Referanslar")]
    public Transform player;
    private Animator anim;
    private CharacterController controller;
    
    private Vector3 velocity;
    private Vector3 finalMove;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (controller == null || !controller.enabled || !gameObject.activeInHierarchy) 
            return;

        if (player == null) return;

        finalMove = Vector3.zero; 

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Karar Mekanizması
        if (distanceToPlayer <= attackRange)
            AttackLogic(); // İsim çakışması önlendi
        else if (distanceToPlayer <= detectionRange)
            FollowPlayer();
        else
            Idle();

        ApplyGravity();
        controller.Move(finalMove * Time.deltaTime);
    }

    // DİKKAT: LateUpdate içindeki kameraya bakma kodunu buradan sildim. 
    // Onu sadece Can Barı (UI Canvas) üzerine eklemelisin, yoksa düşman yamuk yürür.

    void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y -= gravity * Time.deltaTime;
        finalMove.y = velocity.y;
    }
    
    void AttackLogic()
    {
        LookAtPlayer();
        SetAnimations(false, true, false); // IsAttack true olur
    
        if (Time.time >= nextAttackTime)
        {
            // PlayerHealth scripti olan objeye hasar ver
            PlayerHealth pHealth = player.GetComponent<PlayerHealth>();
            if (pHealth != null)
            {
                pHealth.TakeDamage(attackDamage);
            }
            nextAttackTime = Time.time + attackSpeed;
        }
    }

    void Idle()
    {
        SetAnimations(false, false, true);
    }

    void FollowPlayer()
    {
        LookAtPlayer();

        Vector3 moveDirection = (player.position - transform.position).normalized;
        moveDirection.y = 0;

        finalMove.x = moveDirection.x * moveSpeed;
        finalMove.z = moveDirection.z * moveSpeed;

        SetAnimations(true, false, false);
    }

    void LookAtPlayer()
    {
        Vector3 lookDirection = (player.position - transform.position).normalized;
        lookDirection.y = 0; 
        
        if (lookDirection != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void SetAnimations(bool isWalking, bool isAttack, bool isIdle)
    {
        if (anim == null) return;
        anim.SetBool("IsWalking", isWalking);
        anim.SetBool("IsAttack", isAttack);
        anim.SetBool("IsIdle", isIdle);
    }
    
    void OnDisable()
    {
        velocity = Vector3.zero;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}