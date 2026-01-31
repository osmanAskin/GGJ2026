using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour 
{
    public float lookRadius = 10f;
    Transform target;
    [SerializeField] private NavMeshAgent agent;
    private Animator animator;

    private void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
 
    private void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                FaceTarget();
                HandleAnimations(false, true);
            }
            else 
            {
                HandleAnimations(true, false);
            }
        }
        else 
        {
            HandleAnimations(false, false);
        }
    }

    void HandleAnimations(bool isWalking, bool isAttacking)
    {
        animator.SetBool("IsWalking", isWalking);
        animator.SetBool("IsAttack", isAttacking);
        animator.SetBool("IsIdle", !isWalking && !isAttacking);
    }
 
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}